using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SACAAE.Models
{
    public class AlertaComisionProfesor
    {
        public int ID { get; set; }
        public string PROFESOR { get; set; }
        public String COMISION { get; set; }

        public string INCIO { get; set; }
        public String FIN { get; set; }
        public String ENTIDAD { get; set; }
        public Boolean ES_VENCIDO { get; set; }


        public static List<AlertaComisionProfesor> getComisionesAtrasados(int dias)
        { 
             SACAAEEntities entidades = new SACAAEEntities();
             AlertaComisionProfesor obj;
             List<AlertaComisionProfesor> resultado = new List<AlertaComisionProfesor>();
             var res = from s in entidades.ComisionesXProfesors
                       join c in entidades.Profesores on s.Profesor equals c.ID
                       join l in entidades.Comisiones on  s.Comision equals  l.ID
                       join y in entidades.TipoEntidads on l.FK_TipoEntidad equals y.Id
                       where DbFunctions.DiffDays(DateTime.Now, l.Fin).Value <= dias || DbFunctions.DiffDays(DateTime.Now, l.Fin).Value < 0

                       select new { ID = l.ID,Profesor = s.Profesore.Nombre , Comisiones = l.Nombre,Inicio = l.Inicio,Fin = l.Fin,Entidad = y.Nombre };
             foreach (var actual in res)
             {
                 obj = new AlertaComisionProfesor {ID = actual.ID,PROFESOR = actual.Profesor,COMISION = actual.Comisiones,
                                                           INCIO = String.Format("{0:MM/dd/yyyy}", actual.Inicio),
                                                           FIN = String.Format("{0:MM/dd/yyyy}", actual.Fin),
                 ENTIDAD = actual.Entidad};
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

        public static Boolean HayComisionesAtrasadas(int dias)
        {
            SACAAEEntities entidades = new SACAAEEntities();
            List<AlertaComisionProfesor> resultado = new List<AlertaComisionProfesor>();
            int res = (from s in entidades.ComisionesXProfesors
                      join c in entidades.Profesores on s.Profesor equals c.ID
                      join l in entidades.Comisiones on s.Comision equals l.ID
                      join y in entidades.TipoEntidads on l.FK_TipoEntidad equals y.Id
                      where DbFunctions.DiffDays(DateTime.Now, l.Fin).Value <= dias || DbFunctions.DiffDays(DateTime.Now, l.Fin).Value < 0

                      select new { ID = l.ID, Profesor = s.Profesore.Nombre, Comisiones = l.Nombre, Inicio = l.Inicio, Fin = l.Fin, Entidad = y.Nombre }).Count();
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