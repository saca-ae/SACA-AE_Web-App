﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace SACAAE.Models
{
    public class RepositorioProfesor
    {
        private SACAAEEntities entidades = new SACAAEEntities();

        private const String FaltaProfesor = "Profesor no existe";
        private const String MuchoProfesor = "Profesor ya existe";

        public int NumeroProfesores
        {
            get
            {
                return this.entidades.Profesores.Count();
            }
        }

        public RepositorioProfesor()
        {
            entidades = new SACAAEEntities();
        }

        public IQueryable<Profesore> ObtenerTodosProfesores()
        {
            return from profesor in entidades.Profesores
                   orderby profesor.Nombre
                   where profesor.Estado == 1
                   select profesor;
        }

        public Profesore ObtenerProfesor(int id)
        {
            return entidades.Profesores.SingleOrDefault(profesor => profesor.ID == id);
        }

        public Profesore ObtenerProfesor(string nombre)
        {
            return entidades.Profesores.SingleOrDefault(profesor => profesor.Nombre == nombre);
        }

        private void AgregarProfesor(Profesore profesor)
        {
            if (ExisteProfesor(profesor))
                throw new ArgumentException(MuchoProfesor);
            entidades.Profesores.Add(profesor);
        }

        public void CrearProfesor(String nombre, String link, int estado)
        {
            if (string.IsNullOrEmpty(nombre.Trim()))
                throw new ArgumentException("El nombre del profesor no es válido. Por favor, inténtelo de nuevo");
            //if (string.IsNullOrEmpty(plaza.Trim()))
                //throw new ArgumentException("El código de la plaza no es válido. Por favor, inténtelo de nuevo");
        
            
            Profesore profesorNuevo = new Profesore()
            {
                Nombre = nombre,
                //Plaza = plaza,
                Link = link,
                Estado = estado
            
            };

            try
            {
                AgregarProfesor(profesorNuevo);
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

            Save();
        }

        public void BorrarProfesor(Profesore profesor)
        {
            if (!ExisteProfesor(profesor))
                throw new ArgumentException(FaltaProfesor);

            var temp = entidades.Profesores.Find(profesor.ID);
            if (temp != null)
            {
                entidades.Entry(temp).Property(p => p.Estado).CurrentValue = 2; /*Se pone en 2 porque en la BD se definió este como inactivo*/
            }
            Save();
        }

        public void BorrarProfesor(string nombre)
        {
            BorrarProfesor(ObtenerProfesor(nombre));
        }

        public void Actualizar(Profesore profesor)
        {
            if (!ExisteProfesor(profesor))
                AgregarProfesor(profesor);

            var temp = entidades.Profesores.Find(profesor.ID);

            if (temp != null)
            {
                entidades.Entry(temp).Property(p => p.Nombre).CurrentValue = profesor.Nombre;
                //entidades.Entry(temp).Property(p => p.Plaza).CurrentValue = profesor.Plaza;
                entidades.Entry(temp).Property(p => p.Link).CurrentValue = profesor.Link;
            }

            Save();
        }
        
        /*Helpers*/
        public bool ExisteProfesor(Profesore profesor)
        {
            if (profesor == null)
                return false;
            return (entidades.Profesores.SingleOrDefault(p => p.ID == profesor.ID ||
                p.Nombre == profesor.Nombre) != null);
        }

        public IQueryable obtenerProfeCursoPorPlan(int plan, int periodo)
        {
            var profe = "profe";
            return from Curso in entidades.Cursos
                   join BloqueXPlanXCursos in entidades.BloqueXPlanXCursoes on Curso.ID equals BloqueXPlanXCursos.CursoID
                   join BloquesXPlan in entidades.BloqueAcademicoXPlanDeEstudios on BloqueXPlanXCursos.BloqueXPlanID equals BloquesXPlan.ID
                   join grupo in entidades.Grupoes on BloqueXPlanXCursos.ID equals grupo.BloqueXPlanXCursoID
                   join detalleGrupo in entidades.Detalle_Grupo on grupo.ID equals detalleGrupo.Grupo
                   join profesorxCurso in entidades.ProfesoresXCursoes on detalleGrupo.Profesor equals profesorxCurso.Id
                   join profesor in entidades.Profesores on profesorxCurso.Profesor equals profesor.ID
                   where BloquesXPlan.PlanID == plan && grupo.Periodo == periodo
                   select new { Curso.Codigo, Curso.Nombre, Curso.Externo,grupo.Numero,grupo.ID,detalleGrupo.Aula,detalleGrupo.Cupo,profe=profesor.Nombre,Curso.Creditos};

        }
        public void Save()
        {
            entidades.SaveChanges();            
        }
    }
}