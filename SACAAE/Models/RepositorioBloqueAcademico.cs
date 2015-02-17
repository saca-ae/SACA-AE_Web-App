using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SACAAE.Models
{
    public class RepositorioBloqueAcademico
    {
        private SACAAEEntities entidades = new SACAAEEntities();


        public IQueryable<BloqueAcademico> ListarBloquesAcademicos()
        {
            return from BloquesAcademicos in entidades.BloqueAcademicoes
                   select BloquesAcademicos;
        }

        public IQueryable<BloqueAcademico> ListarBloquesXPlan(int pPlanID)
        {
            return from Bloques in entidades.BloqueAcademicoes
                   join BloquesXPlan in entidades.BloqueAcademicoXPlanDeEstudios on Bloques.ID equals BloquesXPlan.BloqueID 
                   where BloquesXPlan.PlanID == pPlanID
                   select Bloques;
        }


        /*public IQueryable<Aula> ListarAulasXSede(int pSedeID)
        {
            return from Aulas in entidades.Aulas
                   where Aulas.SedeID == pSedeID
                   select Aulas;
        }*/


        public int idBloqueAcademico(string pDescripcion)
        {
            return (from BloquesAcademicos in entidades.BloqueAcademicoes
                    where BloquesAcademicos.Descripcion == pDescripcion
                    select BloquesAcademicos).FirstOrDefault().ID;
        }

        public BloqueAcademico obtenerBloqueAcademico(int id)
        {
            return entidades.BloqueAcademicoes.SingleOrDefault(bloque => bloque.ID == id);
        }

        public IQueryable<BloqueAcademico> obtenerBloques(int PlanDeEstudio)
        {
            return from BloquesAcademicos in entidades.BloqueAcademicoes
                   join BloquesXPlan in entidades.BloqueAcademicoXPlanDeEstudios on BloquesAcademicos.ID equals BloquesXPlan.BloqueID
                   where BloquesXPlan.PlanID == PlanDeEstudio
                   select BloquesAcademicos;
        }

        public bool existeBloque(string pDescripcion)
        {
            return (entidades.BloqueAcademicoes.SingleOrDefault(bloque => bloque.Descripcion == pDescripcion) != null);
        }


        public void agregarBloqueAcademico(BloqueAcademico pBloqueAcademico)
        {
            if (existeBloque(pBloqueAcademico.Descripcion))
                return;
            else
            {
                entidades.BloqueAcademicoes.Add(pBloqueAcademico);
                Save();
            }
        }

        public void eliminarBloqueAcademico(BloqueAcademico pBloqueAcademico)
        {
            var vBloque = entidades.BloqueAcademicoes.SingleOrDefault(bloque => bloque.ID == pBloqueAcademico.ID);
            if (vBloque != null)
            {
                entidades.BloqueAcademicoes.Remove(vBloque);
                Save();
            }
            else
                return;
            //throw new Exception("Se ha producido un error, no se ha encontrado referencia del registro seleccionado. Por Favor comuniquese con un administrador.");
        }

        public void modificarBloqueAcademico(BloqueAcademico pBloqueAcademico)
        {
            var vBloque = entidades.BloqueAcademicoes.SingleOrDefault(bloque => bloque.ID == pBloqueAcademico.ID);
            if (vBloque != null)
            {
                entidades.Entry(vBloque).Property(bloque => bloque.Descripcion).CurrentValue = pBloqueAcademico.Descripcion;
                entidades.Entry(vBloque).Property(bloque => bloque.Nivel).CurrentValue = pBloqueAcademico.Nivel;
                Save();
            }
            else
                return;
        }

        
        private void Save()
        {
            entidades.SaveChanges();
        }
    }
}