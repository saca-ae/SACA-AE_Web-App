using SACAAE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace SACAAE
{
    /// <summary>
    /// Descripción breve de WebServiceMobile
    /// </summary>
    [WebService(Namespace = "http://saca-ae.net/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    public class WebServiceMobile : System.Web.Services.WebService
    {
        SACAAEEntities entidades = new SACAAEEntities();
        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public AlertaComisionProfesor HelloWorld()
        {
            return new AlertaComisionProfesor { COMISION = "RASTA" };
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public Pass Login(String pMail)
        {
            if ((from s in entidades.Profesores
                 where s.Correo == pMail

                 select s.ID).Count() == 1)
            {
                return new Pass { Password = "1"};
            }
            else
            {
                return new Pass { Password = "0" };
            }
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public Pass LoginContra(String pMail,string pContrasenia)
        {
            if ((from s in entidades.Profesores
                 where s.Correo == pMail && s.Contrasenia == pContrasenia

                 select s.ID).Count() == 1)
            {
                return new Pass { Password = "1" };
            }
            else
            {
                return new Pass { Password = "0" };
            }
        }


        // carga estimada, horas asignadas al proyecto, "" a comision
        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public List<AlertaProyectoProfesor> GetProyectos(String pMail)
        {
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<AlertaProyectoProfesor> resultado = new List<AlertaProyectoProfesor>();
            var proyectos = from s in entidades.ProyectosXProfesors
                            join c in entidades.Profesores on s.Profesor equals c.ID
                            join l in entidades.Proyectos on s.Proyecto equals l.ID
                            join y in entidades.TipoEntidads on l.FK_TipoEntidad equals y.Id
                            where c.Correo == pMail

                            select new { ID = l.ID,Profesor = c.Nombre,Proyectos = l.Nombre, Inicio = l.Inicio, Fin = l.Fin, Entidad = y.Nombre };
            

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

            return resultado;
        }
    

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public List<AlertaComisionProfesor> GetComisiones(String pMail)
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

            return resultado;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public List<CursoWS> GetCursos(String pMail)
        {
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<CursoWS> resultado = new List<CursoWS>();
            var cursos = from s in entidades.ProfesoresXCursoes
                            join c in entidades.Profesores on s.Profesor equals c.ID
                            join w in entidades.Detalle_Grupo on s.Profesor equals w.Profesor
                            join g in entidades.Grupoes on w.Grupo equals g.ID
                            join d in entidades.Dias on w.Horario equals d.Horario
                            join p in entidades.BloqueXPlanXCursoes on g.ID equals p.CursoID
                            where c.Correo == pMail

                            select new {ID= g.ID, Profesor = c.Nombre,  Entidad = p.BloqueAcademicoXPlanDeEstudio.PlanesDeEstudio.TipoEntidad.Nombre, Inicio = d.Hora_Inicio,Fin = d.Hora_Fin, Day = d.Dia1,
                            Grupoo = g.Numero,Periodoo = g.Periodo1.Nombre, Aulaa = w.Aula,Cursoo = p.Curso.Nombre,Codigo = p.Curso.Codigo};


            foreach (var actual in cursos)
            {
                resultado.Add(new CursoWS
                {
                    Id = actual.ID+"",
                    Profesor = actual.Profesor,
                    Grupo = actual.Grupoo+"",
                    Periodo = actual.Periodoo,
                    Aula = actual.Aulaa,
                    Codigo = actual.Codigo,
                    Curso = actual.Cursoo,
                    Inicio = actual.Inicio.Value.ToString(),
                    Fin =  actual.Fin.Value.ToString(),
                    Entidad = actual.Entidad
                });
            }

            return resultado;
        }
    }

     public class Pass
    {
        public String Password { get; set; }
    }
}
