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
    }
}