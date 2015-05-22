using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SACAAE.Models
{
    public class repositorioSedes
    {

        private const string MuchoSede = "Sede ya existe";

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
            if (ExisteSede(sede))
                throw new ArgumentException(MuchoSede);

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

        public bool ExisteSede(Sede sede)
        {
            if (sede == null)
                return false;
            return (entidades.Sedes.SingleOrDefault(s => s.ID == sede.ID ||
                s.ID == sede.ID) != null);
        }

        public void Actualizar(Sede sede)
        {
            if (!ExisteSede(sede))
                crearSede(sede);

            var temp = entidades.Sedes.Find(sede.ID);

            if (temp != null)
            {
                entidades.Entry(temp).Property(s => s.Nombre).CurrentValue = sede.Nombre;
            }

            entidades.SaveChanges(); 
        }
    
    }
}