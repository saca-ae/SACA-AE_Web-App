using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SACAAE.Models
{
    public class repositorioPlanesEstudio
    {


        private SACAAEEntities entidades = new SACAAEEntities(); 
        
        
        public IQueryable<PlanesDeEstudio> ObtenerTodosPlanes()
        {
            return from plan in entidades.PlanesDeEstudios
                   orderby plan.Nombre
                   select plan; 
                  
                   
        }

        public IQueryable ObtenerPlanesConModalidad(int IDModalidad, int IDSede)
        {
            
            
            
            return from plan in entidades.PlanesDeEstudios
                   join planXsede in entidades.PlanesDeEstudioXSedes
                   on plan.ID equals planXsede.PlanDeEstudio
                   where plan.Modalidad == IDModalidad && planXsede.Sede == IDSede
                   orderby plan.Nombre
                   select new {plan.ID, plan.Nombre, plan.Modalidad}; 
            
                   
        }
    }
}