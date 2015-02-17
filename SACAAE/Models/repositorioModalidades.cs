using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace SACAAE.Models
{
    public class repositorioModalidades
    {
        private SACAAEEntities entidades;

        public repositorioModalidades()
        {
            entidades = new SACAAEEntities(); 
        }

        public IQueryable<Modalidade> tomarModalidades()
        {
            return from modalidades in entidades.Modalidades
                   orderby modalidades.Nombre
                   select modalidades; 
        }

        public IQueryable<Modalidade> ObtenerTodosModalidades()
        {
            return from Modalidades in entidades.Modalidades
                   orderby Modalidades.Nombre
                   select Modalidades;
        }

        public int idModalidad(string nombre)
        {
            return (from Modalidade in entidades.Modalidades
                               where Modalidade.Nombre == nombre
                               select Modalidade).FirstOrDefault().ID;
        }
    }
}