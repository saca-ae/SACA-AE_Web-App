using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SACAAE.Models
{
    public class repositorioSedes
    {

        private SACAAEEntities entidades;
        public repositorioSedes()
        {
            entidades = new SACAAEEntities(); 
        }
        public IQueryable tomarTodasLasSedes()
        {
            return from sede in entidades.Sedes
                   orderby sede.Nombre
                   select sede;
        }

        public IQueryable<Sede> ObtenerTodosSedes()
        {
            return from Sede in entidades.Sedes
                   orderby Sede.Nombre
                   select Sede;
        }


        public Sede ObtenerSede(int id)
        {
            return entidades.Sedes.SingleOrDefault(sede => sede.ID == id);

        }

        public Sede ObtenerSede(String nombre) {

            return entidades.Sedes.SingleOrDefault(sede => sede.Nombre == nombre);
        }

        public void crearSede(Sede sede) 
        {
            entidades.Sedes.Add(sede);
            entidades.SaveChanges();
        }

        public void borrarSede(Sede pSede) {

            var vSede = entidades.Sedes.SingleOrDefault(sede => sede.ID == pSede.ID);
            if (vSede != null)
            {
                entidades.Sedes.Remove(vSede);
                entidades.SaveChanges();
            }
            else
                return;
     
        }

        public void borrarSede(String nombre) {

            borrarSede(ObtenerSede(nombre));
        }
    
    }
}