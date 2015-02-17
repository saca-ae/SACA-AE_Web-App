using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace SACAAE.Models
{
    public class RepositorioCursosXGrupo
    {
        private SACAAEEntities entidades = new SACAAEEntities();

        /*public IQueryable<CursosXGrupo> ObtenerCursosGrupo(int Grupo)
        {
            return from CursosXGrupo in entidades.CursosXGrupoes
                   where CursosXGrupo.Grupo==Grupo
                   select CursosXGrupo;
        }
        public IQueryable<CursosXGrupo> ObtenerCursosGrupoPeriodo(int Periodo)
        {
            return from CursosXGrupo in entidades.CursosXGrupoes
                   where CursosXGrupo.Grupo1.Periodo==Periodo
                   select CursosXGrupo;
        }

        public int Existe(int CursoBuscado ,int GrupoBuscado)
        {
            IQueryable<CursosXGrupo> Resultado= 
                from CursoXGrupo in entidades.CursosXGrupoes
                where CursoXGrupo.Grupo == GrupoBuscado && CursoXGrupo.Curso==CursoBuscado
                select CursoXGrupo;
            if (Resultado.Count() > 0)
            {
                return Resultado.FirstOrDefault().ID;
            }
            else
                return -1;
        }

        public int GuardarCursoXGrupo(int GrupoNuevo, int CursoNuevo)
        {
            CursosXGrupo CursoXGrupoNuevo = new CursosXGrupo();
            CursoXGrupoNuevo.Grupo=GrupoNuevo;
            CursoXGrupoNuevo.Curso=CursoNuevo;
            try
            {
                entidades.CursosXGrupoes.Add(CursoXGrupoNuevo);//Agrega la nueva entidad en el modelo local
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
            return CursoXGrupoNuevo.ID;
        }*/
    }
}