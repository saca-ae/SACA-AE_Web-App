using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace SACAAE.Models
{
    public class HorariosController : Controller
    {
        private RepositorioPlanesDeEstudio repoPlanes = new RepositorioPlanesDeEstudio();
        private RepositorioBloqueAcademico repoBloques = new RepositorioBloqueAcademico();
        private RepositorioAulas repoAulas = new RepositorioAulas();
        private repositorioSedes repoSedes = new repositorioSedes();
        private RepositorioPeriodos repoPeriodos = new RepositorioPeriodos();
        private repositorioModalidades repoModalidades = new repositorioModalidades();
        private repositorioGrupos Grupos = new repositorioGrupos();
        private RepositorioCursos Cursos = new RepositorioCursos();
        private RepositorioHorario Horario = new RepositorioHorario();
        private RepositorioCursosXGrupo CursosXGrupo = new RepositorioCursosXGrupo();

        public ActionResult Index()
        {
            ViewBag.Modalidades = repoModalidades.ObtenerTodosModalidades();
            ViewBag.Sedes = repoSedes.ObtenerTodosSedes();
            ViewBag.Periodos = repoPeriodos.obtenerTodosPeriodos();
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Index(string button)
        {
            return RedirectToAction("Horarios");
        }
        public ActionResult Horarios()
        {
            String PlanDeEstudio;
            String Modalidad;
            String Periodo;
            
            try
            {
                PlanDeEstudio = Request.Cookies["SelPlanDeEstudio"].Value;
                Modalidad = Request.Cookies["SelModalidad"].Value;
                Periodo = Request.Cookies["PeriodoHorario"].Value;
            }
            catch (Exception e)
            {
                throw new ArgumentException("No se detecto ningun Plan de Estudio" + e.Message);
            }
            
            int IdPlanDeEstudio = Int32.Parse(PlanDeEstudio);
            int IdPeriodo = Int32.Parse(Periodo);
            List<String> Dias = new List<String>();
            Dias.Add("Lunes");
            Dias.Add("Martes");
            Dias.Add("Miercoles");
            Dias.Add("Jueves");
            Dias.Add("Viernes");
            Dias.Add("Sabado");
            Dias.Add("Domingo");
            ViewBag.Dias = Dias;

            List<String> Horas = new List<String>();
            for (int i=0;i<24;i++)
            {
                if(i<10)
                Horas.Add("0"+i.ToString());
                else
                Horas.Add(i.ToString());
            }
            ViewBag.Horas = Horas;

            List<String> Minutos = new List<String>();
            for (int i = 0; i < 60; i+=10)
            {
                if (i < 10)
                    Minutos.Add("0" + i.ToString());
                else
                    Minutos.Add(i.ToString());
            }
            int idSede= Int16.Parse(Request.Cookies["SelSede"].Value);
            ViewBag.Minutos = Minutos;
            ViewBag.Bloques = repoBloques.ListarBloquesXPlan(IdPlanDeEstudio);
            ViewBag.Aulas = repoAulas.ListarAulasXSedeCompleta(idSede);
            int idPlanXSede= repoPlanes.IdPlanDeEstudioXSede(idSede,IdPlanDeEstudio);
            return View();
        }

        [HttpPost]
        public ActionResult GuardarCambios()
        {
            int Cantidad;
            int PlanDeEstudio;
            try
            {
                 Cantidad = Convert.ToInt32(Request.Cookies["Cantidad"].Value);
            }
            catch (Exception e)
            {
                Cantidad = 0;
            }

            try
           {
                PlanDeEstudio = Int32.Parse(Request.Cookies["SelPlanDeEstudio"].Value);
            }
            catch (Exception e)
            {
                throw new ArgumentException("No se detecto ningun Grupo o Plan de Estudio" + e.Message);
            }


            if (Cantidad == 0) { return RedirectToAction("Horarios"); }//En caso de que el horario este vacio se asume que se desea borrar por lo que se limpian los dias y se termina, este return evita que falle el programa cuando no hay cookies
            //Eliminar Horarios Viejos
            for (int i = 1; i <= Cantidad; i++)
            {
                String Detalles= Request.Cookies["Cookie" + i].Value;
                string[] Partes = Detalles.Split('|');
                int Grupo = Int32.Parse(Partes[5]);
                int IdHorario = Cursos.IdHorarioCurso(Grupo);
                if(IdHorario!=0){
                Horario.EliminarDias(IdHorario);
                }

            }
            //Guardar Datos
            for (int i = 1; i <= Cantidad; i++)
            {
                String Detalles= Request.Cookies["Cookie" + i].Value;//Obtiene los datos de la cookie
                string[] Partes = Detalles.Split('|');
                String Curso = Partes[0];
                String Dia = Partes[1];
                String HoraInicio = Partes[2];
                String HoraFin = Partes[3];
                String Bloque = Partes[4];
                int Grupo = Int32.Parse(Partes[5]);
                String Aula = Partes[6];
                if (Curso != "d")
                {
                    int IdCurso = Cursos.IdCursos(Curso, PlanDeEstudio);
                    int IdHorario = Cursos.IdHorarioCurso(Grupo);
                    if (IdHorario != 0)
                    {
                        Horario.AgregarDia(Dia, IdHorario, Convert.ToInt32(HoraInicio), Convert.ToInt32(HoraFin));
                    }
                    else
                    {
                        int HorarioNuevo = Horario.NuevoHorario();
                        Horario.AgregarDia(Dia, HorarioNuevo, Convert.ToInt32(HoraInicio), Convert.ToInt32(HoraFin));
                        int idAula = repoAulas.idAula(Aula);
                        int cupo = repoAulas.ObtenerAula(idAula).Espacio;
                        Cursos.GuardarDetallesCurso(Grupo, HorarioNuevo, Aula, 5, cupo);
                    }
                }
                
            }
            Response.Cookies.Clear();
            TempData["Message"] = "Cambios guardados satisfactoriamente";
            return RedirectToAction("Horarios");
        }

        public ActionResult ObtenerHorarios(int plan, int periodo)
        {
            int idSede = Int16.Parse(Request.Cookies["SelSede"].Value);
            int idPlanXSede = repoPlanes.IdPlanDeEstudioXSede(idSede, plan);
            IQueryable listaHorarios = Horario.obtenerInfo(idPlanXSede, periodo);
                var json = JsonConvert.SerializeObject(listaHorarios);

                return Content(json);
        }

        public ActionResult ExisteHorario(string dia,int HoraInicio, int HoraFin, string aula, int grupo, int periodo)
        {
            int res = Horario.ExisteHorario(dia,HoraInicio, HoraFin, aula, grupo, periodo);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        

        public ActionResult Resultado()
        {
            return View();
        }
    }
}
