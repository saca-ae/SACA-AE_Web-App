using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using System.Text;
namespace SACAAE.Models
{
    public class ReportesController : Controller
    {
        private repositorioGrupos repoGrupos = new repositorioGrupos();
        private RepositorioCursos repoCursos = new RepositorioCursos();
        private RepositorioHorario repoHorario = new RepositorioHorario();
        private RepositorioProyecto repoProyecto = new RepositorioProyecto();
        private RepositorioComision repoComisiones = new RepositorioComision();
        private RepositorioPeriodos repoPeriodos = new RepositorioPeriodos();
        private RepositorioBloqueXPlanXCurso repoBloqueXPlanXCurso = new RepositorioBloqueXPlanXCurso();

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
                StreamWriter sw = new StreamWriter(fs,Encoding.UTF8);
                sw.WriteLine("Tipo;Grupo;Nombre;Profesor;Dia;Hora Inicio;Hora Fin;Cupo;Plan de Estudio;Modalidad;Sede; Horas Teoricas; Horas Practicas; Externo;Carga Estimada");
                String Periodo = Request.Cookies["Periodo"].Value;
                int IdPeriodo = repoPeriodos.IdPeriodo(Periodo);
                IQueryable<Grupo> Grupos = repoGrupos.ObtenerTodosGrupos(IdPeriodo);
                foreach (Grupo item in Grupos)
                {
                    Detalle_Grupo Detalle = repoGrupos.obtenerUnDetalleGrupo(item.ID);
                    if (Detalle != null)
                    {
                        IQueryable<Dia> Dias = repoHorario.ObtenerDias(Detalle.Horario);
                        foreach (Dia Dia in Dias)
                        {

                            string HoraInicio = (Dia.Hora_Inicio / 100).ToString() + ":" + (Dia.Hora_Inicio % 100).ToString();
                            string HoraFin = (Dia.Hora_Fin / 100).ToString() + ":" + (Dia.Hora_Fin % 100).ToString();
                            int Carga = 0;
                            Curso CursoInfo = repoBloqueXPlanXCurso.ListarCursoXID(item.BloqueXPlanXCursoID);
                            //if (!CursoInfo.Externo)
                            //{
                                Carga=(int)Detalle.ProfesoresXCurso.Horas;  
                                /*Carga = ((CursoInfo.HorasTeoricas * 2) + ((int)CursoInfo.HorasPracticas * 1.75));
                                double CargaCupo = this.CalculoCupo(Convert.ToInt32(Detalle.Cupo), Convert.ToInt32(CursoInfo.HorasTeoricas), Convert.ToInt32(CursoInfo.HorasPracticas));
                                Carga = Carga + CargaCupo;*/
                            //}
                            sw.WriteLine("Curso;" +
                                        item.Numero + ";" +
                                        CursoInfo.Nombre + ";" +
                                        Detalle.ProfesoresXCurso.Profesore.Nombre + ";" +
                                        Dia.Dia1 + ";" +
                                        HoraInicio + ";" +
                                        HoraFin + ";" +
                                        Detalle.Cupo + ";" +
                                        item.BloqueXPlanXCurso.BloqueAcademicoXPlanDeEstudio.PlanesDeEstudio.Nombre + ";" +
                                        item.BloqueXPlanXCurso.BloqueAcademicoXPlanDeEstudio.PlanesDeEstudio.Modalidade.Nombre + ";" +
                                        item.PlanesDeEstudioXSede.Sede1.Nombre + ";" +
                                        item.BloqueXPlanXCurso.Curso.HorasTeoricas + ";" +
                                        item.BloqueXPlanXCurso.Curso.HorasPracticas + ";" +
                                        item.BloqueXPlanXCurso.Curso.Externo.ToString() + ";" +
                                        Carga
                                         );
                        }

                    }
                    
                    }
                

                //Proyecto

