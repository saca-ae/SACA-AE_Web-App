﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SACAAE.Models
{
    public class RepositorioAulas
    {
        private SACAAEEntities entidades = new SACAAEEntities();

        public RepositorioAulas()
        {
            entidades = new SACAAEEntities();
        }

        public IQueryable<Aula> ListarAulas()
        {
            
            return from Aulas in entidades.Aulas
                   select Aulas;
        }

        public IQueryable<Aula> ListarAulasXEntidad(int entidadID)
        {
            if (entidadID == 1) {
                return from aula in entidades.Aulas
                       join Sedes in entidades.Sedes on aula.SedeID equals Sedes.ID
                       join PlanXSede in entidades.PlanesDeEstudioXSedes on Sedes.ID equals PlanXSede.Sede
                       join PlanDeEstudio in entidades.PlanesDeEstudios on PlanXSede.PlanDeEstudio equals PlanDeEstudio.ID
                       where PlanDeEstudio.TipoEntidad.Id == 1 || PlanDeEstudio.TipoEntidad.Id == 2 ||
                       PlanDeEstudio.TipoEntidad.Id == 3 || PlanDeEstudio.TipoEntidad.Id == 4 || PlanDeEstudio.TipoEntidad.Id == 10
                       select aula;
            }
            else {
                return from aula in entidades.Aulas
                       join Sedes in entidades.Sedes on aula.SedeID equals Sedes.ID
                       join PlanXSede in entidades.PlanesDeEstudioXSedes on Sedes.ID equals PlanXSede.Sede
                       join PlanDeEstudio in entidades.PlanesDeEstudios on PlanXSede.PlanDeEstudio equals PlanDeEstudio.ID
                       where PlanDeEstudio.TipoEntidad.Id == entidadID
                       select aula;
            }
        }


        public IQueryable ListarAulasXSede(int pSedeID)
        {
            return from Aulas in entidades.Aulas
                   where Aulas.SedeID == pSedeID
                   select new { Aulas.ID, Aulas.Codigo, Aulas.Espacio, Aulas.Activa };
        }

        public IQueryable ListarAulasXSedeCompleta(int pSedeID)
        {
            return from Aulas in entidades.Aulas
                   where Aulas.SedeID == pSedeID
                   select Aulas;
        }

        
        public IQueryable obtenerInfoAula(string aula, int periodo)
        {
            return from Dias in entidades.Dias
                   join detallesGrupo in entidades.Detalle_Grupo on Dias.Horario equals detallesGrupo.Horario
                   join grupos in entidades.Grupoes on detallesGrupo.Grupo equals grupos.ID
                   join bloqueXPlanXCurso in entidades.BloqueXPlanXCursoes on grupos.BloqueXPlanXCursoID equals bloqueXPlanXCurso.ID
                   join cursos in entidades.Cursos on bloqueXPlanXCurso.CursoID equals cursos.ID
                   where detallesGrupo.Aula == aula && grupos.Periodo == periodo
                   select new { Dias.Dia1, Dias.Hora_Inicio, Dias.Hora_Fin, cursos.Nombre, grupos.Numero, grupos.ID, detallesGrupo.Aula, bloqueXPlanXCurso.BloqueAcademicoXPlanDeEstudio.BloqueAcademico.Descripcion };
        }

        public int idAula(string pCodigoAula)
        {
            return (from Aulas in entidades.Aulas
                    where Aulas.Codigo == pCodigoAula
                    select Aulas).FirstOrDefault().ID;
        }

        public Aula ObtenerAula(int id)
        {
            return entidades.Aulas.SingleOrDefault(aula => aula.ID == id);
        }

        public bool existeAula(int pSede, string pCodigoAula)
        {
            return (entidades.Aulas.SingleOrDefault(c => c.Codigo == pCodigoAula && c.SedeID == pSede) != null);
        }


        public void agregarAula(Aula pAula)
        {
            if (existeAula(pAula.SedeID, pAula.Codigo))
                return;
            else {
                entidades.Aulas.Add(pAula);
                Save();
            }
        }

        public void eliminarAula(Aula pAula)
        {
            var vAula = entidades.Aulas.SingleOrDefault(aula => aula.ID == pAula.ID);
            if (vAula != null){
                    entidades.Aulas.Remove(vAula);
                    Save();
            }
            else
                return;
                //throw new Exception("Se ha producido un error, no se ha encontrado referencia del registro seleccionado. Por Favor comuniquese con un administrador.");
        }

        public void ModificarAula(Aula pAula)
        {
            var vAula = entidades.Aulas.SingleOrDefault(aula => aula.ID == pAula.ID);
            if (vAula != null)
            {
                entidades.Entry(vAula).Property(aula => aula.Codigo).CurrentValue = pAula.Codigo;
                entidades.Entry(vAula).Property(aula => aula.Espacio).CurrentValue = pAula.Espacio;
                entidades.Entry(vAula).Property(aula => aula.SedeID).CurrentValue = pAula.SedeID;
                entidades.Entry(vAula).Property(aula => aula.Activa).CurrentValue = pAula.Activa;
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