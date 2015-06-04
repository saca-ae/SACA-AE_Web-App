using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SACAAE.Models;
using Newtonsoft.Json;


namespace SACAAE.Controllers
{
    public class ProfesoresProyectosController : Controller
    {

        private RepositorioProyecto repoProyectos = new RepositorioProyecto();
        private RepositorioProfesor repoProfesores = new RepositorioProfesor();
        RepositorioProfesorProyecto repoProfesProyectos = new RepositorioProfesorProyecto();
        private RepositorioPeriodos repoPeriodos = new RepositorioPeriodos();
        private const string TempDataMessageKey = "Message";

        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Asignar()
        {


            /*EN CASO DE UTILIZAR HORARIO TEC
            
            List<String> HorasInicio = new List<String>();
            List<String> HorasFin = new List<String>();
            
            for (int i = 7; i < 22; i++)
            {
                if (i < 13)
                    HorasInicio.Add(i.ToString() + ":30");
                else
                    HorasInicio.Add(i.ToString() + ":00");
            }
            ViewBag.HorasInicio = HorasInicio;

            List<String> HorasFin = new List<String>();
            for (int i = 8; i < 22; i++)
            {
                if (i < 13)
                    HorasFin.Add(i.ToString() + ":20");
                else
                    HorasFin.Add(i.ToString() + ":50");
            }
            ViewBag.HorasFin = HorasFin;
            */

            List<String> HorasInicio = new List<String>();
            List<String> HorasFin = new List<String>();
            for (int i = 7; i < 23; i++)
            {
                HorasInicio.Add(i.ToString() + ":00");
                HorasFin.Add(i.ToString() + ":00");
            }
            ViewBag.HorasInicio = HorasInicio;
            ViewBag.HorasFin = HorasFin;

            String entidad = Request.Cookies["Entidad"].Value;
            int entidadID;

            if (entidad.Equals("TEC")) { entidadID = 1; }
            else if (entidad.Equals("CIE")) { entidadID = 7; }
            else if (entidad.Equals("TAE")) { entidadID = 5; }
            else if (entidad.Equals("MAE")) { entidadID = 6; }
            else if (entidad.Equals("DDE")) { entidadID = 11; }
            else if (entidad.Equals("Emprendedores")) { entidadID = 12; }
            else if (entidad.Equals("Actualizacion_Cartago")) { entidadID = 9; }
            else { entidadID = 8; }

            ViewBag.profesores = repoProfesores.ObtenerTodosProfesores();
            ViewBag.proyectos = repoProyectos.ObtenerProyectoXEntidad(entidadID);
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Asignar(String sltProfesor, String sltProyecto)
        {

            int Cantidad;
            try
            {
                Cantidad = Convert.ToInt32(Request.Cookies["Cantidad"].Value);
                Cantidad++;
            }
            catch (Exception e)
            {
                Cantidad = 0;
            }

            String Periodo = Request.Cookies["Periodo"].Value;
            int IdPeriodo = repoPeriodos.IdPeriodo(Periodo);

            for (int i = 1; i < Cantidad; i++)
            {
                String Detalles = Request.Cookies["DiaSeleccionadoCookie" + i].Value;//Obtiene los datos de la cookie
                string[] Partes = Detalles.Split('|');

                String Dia = Partes[0];
                String HoraInicio = Partes[1];
                String HoraFin = Partes[2];

                if (Dia != "d")
                {
                    var creado = repoProfesProyectos.CrearProyectoProfesor(sltProfesor, sltProyecto, Dia, HoraInicio, HoraFin, IdPeriodo);

                    if (creado)
                    {
                        TempData[TempDataMessageKey] = "Profesor asignado correctamente.";
                    }
                    else
                    {
                        TempData[TempDataMessageKey] = "Ocurrió un error al asignar el profesor.";
                    }
                }


            }
            return RedirectToAction("Asignar");
        }

        [Authorize]
        public ActionResult Revocar()
        {
            if (Request.UrlReferrer != null)
            {
                ViewBag.returnUrl = Request.UrlReferrer.ToString();
            }
            else
            {
                ViewBag.returnUrl = null;
            }

            /* Se obtiene la lista de profesores */
            List<Profesore> ListaProfesores = repoProfesores.ObtenerTodosProfesores().ToList<Profesore>();

            if (ListaProfesores.Count > 0)
                ViewBag.Profesores = ListaProfesores;
            else
                ViewBag.Profesores = null;

            return View();
        }


        public ActionResult ObtenerProyectosXProfesor(int idProfesor)
        {
            IQueryable listaComisiones = repoProfesProyectos.ObtenerProyectosXProfesor(idProfesor);
            if (HttpContext.Request.IsAjaxRequest())
            {
                var json = JsonConvert.SerializeObject(listaComisiones);

                return Content(json);
            }
            return View(listaComisiones);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Revocar(int sltProyectos)
        {
            var revocado = false;

            revocado = repoProfesProyectos.revocarProfesor(sltProyectos);

            if (revocado)
            {
                TempData[TempDataMessageKey] = "Profesor revocado de proyecto correctamente.";
            }
            else
            {
                TempData[TempDataMessageKey] = "Ocurrió un error al revocar el profesor.";
            }

            return RedirectToAction("Revocar");
        }
    }
}