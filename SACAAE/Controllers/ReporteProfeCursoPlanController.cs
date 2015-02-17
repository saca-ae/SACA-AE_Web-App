using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Reflection;

namespace SACAAE.Models
{
    public class ReporteProfeCursoPlanController : Controller
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
        private RepositorioProfesor repoProf = new RepositorioProfesor();
        private RepositorioCursoProfesor repoCursoPorf = new RepositorioCursoProfesor();

        [Authorize]
        public ActionResult Index()
        {
            ViewBag.Modalidades = repoModalidades.ObtenerTodosModalidades();
            ViewBag.Sedes = repoSedes.ObtenerTodosSedes();
            ViewBag.Periodos = repoPeriodos.obtenerTodosPeriodos();
            return View();
        }

        [Authorize]
        public FileResult Download()
        {
            var fi = new FileInfo("myfile.txt");
            byte[] bytes;
            try
            {
                fi.Delete();
            }
            catch (Exception e)
            { }
            using (Stream fs = new MemoryStream())
            {
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine("Codigo;Nombre;Grupo;Curso Externo;Dia;Hora Inicio;Hora Fin;Cupo;Profesor;Creditos");
                string Periodo = Request.Cookies["Periodo"].Value;
                int idPeriodo = Int16.Parse(Periodo);
                string Plan = Request.Cookies["Plan"].Value;
                int idPlan = Int16.Parse(Periodo);
                PropertyInfo[] properties = repoProf.obtenerProfeCursoPorPlan(idPlan,idPeriodo).GetType().GetProperties();
                        foreach (PropertyInfo item in properties)
                        {

        //                    string HoraInicio = (Dia.Hora_Inicio / 100).ToString() + ":" + (Dia.Hora_Inicio % 100).ToString();
        //                    string HoraFin = (Dia.Hora_Fin / 100).ToString() + ":" + (Dia.Hora_Fin % 100).ToString();
        //                    double Carga = 0;
        //                    if (!item.Curso1.Externo)
        //                    {
        //                        Carga = ((item.Curso1.HorasTeoricas * 2) + ((int)item.Curso1.HorasPracticas * 1.75));
        //                        double CargaCupo = this.CalculoCupo(Convert.ToInt32(Detalle.Cupo), Convert.ToInt32(item.Curso1.HorasTeoricas), Convert.ToInt32(item.Curso1.HorasPracticas));
        //                        Carga = Carga + CargaCupo;
        //                    }
                            sw.WriteLine(item.GetValue("Codigo",null) +";" +
                                        item.GetValue("Nombre",null) + ";" +
                                        item.GetValue("Numero",null) +";" +
                                        "si" + ";" +
                                        item.GetValue("Aula",null) + ";" +
                                        item.GetValue("Cupo",null) + ";" +
                                        item.GetValue("profe",null));
                        }
        //                                item.Grupo1.PlanesDeEstudioXSede.PlanesDeEstudio.Nombre + ";" +
        //                                item.Grupo1.PlanesDeEstudioXSede.PlanesDeEstudio.Modalidade.Nombre + ";" +
        //                                item.Grupo1.PlanesDeEstudioXSede.Sede1.Nombre + ";" +
        //                                item.Curso1.HorasTeoricas + ";" +
        //                                item.Curso1.HorasPracticas + ";" +
        //                                item.Curso1.Externo.ToString() + ";" +
        //                                Carga
        //                                 );
        //                }
        //            }
        //        }

        //        //Proyecto

        //        IQueryable<Proyecto> Proyectos = repoProyecto.ObtenerTodosProyectos();
        //        foreach (Proyecto Proyecto in Proyectos)
        //        {
        //            IQueryable<ProyectosXProfesor> Profesores = repoProyecto.ObtenerProyectoXProfesor(Proyecto.ID);
        //            foreach (ProyectosXProfesor Profe in Profesores)
        //            {
        //                IQueryable<Dia> Dias = repoHorario.ObtenerDias(Profe.Horario);
        //                foreach (Dia Dia in Dias)
        //                {
        //                    string HoraInicio = (Dia.Hora_Inicio / 100).ToString() + ":" + (Dia.Hora_Inicio % 100).ToString();
        //                    string HoraFin = (Dia.Hora_Fin / 100).ToString() + ":" + (Dia.Hora_Fin % 100).ToString();

        //                    sw.WriteLine("Proyecto;N/A;" +
        //                                Proyecto.Nombre + ";" +
        //                                Profe.Profesores.Nombre + ";" +
        //                                Dia.Dia1 + ";" +
        //                                HoraInicio + ";" +
        //                                HoraFin + ";N/A;N/A;N/A;N/A;N/A;N/A;N/A;" +
        //                                (((Dia.Hora_Fin - Dia.Hora_Inicio) / 100) * 3)
        //                                );
        //                }
        //            }
        //        }


        //        //Comisiones

        //        IQueryable<Comisione> Comisiones = repoComisiones.ObtenerTodasComisiones();
        //        foreach (Comisione Comision in Comisiones)
        //        {
        //            IQueryable<ComisionesXProfesor> Profesores = repoComisiones.ObtenerComisionesXProfesor(Comision.ID);
        //            foreach (ComisionesXProfesor Profe in Profesores)
        //            {
        //                IQueryable<Dia> Dias = repoHorario.ObtenerDias(Profe.Horario);
        //                foreach (Dia Dia in Dias)
        //                {
        //                    string HoraInicio = (Dia.Hora_Inicio / 100).ToString() + ":" + (Dia.Hora_Inicio % 100).ToString();
        //                    string HoraFin = (Dia.Hora_Fin / 100).ToString() + ":" + (Dia.Hora_Fin % 100).ToString();

        //                    sw.WriteLine("Comision;N/A;" +
        //                                Comision.Nombre + ";" +
        //                                Profe.Profesores.Nombre + ";" +
        //                                Dia.Dia1 + ";" +
        //                                HoraInicio + ";" +
        //                                HoraFin + ";N/A;N/A;N/A;N/A;N/A;N/A;N/A;" +
        //                                ((Dia.Hora_Fin - Dia.Hora_Inicio) / 100)
        //                                );
        //                }
        //            }
        //        }

                sw.Flush();
                fs.Flush();
                fs.Position = 0;
                bytes = new byte[fs.Length];
                fs.Read(bytes, 0, bytes.Length);
                sw.Close();
                sw.Dispose();

            }
            return File(bytes, System.Net.Mime.MediaTypeNames.Application.Octet, "Reporte.csv");
        //}
        }

    }
}