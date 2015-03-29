using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SACAAE.Models
{
    public class AlertaProyectoProfesor
    {
        public int ID { get; set; }
        public string PROFESOR { get; set; }
        public String PROYECTO { get; set; }

        public string INCIO { get; set; }
        public String FIN { get; set; }
        public String ENTIDAD { get; set; }
        public Boolean ES_VENCIDO { get; set; }

        public static List<AlertaProyectoProfesor> getProyectosAtrasados(int dias)
        { 

             SACAAEEntities entidades = new SACAAEEntities();
             List<AlertaProyectoProfesor> resultado = new List<AlertaProyectoProfesor>();
             AlertaProyectoProfesor obj;
             var res = from s in entidades.ProyectosXProfesors
                       join c in entidades.Profesores on s.Profesor equals c.ID
                       join l in entidades.Proyectos on  s.Proyecto equals  l.ID
                       join y in entidades.TipoEntidads on l.FK_TipoEntidad equals y.Id
                       where DbFunctions.DiffDays(DateTime.Now, l.Fin).Value <= dias || DbFunctions.DiffDays(DateTime.Now, l.Fin).Value < 0

                       select new { ID = l.ID,Profesor = s.Profesore.Nombre , Proyectos = l.Nombre,Inicio = l.Inicio,Fin = l.Fin ,Entidad = y.Nombre};
             foreach (var actual in res)
             {
                 obj = new AlertaProyectoProfesor
                 {
                     ID = actual.ID,
                     PROFESOR = actual.Profesor,
                     PROYECTO = actual.Proyectos,
                     INCIO = String.Format("{0:MM/dd/yyyy}", actual.Inicio.Value.Date),
                     FIN = String.Format("{0:MM/dd/yyyy}", actual.Fin.Value.Date),
                     ENTIDAD = actual.Entidad
                 };
                 if (actual.Fin < DateTime.Now)
                 {
                     obj.ES_VENCIDO = true;
                 }
                 else
                 {
                     obj.ES_VENCIDO = false;
                 }
                 resultado.Add(obj);
             }
             return resultado;
        }
        public static Boolean HayProyectosAtrasadas(int dias)
        {
            SACAAEEntities entidades = new SACAAEEntities();
            List<AlertaComisionProfesor> resultado = new List<AlertaComisionProfesor>();
            int res = (from s in entidades.ProyectosXProfesors
                       join c in entidades.Profesores on s.Profesor equals c.ID
                       join l in entidades.Proyectos on s.Proyecto equals l.ID
                       join y in entidades.TipoEntidads on l.FK_TipoEntidad equals y.Id
                       where DbFunctions.DiffDays(DateTime.Now, l.Fin).Value <= dias || DbFunctions.DiffDays(DateTime.Now, l.Fin).Value < 0

                       select new { ID = l.ID, Profesor = s.Profesore.Nombre, Proyectos = l.Nombre, Inicio = l.Inicio, Fin = l.Fin, Entidad = y.Nombre }).Count();
            if (res != 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        
    }
}