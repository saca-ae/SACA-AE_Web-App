using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SACAAE.Models
{
    public class repositorioGrupos
    {
        private SACAAEEntities entidades;

        
        public repositorioGrupos()
        {
            entidades = new SACAAEEntities(); 
        }

        public IQueryable ListarGruposXSedeXPeriodo(int pPlanID, int pPeriodoID)
        {
            return from Grupos in entidades.Grupoes
                   join BloqueXPlanXCursos in entidades.BloqueXPlanXCursoes on Grupos.BloqueXPlanXCursoID equals BloqueXPlanXCursos.ID
                   join Cursos in entidades.Cursos on BloqueXPlanXCursos.CursoID equals Cursos.ID
                   where Grupos.PlanDeEstudio == pPlanID && Grupos.Periodo == pPeriodoID
                   select new { Grupos.ID, Grupos.Numero, Cursos.Nombre};
        }

        public Grupo obtenerUnGrupo(int id)
        {
            return entidades.Grupoes.SingleOrDefault(grupo => grupo.ID == id);
        }

        public Detalle_Grupo obtenerUnDetalleGrupo(int grupo)
        {
            return entidades.Detalle_Grupo.SingleOrDefault(detalleGrupo => detalleGrupo.Grupo == grupo);
        }
        public int ObtenerUltimoNumeroGrupo(int PlanXSedeID, int PeriodoID, int BloqueXPlanXCursoID)
        {
            var vGrupos = from grupos in entidades.Grupoes
                          where grupos.PlanDeEstudio == PlanXSedeID && grupos.Periodo == PeriodoID && grupos.BloqueXPlanXCursoID == BloqueXPlanXCursoID
                          orderby grupos.Numero descending
                           select grupos;
            if (vGrupos.Any())
                return vGrupos.First().Numero;
            else
                return 0;
        }

        public void eliminarGrupo(Grupo pGrupo)
        {
            var vGrupo = entidades.Grupoes.SingleOrDefault(grupo => grupo.ID == pGrupo.ID);
            if (vGrupo != null)
            {
                entidades.Grupoes.Remove(vGrupo);
                Save();
            }
            else
                return;
        }

        public IQueryable<Grupo> ObtenerTodosGrupos()
        {
            return from Grupo in entidades.Grupoes
                   orderby Grupo.Numero
                   select Grupo;
        }

        public IQueryable<Grupo> ObtenerTodosGrupos(int periodo)
        {
            return from Grupo in entidades.Grupoes
                   where Grupo.Periodo==periodo
                   orderby Grupo.Numero
                   select Grupo;
        }

        public IQueryable<Grupo> ListaGrupos(int PlanDeEstudio, int Periodo, int Bloque)
        {
            var idBloque = from bloqueXPlanXCurso in entidades.BloqueXPlanXCursoes
                           join bloqueXPlan in entidades.BloqueAcademicoXPlanDeEstudios on bloqueXPlanXCurso.BloqueXPlanID equals bloqueXPlan.ID
                           select bloqueXPlan.BloqueID;
            return from Grupo in entidades.Grupoes
                   where Grupo.PlanDeEstudio == PlanDeEstudio && Grupo.BloqueXPlanXCursoID == Bloque && Grupo.Periodo == Periodo
                   select Grupo;
        }
        
        //public bool existeGrupo(string name, int periodo, int IDplan)
        //{
        //    return (entidades.Grupoes.SingleOrDefault(c => c.Nombre == name && 
        //                                                                c.Periodo == periodo && c.PlanDeEstudio ==IDplan) != null); 
        //}
        //public IQueryable<Grupo> tomarGruposconCondiciones(int plan, int bloque, int periodo)
        //{
        //    return from grupo in entidades.Grupoes
        //           where grupo.PlanDeEstudio == plan &&
        //           grupo.Periodo == periodo &&
        //           grupo.Bloque == bloque
        //           orderby grupo.Nombre
        //           select grupo; 
        //}

        public void agregarGrupo(Grupo pGrupo)
        {
                entidades.Grupoes.Add(pGrupo);
                Save();

        }

        //public void eliminarGrupo(int id)
        //{
        //    var temp = entidades.Grupoes.Find(id);
        //    if (temp != null)
        //    {
        //        entidades.Grupoes.Remove(temp);
        //    }
        //    Save();
        //}


        private void Save()
        {
            entidades.SaveChanges();
        }
    }
}