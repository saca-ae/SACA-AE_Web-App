using SACAAE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SACAAE
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "ServiceMobile" en el código, en svc y en el archivo de configuración a la vez.
    // NOTA: para iniciar el Cliente de prueba WCF para probar este servicio, seleccione ServiceMobile.svc o ServiceMobile.svc.cs en el Explorador de soluciones e inicie la depuración.
    public class ServiceMobile : IServiceMobile
    {
        public String DoWork()
        {
            return "hell0";
        }

        SACAAEEntities entidades = new SACAAEEntities();
        
        public string HelloWorld()
        {
            return "Hola a todos";
        }

        
        public string Login(String pMail)
        {
            if ((from s in entidades.Profesores
                 where s.Correo == pMail

                 select s.ID).Count() == 1)
            {
                return "1";
            }
            else
            {
                return "0";
            }
        }

        // carga estimada, horas asignadas al proyecto, "" a comision
        
        public string GetProyectos(String pMail)
        {
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<AlertaProyectoProfesor> resultado = new List<AlertaProyectoProfesor>();
            var proyectos = from s in entidades.ProyectosXProfesors
                            join c in entidades.Profesores on s.Profesor equals c.ID
                            join l in entidades.Proyectos on s.Proyecto equals l.ID
                            join y in entidades.TipoEntidads on l.FK_TipoEntidad equals y.Id
                            where c.Correo == pMail

                            select new { ID = l.ID, Profesor = c.Nombre, Proyectos = l.Nombre, Inicio = l.Inicio, Fin = l.Fin, Entidad = y.Nombre };


            foreach (var actual in proyectos)
            {

                resultado.Add(new AlertaProyectoProfesor
                {
                    ID = actual.ID,
                    PROFESOR = actual.Profesor,
                    PROYECTO = actual.Proyectos,
                    INCIO = String.Format("{0:MM/dd/yyyy}", actual.Inicio.Value.Date),
                    FIN = String.Format("{0:MM/dd/yyyy}", actual.Fin.Value.Date),
                    ENTIDAD = actual.Entidad
                });
            }

            return serializer.Serialize(resultado);
        }


        
        public string GetComisiones(String pMail)
        {
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<AlertaComisionProfesor> resultado = new List<AlertaComisionProfesor>();
            var comisiones = from s in entidades.ComisionesXProfesors
                             join c in entidades.Profesores on s.Profesor equals c.ID
                             join l in entidades.Comisiones on s.Comision equals l.ID
                             join y in entidades.TipoEntidads on l.FK_TipoEntidad equals y.Id
                             where c.Correo == pMail

                             select new { ID = l.ID, Profesor = c.Nombre, Comision = l.Nombre, Inicio = l.Inicio, Fin = l.Fin, Entidad = y.Nombre };


            foreach (var actual in comisiones)
            {
                resultado.Add(new AlertaComisionProfesor
                {
                    ID = actual.ID,
                    PROFESOR = actual.Profesor,
                    COMISION = actual.Comision,
                    INCIO = String.Format("{0:MM/dd/yyyy}", actual.Inicio.Date),
                    FIN = String.Format("{0:MM/dd/yyyy}", actual.Fin.Date),
                    ENTIDAD = actual.Entidad
                });
            }

            return serializer.Serialize(resultado);
        }

        
        public string GetCursos(String pMail)
        {
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

            var cursos = from s in entidades.ProfesoresXCursoes
                         join c in entidades.Profesores on s.Profesor equals c.ID
                         join w in entidades.Detalle_Grupo on s.Profesor equals w.Profesor
                         join g in entidades.Grupoes on w.Grupo equals g.ID
                         join d in entidades.Dias on w.Horario equals d.Horario
                         join p in entidades.BloqueXPlanXCursoes on g.ID equals p.CursoID
                         where c.Correo == pMail

                         select new
                         {
                             Profesor = c.Nombre,
                             Entidad = p.BloqueAcademicoXPlanDeEstudio.PlanesDeEstudio.TipoEntidad.Nombre,
                             Inicio = d.Hora_Inicio,
                             Fin = d.Hora_Fin,
                             Day = d.Dia1,
                             Grupoo = g.Numero,
                             Periodoo = g.Periodo1.Nombre,
                             Aulaa = w.Aula,
                             Cursoo = p.Curso.Nombre,
                             Codigo = p.Curso.Codigo
                         };
            return serializer.Serialize(cursos);
        }

    }
}
