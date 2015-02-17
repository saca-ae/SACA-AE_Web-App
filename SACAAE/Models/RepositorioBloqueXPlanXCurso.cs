using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SACAAE.Models
{
    public class RepositorioBloqueXPlanXCurso
    {

        private SACAAEEntities entidades = new SACAAEEntities();


        public IQueryable<Curso> ListarCursosXBloque(int pBloqueID)
        {
            return from cursos in entidades.Cursos
                   join BPC in entidades.BloqueXPlanXCursoes on cursos.ID equals BPC.CursoID
                   join BloquesXPlan in entidades.BloqueAcademicoXPlanDeEstudios on BPC.BloqueXPlanID equals BloquesXPlan.ID
                   where BloquesXPlan.BloqueID == pBloqueID
                   select cursos;
        }

        public IQueryable<Curso> ListarCursosXPlan(int pPlanID)
        {
            return from cursos in entidades.Cursos
                   join BPC in entidades.BloqueXPlanXCursoes on cursos.ID equals BPC.CursoID
                   join BloquesXPlan in entidades.BloqueAcademicoXPlanDeEstudios on BPC.BloqueXPlanID equals BloquesXPlan.ID
                   where BloquesXPlan.PlanID == pPlanID
                   select cursos;
        }

        public Curso ListarCursoXID(int ID)
        {
            return (from cursos in entidades.Cursos
                   join BPC in entidades.BloqueXPlanXCursoes on cursos.ID equals BPC.CursoID
                   where BPC.ID == ID
                   select cursos).FirstOrDefault();
        }

        public bool existeRelacionBloqueXPlanXCurso(int pBloqueXPlanID, int pCursoID)
        {
            return (entidades.BloqueXPlanXCursoes.SingleOrDefault(relacion => relacion.BloqueXPlanID == pBloqueXPlanID && relacion.CursoID == pCursoID) != null);
        }

        public int obtenerBloqueXPlanXCursoID(int pBloqueXPlanID, int pCursoID)
        {
            return (entidades.BloqueXPlanXCursoes.SingleOrDefault(relacion => relacion.BloqueXPlanID == pBloqueXPlanID && relacion.CursoID == pCursoID).ID);
        }

        public bool existeRelacionCursoEnPlan(int pPlanID, int pCursoID)
        {
            var request = from cursos in entidades.Cursos
                          join BloqueXPlanXCursos in entidades.BloqueXPlanXCursoes on cursos.ID equals BloqueXPlanXCursos.CursoID
                          join BloqueXPlan in entidades.BloqueAcademicoXPlanDeEstudios on BloqueXPlanXCursos.BloqueXPlanID equals BloqueXPlan.ID
                          where cursos.ID == pCursoID && BloqueXPlan.PlanID == pPlanID
                          select BloqueXPlanXCursos;
            if (request.Any())
                return true;
            return false;
        }

        public void crearRelacionBloqueXPlanXCurso(BloqueXPlanXCurso pBloqueXPlanXCurso)
        {
            if (existeRelacionBloqueXPlanXCurso(pBloqueXPlanXCurso.BloqueXPlanID, pBloqueXPlanXCurso.CursoID))
                return;
            else
            {
                entidades.BloqueXPlanXCursoes.Add(pBloqueXPlanXCurso);
                Save();
            }
        }

        public void modificarCursoBloquePlan(int idBloqueXplan, int idBloque)
        {
            var vBloqueXPlanXCurso = entidades.BloqueXPlanXCursoes.SingleOrDefault(bloqueXplanXcurso => bloqueXplanXcurso.BloqueXPlanID == idBloqueXplan);
            if (vBloqueXPlanXCurso != null)
            {
                entidades.Entry(vBloqueXPlanXCurso).Property(bloqueXplanXcurso => bloqueXplanXcurso.BloqueXPlanID).CurrentValue = idBloque;
                Save();
            }
            else
                return;
        }

        public void eliminarCursoBloquePlan(int pBloqueXPlanID, int curso)
        {
            var vBloque = entidades.BloqueXPlanXCursoes.SingleOrDefault(bloque => bloque.BloqueXPlanID == pBloqueXPlanID && bloque.CursoID==curso);
            if (vBloque != null)
            {
                entidades.BloqueXPlanXCursoes.Remove(vBloque);
                Save();
            }
            else
                return;
            //throw new Exception("Se ha producido un error, no se ha encontrado referencia del registro seleccionado. Por Favor comuniquese con un administrador.");
        }
        private void Save()
        {
            entidades.SaveChanges();
        }

    }
}