                IQueryable<Proyecto> Proyectos = repoProyecto.ObtenerTodosProyectos();
                foreach (Proyecto Proyecto in Proyectos)
                {
                    IQueryable<ProyectosXProfesor> Profesores = repoProyecto.ObtenerProyectoXProfesor(Proyecto.ID);
                    foreach (ProyectosXProfesor Profe in Profesores)
                    {
                        IQueryable<Dia> Dias = repoHorario.ObtenerDias((int)Profe.Horario);
                        foreach (Dia Dia in Dias)
                        {
                            string HoraInicio = (Dia.Hora_Inicio / 100).ToString() + ":" + (Dia.Hora_Inicio % 100).ToString();
                            string HoraFin = (Dia.Hora_Fin / 100).ToString() + ":" + (Dia.Hora_Fin % 100).ToString();
                            double CargaC = (double)(Dia.Hora_Fin - Dia.Hora_Inicio) / 100;
                            sw.WriteLine("Proyecto;N/A;" +
                                        Proyecto.Nombre + ";" +
                                        Profe.Profesore.Nombre + ";" +
                                        Dia.Dia1 + ";" +
                                        HoraInicio + ";" +
                                        HoraFin + ";N/A;N/A;N/A;N/A;N/A;N/A;N/A;" +
                                        Math.Ceiling(CargaC)
                                        );
                        }
                    }
                }


                //Comisiones

                IQueryable<Comisione> Comisiones = repoComisiones.ObtenerTodasComisiones();
                foreach (Comisione Comision in Comisiones)
                {
                    IQueryable<ComisionesXProfesor> Profesores = repoComisiones.ObtenerComisionesXProfesor(Comision.ID);
                    foreach (ComisionesXProfesor Profe in Profesores)
                    {
                        IQueryable<Dia> Dias = repoHorario.ObtenerDias((int)Profe.Horario);
                        foreach (Dia Dia in Dias)
                        {
                            string HoraInicio = (Dia.Hora_Inicio / 100).ToString() + ":" + (Dia.Hora_Inicio % 100).ToString();
                            string HoraFin = (Dia.Hora_Fin / 100).ToString() + ":" + (Dia.Hora_Fin % 100).ToString();
                            double CargaC=(double)(Dia.Hora_Fin-Dia.Hora_Inicio)/100;

                            sw.WriteLine("Comision;N/A;" +
                                        Comision.Nombre + ";" +
                                        Profe.Profesore.Nombre + ";" +
                                        Dia.Dia1 + ";" +
                                        HoraInicio + ";" +
                                        HoraFin + ";N/A;N/A;N/A;N/A;N/A;N/A;N/A;" +
                                        Math.Ceiling(CargaC)
                                        );
                        }
                    }
                }

                sw.Flush();
                fs.Flush();
                fs.Position = 0;
                bytes = new byte[fs.Length];
                fs.Read(bytes, 0, bytes.Length);
                sw.Close();
                sw.Dispose();

            }
            return File(bytes, System.Net.Mime.MediaTypeNames.Application.Octet, "Reporte.csv");
        }


        public double CalculoCupo(int Cupo, int HorasTeoria, int HorasPractica)
        {

            double resultado = 0;
            if (HorasPractica == 0)
            {
                if (Cupo < 15)
                {
                    resultado = 2;
                }
                else if (Cupo < 25)
                {
                    resultado = 3;
                }
                else if (Cupo < 35)
                {
                    resultado = 4;
                }
                else if (Cupo < 45)
                {
                    resultado = 5;
                }
                else
                {
                    resultado = 6;
                }

                if (HorasTeoria >= 5)
                {
                    resultado = resultado + 0.75;
                }
            }
            if (HorasTeoria == 0)
            {
                if (Cupo < 15)
                {
                    resultado = 3;
                }
                else if (Cupo < 25)
                {
                    resultado = 4.5;
                }
                else
                {
                    resultado = 6;
                }
            }
            else
            {
                if (Cupo < 15)
                {
                    resultado = 2.5;
                }
                else if (Cupo < 25)
                {
                    resultado = 3.75;
                }
                else if (Cupo < 35)
                {
                    resultado = 5.25;
                }
                else
                {
                    resultado = 6.5;
                }
            }
            return resultado;
        }

    }
}
