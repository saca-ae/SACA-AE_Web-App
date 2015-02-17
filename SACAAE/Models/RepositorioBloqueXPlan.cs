using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SACAAE.Models
{
    public class RepositorioBloqueXPlan
    {
        private SACAAEEntities entidades = new SACAAEEntities();


        public IQueryable<BloqueAcademicoXPlanDeEstudio> ListarBloquesXPlan(int pPlanID)
        {
            return from BloquesXPlan in entidades.BloqueAcademicoXPlanDeEstudios
                   join Planes in entidades.PlanesDeEstudios on BloquesXPlan.PlanID equals Planes.ID
                   join Bloque in entidades.BloqueAcademicoes on BloquesXPlan.BloqueID equals Bloque.ID
                   where Planes.ID == pPlanID
                   select BloquesXPlan;
        }

        public int obtenerIdBloqueXPlan(int pPlanID, int pBloqueID)
        {
            return (entidades.BloqueAcademicoXPlanDeEstudios.SingleOrDefault(relacion => relacion.PlanID == pPlanID && relacion.BloqueID == pBloqueID).ID);
        }
        public bool existeRelacionBloqueXPlan(int pPlanID, int pBloqueID)
        {
            return (entidades.BloqueAcademicoXPlanDeEstudios.SingleOrDefault(relacion => relacion.PlanID == pPlanID && relacion.BloqueID == pBloqueID) != null);
        }

        public int idBloqueXPlan(int pPlanID,int pBloqueID)
        {
            return (from BloquesXPlan in entidades.BloqueAcademicoXPlanDeEstudios
                    where BloquesXPlan.PlanID == pPlanID && BloquesXPlan.BloqueID == pBloqueID
                    select BloquesXPlan).FirstOrDefault().ID;
        }

        public void crearRelacionBloqueXPlan(BloqueAcademicoXPlanDeEstudio pBloqueXPlan)
        {
            if (existeRelacionBloqueXPlan(pBloqueXPlan.PlanID, pBloqueXPlan.BloqueID))
                return;
            else
            {
                entidades.BloqueAcademicoXPlanDeEstudios.Add(pBloqueXPlan);
                Save();
            }
        }

        public string eliminarBloquePlan(int pBloqueXPlanID)
        {

            var vBloques = (from BloquesXPlanXCursos in entidades.BloqueXPlanXCursoes
                            where BloquesXPlanXCursos.BloqueXPlanID == pBloqueXPlanID
                            select BloquesXPlanXCursos);
            if (vBloques != null)
            {
                foreach (var vBloque in vBloques){
                    var CursosAsignados = from Grupos in entidades.Grupoes
                                          where Grupos.BloqueXPlanXCursoID == vBloque.ID
                                          select Grupos;
                    if (CursosAsignados.Any())
                        return "Debe eliminar los grupos de este curso";
                    entidades.BloqueXPlanXCursoes.Remove(vBloque);
                }
                var vBloquePlan = entidades.BloqueAcademicoXPlanDeEstudios.SingleOrDefault(bloquePlan => bloquePlan.ID == pBloqueXPlanID);
                entidades.BloqueAcademicoXPlanDeEstudios.Remove(vBloquePlan);
                Save();
            }
            else
                return "El registro ha sido borrado correctamente.";
            return "El registro no ha sido borrado.";
            //throw new Exception("Se ha producido un error, no se ha encontrado referencia del registro seleccionado. Por Favor comuniquese con un administrador.");
        }

        private void Save()
        {
            entidades.SaveChanges();
        }

    }
}