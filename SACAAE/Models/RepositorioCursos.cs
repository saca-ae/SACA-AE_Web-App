using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SACAAE.Models
{
    public class RepositorioCursos
    {
        private SACAAEEntities entidades;

        public RepositorioCursos()
        {
            entidades = new SACAAEEntities(); 
        }

        public Curso ObtenerCurso(int id)
        {
            return entidades.Cursos.SingleOrDefault(curso => curso.ID == id);
        }

        public int IdCursos(string CursoBuscado, int PlanDeEstudioCurso)
        {

            IQueryable<Curso> Resultado =
                from Curso in entidades.Cursos
                join BloqueXPlanXCursos in entidades.BloqueXPlanXCursoes on Curso.ID equals BloqueXPlanXCursos.ID
                join BloquesXPlan in entidades.BloqueAcademicoXPlanDeEstudios on BloqueXPlanXCursos.BloqueXPlanID equals BloquesXPlan.ID
                where (Curso.Nombre == CursoBuscado && BloquesXPlan.PlanID == PlanDeEstudioCurso)
                select Curso;

            try
            {
                return Resultado.FirstOrDefault().ID;
            }
            catch (Exception e)
            {
                return -1;
            }
        }

        public int IdHorarioCurso(int grupo)
       {
            IQueryable<Detalle_Grupo> Resultado =
                from Detalle_Grupos in entidades.Detalle_Grupo
                where Detalle_Grupos.Grupo == grupo
                select Detalle_Grupos;
            Detalle_Grupo res= Resultado.FirstOrDefault();
            if (res == null)
                return 0;
            return res.Horario;
        }

        public IQueryable<Curso> ObtenerCursos(int PlanDeEstudio)
        {
            return from Curso in entidades.Cursos
                   join BloqueXPlanXCursos in entidades.BloqueXPlanXCursoes on Curso.ID equals BloqueXPlanXCursos.ID
                   join BloquesXPlan in entidades.BloqueAcademicoXPlanDeEstudios on BloqueXPlanXCursos.BloqueXPlanID equals BloquesXPlan.ID
                   where BloquesXPlan.PlanID == PlanDeEstudio
                   select Curso;
        }

        public IQueryable<Curso> ObtenerCursos()
        {
            return from Curso in entidades.Cursos
                   select Curso;
        }

        public IQueryable<Curso> ObtenerCursos(int PlanDeEstudio, int bloque)
        {
            return from curso in entidades.Cursos
                   join BloqueXPlanXCursos in entidades.BloqueXPlanXCursoes on curso.ID equals BloqueXPlanXCursos.CursoID
                   join BloquesXPlan in entidades.BloqueAcademicoXPlanDeEstudios on BloqueXPlanXCursos.BloqueXPlanID equals BloquesXPlan.ID
                   where BloquesXPlan.PlanID == PlanDeEstudio && BloquesXPlan.BloqueID == bloque
                   orderby curso.Nombre
                   select curso;
        }

        public IQueryable<Detalle_Grupo> ObtenerDetalleCursos()
        {
            return from Detalle_Curso in entidades.Detalle_Grupo
                   select Detalle_Curso;
        }

        public int CursoSinProfesor(int Profesor)
        {
            ProfesoresXCurso NuevoProfesorXCurso = new ProfesoresXCurso();
            NuevoProfesorXCurso.Profesor = Profesor;
            NuevoProfesorXCurso.Horas = 0;
            try
            {
                entidades.ProfesoresXCursoes.Add(NuevoProfesorXCurso);//Agrega la nueva entidad en el modelo local
            }
            catch (ArgumentException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new ArgumentException("El proveedor de autenticación retornó un error. Por favor, intente de nuevo. " +
                    "Si el problema persiste, por favor contacte un administrador.\n" + e.Message);
            }
            entidades.SaveChanges();//Pasa los cambios a la base de datos
            return NuevoProfesorXCurso.Id;
        }

        public int GuardarDetallesCurso(int grupo, int Horario, string Aula, int Profesor,int cupo)
        {
            Detalle_Grupo DetallesNuevos = new Detalle_Grupo();
            DetallesNuevos.Grupo = grupo;
            DetallesNuevos.Horario = Horario;
            DetallesNuevos.Aula = Aula;
            DetallesNuevos.Profesor = Profesor;
            DetallesNuevos.Cupo = cupo;
            try
            {
                entidades.Detalle_Grupo.Add(DetallesNuevos);//Agrega la nueva entidad en el modelo local
            }
            catch (ArgumentException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new ArgumentException("El proveedor de autenticación retornó un error. Por favor, intente de nuevo. " +
                    "Si el problema persiste, por favor contacte un administrador.\n" + e.Message);
            }
            entidades.SaveChanges();//Pasa los cambios a la base de datos
            return DetallesNuevos.Id;
        }
        
        public void guardarCurso(Curso curso)
        {

            if (existeCurso(curso.Nombre))
                return; 
            entidades.Cursos.Add(curso);
            Save(); 
           
        }

        public IQueryable<Curso> ObtenerCursosDePlan(int id)
        {
            return from curso in entidades.Cursos
                   join BloqueXPlanXCursos in entidades.BloqueXPlanXCursoes on curso.ID equals BloqueXPlanXCursos.ID
                   join BloquesXPlan in entidades.BloqueAcademicoXPlanDeEstudios on BloqueXPlanXCursos.BloqueXPlanID equals BloquesXPlan.ID
                   where BloquesXPlan.PlanID == id
                   orderby curso.Nombre
                   select curso; 
        }

        public String borrarCurso(int Curso)
        {
            if (existeCurso(Curso))
            {
                var existeEnPlan = from bloqueXPlanXCurso in entidades.BloqueXPlanXCursoes
                                   where bloqueXPlanXCurso.CursoID == Curso
                                   select bloqueXPlanXCurso;
                if (!existeEnPlan.Any())
                {
                    var curso = entidades.Cursos.SingleOrDefault(c => c.ID == Curso);
                    if (curso != null)
                    {
                        entidades.Cursos.Remove(curso);
                        Save();
                        return "Curso removido satisfactoriamente";
                    }
                    else
                        return "El curso no existe";
                }
                else
                    return "Debe remover el curso de los planes a los que esta asignado";


            }
                return "El curso no existe";

        }

        //public IQueryable<Detalle_Grupo> ObtenerDetalleCursos(int CursoXGrupo)
        //{
        //    return from Detalle_Curso in entidades.Detalle_Grupo
        //           where Detalle_Curso.Curso == CursoXGrupo
        //           select Detalle_Curso;
        //}

        public bool existeCurso(string Codigo)
        {
            return (entidades.Cursos.SingleOrDefault(c => c.Codigo == Codigo) != null);
           
        }

        public bool existeCursoPorNombre(string Curso)
        {
            return (entidades.Cursos.SingleOrDefault(c => c.Nombre == Curso) != null);
        }

        public bool existeCurso(int Curso)
        {

            return (entidades.Cursos.SingleOrDefault(c => c.ID == Curso) != null);

        }


        public Boolean existeCursoEnBloque(int planDeEstudio, int Bloque)
        {
            var request = from curso in entidades.Cursos
                          join BloqueXPlanXCursos in entidades.BloqueXPlanXCursoes on curso.ID equals BloqueXPlanXCursos.ID
                          join BloquesXPlan in entidades.BloqueAcademicoXPlanDeEstudios on BloqueXPlanXCursos.BloqueXPlanID equals BloquesXPlan.ID
                          where BloquesXPlan.PlanID == planDeEstudio && BloquesXPlan.BloqueID == Bloque
                          select curso;
            if (request.Any())
                return true;
            return false;
        }

        public void ModificarCurso(Curso pCurso)
        {
            var vCurso = entidades.Cursos.SingleOrDefault(curso => curso.ID == pCurso.ID);
            if (vCurso != null)
            {
                entidades.Entry(vCurso).Property(curso => curso.Codigo).CurrentValue = pCurso.Codigo;
                entidades.Entry(vCurso).Property(curso => curso.HorasPracticas).CurrentValue = pCurso.HorasPracticas;
                entidades.Entry(vCurso).Property(curso => curso.HorasTeoricas).CurrentValue = pCurso.HorasTeoricas;
                entidades.Entry(vCurso).Property(curso => curso.Nombre).CurrentValue = pCurso.Nombre;
                entidades.Entry(vCurso).Property(curso => curso.Bloque).CurrentValue = pCurso.Bloque;
                entidades.Entry(vCurso).Property(curso => curso.Externo).CurrentValue = pCurso.Externo;
                entidades.Entry(vCurso).Property(curso => curso.Creditos).CurrentValue = pCurso.Creditos;
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