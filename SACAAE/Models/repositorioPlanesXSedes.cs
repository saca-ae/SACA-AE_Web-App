using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SACAAE.Models
{

    public class repositorioPlanesXSedes
    {

        private SACAAEEntities entidades;

        public repositorioPlanesXSedes()
        {
            entidades = new SACAAEEntities();
        }

        public PlanesDeEstudioXSede tomarIDPlanXSede(int idSede, int idPlan)
        {
            return entidades.PlanesDeEstudioXSedes.SingleOrDefault(plansSede => plansSede.PlanDeEstudio == idPlan && plansSede.Sede == idSede);
        }
        public void agregrarPlanXSede(PlanesDeEstudioXSede planXSede)
        {
            entidades.PlanesDeEstudioXSedes.Add(planXSede);
            Save();
        }

        private void Save()
        {
            entidades.SaveChanges();
        }
    }
}