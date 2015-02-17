using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SACAAE.Models
{
    public class RepositorioPlazaProfesor
    {
        private SACAAEEntities entidades = new SACAAEEntities();

        public void AsignarPlaza(string codigoPlaza, string codigoProfesor, int? horasAsignadas)
        {
            // if (string.IsNullOrEmpty(codigoPlaza.Trim()))
            //   throw new ArgumentException("El codigo de la plaza no es válido. Por favor, intente de nuevo.");

           
            RepositorioProfesor repoProfesor = new RepositorioProfesor();
            RepositorioPlazas repoPlaza = new RepositorioPlazas();

            var IDProfesor = repoProfesor.ObtenerProfesor(Int16.Parse(codigoProfesor));
            var IDPlaza = repoPlaza.ObtenerPlaza(Int16.Parse(codigoPlaza));

            PlazaXProfesor asignarPlaza = new PlazaXProfesor()
            {
                Plaza = Int16.Parse(IDPlaza.ID.ToString()),
                Profesor = Int16.Parse(IDProfesor.ID.ToString()),
                Horas_Asignadas = horasAsignadas
            };
            if (!ExistePlaza(asignarPlaza))
            {
                try
                {
                    AgregarPlazaProfesor(asignarPlaza);
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
            }
            else
            {
                asignarPlaza = repoPlaza.ObtenerPlazaXProfesor(IDPlaza.ID, IDProfesor.ID);
                asignarPlaza.Horas_Asignadas = horasAsignadas;
                Actualizar(asignarPlaza);
            }

            Save();
        }

        public void QuitarNombramiento(string codigoPlaza, string codigoProfesor)
        {
            RepositorioProfesor repoProfesor = new RepositorioProfesor();
            RepositorioPlazas repoPlaza = new RepositorioPlazas();
            var IDProfesor = repoProfesor.ObtenerProfesor(Int16.Parse(codigoProfesor));
            var IDPlaza = repoPlaza.ObtenerPlaza(Int16.Parse(codigoPlaza));
            Console.Write(IDPlaza);
            var temp = entidades.PlazaXProfesors.FirstOrDefault(pp=>pp.Plaza==IDPlaza.ID);
            Console.Write(temp);
            if (temp != null)
            {
                entidades.PlazaXProfesors.Remove(temp);
            }


            Save();

        }
        public bool NombrarPlaza(string codigoPlaza, string codigoProfesor)
        {
            // if (string.IsNullOrEmpty(codigoPlaza.Trim()))
            //   throw new ArgumentException("El codigo de la plaza no es válido. Por favor, intente de nuevo.");


            RepositorioProfesor repoProfesor = new RepositorioProfesor();
            RepositorioPlazas repoPlaza = new RepositorioPlazas();

            bool res = true;
            var IDProfesor = repoProfesor.ObtenerProfesor(Int16.Parse(codigoProfesor));
            var IDPlaza = repoPlaza.ObtenerPlaza(Int16.Parse(codigoPlaza));

            PlazaXProfesor nombrarPlaza = new PlazaXProfesor()
            {
                Plaza = Int16.Parse(IDPlaza.ID.ToString()),
                Profesor = Int16.Parse(IDProfesor.ID.ToString()),
                Horas_Asignadas = IDPlaza.Horas_Totales,
                HorasEnPropiedad = (int)IDPlaza.Horas_Totales
            };
            if (!ExistePlazaXProfesor(nombrarPlaza))
            {
                if (!ExistePlaza(nombrarPlaza))
                {
                    try
                    {
                        AgregarPlazaProfesor(nombrarPlaza);
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
                }
                else
                {
                    nombrarPlaza = repoPlaza.ObtenerPlazaXProfesor(IDPlaza.ID, IDProfesor.ID);
                    nombrarPlaza.Horas_Asignadas = IDPlaza.Horas_Totales;
                    Actualizar(nombrarPlaza);
                }

            }
            else
            { res = false; }
            

            Save();
            return res;
        }
        private void AgregarPlazaProfesor(PlazaXProfesor asignarPlaza)
        {
            entidades.PlazaXProfesors.Add(asignarPlaza);
        }

        public bool ExistePlaza(PlazaXProfesor plazaXProfesor)
        {
            if (plazaXProfesor == null)
                return false;
            return (entidades.PlazaXProfesors.SingleOrDefault(p => p.Plaza == plazaXProfesor.Plaza && p.Profesor==plazaXProfesor.Profesor) != null);
        }
        public bool ExistePlazaXProfesor(PlazaXProfesor plazaXProfesor)
        {
            if (plazaXProfesor == null)
                return false;
            return (entidades.PlazaXProfesors.SingleOrDefault(p => p.Plaza == plazaXProfesor.Plaza && (p.HorasEnPropiedad>0 || p.Horas_Asignadas>0)) != null);
        }
        public void Actualizar(PlazaXProfesor plaza)
        {            

            var temp = entidades.PlazaXProfesors.Find(plaza.ID);

            if (temp != null)
            {
                entidades.Entry(temp).Property(p => p.Horas_Asignadas).CurrentValue += plaza.Horas_Asignadas;
                entidades.Entry(temp).Property(p => p.HorasEnPropiedad).CurrentValue += (int)plaza.Horas_Asignadas;
            }

            Save();

        }

        public void ActualizarXLiberacion(PlazaXProfesor plaza)
        {

            var temp = entidades.PlazaXProfesors.Find(plaza.ID);

            if (temp != null)
            {
                entidades.Entry(temp).Property(p => p.Horas_Asignadas).CurrentValue -= plaza.Horas_Asignadas;
                if(entidades.Entry(temp).Property(p => p.Horas_Asignadas).CurrentValue==0)
                {
                    entidades.PlazaXProfesors.Remove(temp);
                }
            }

            Save();

        }

        public void LiberarHoras(string codigoPlaza, string codigoProfesor, int? horasAsignadas)
        {
            RepositorioProfesor repoProfesor = new RepositorioProfesor();
            RepositorioPlazas repoPlaza = new RepositorioPlazas();

            var IDProfesor = repoProfesor.ObtenerProfesor(Int16.Parse(codigoProfesor));
            var IDPlaza = repoPlaza.ObtenerPlaza(Int16.Parse(codigoPlaza));

            PlazaXProfesor liberarHorasPlaza = new PlazaXProfesor()
            {
                Plaza = Int16.Parse(IDPlaza.ID.ToString()),
                Profesor = Int16.Parse(IDProfesor.ID.ToString()),
                Horas_Asignadas = horasAsignadas
            };
            if (ExistePlaza(liberarHorasPlaza))
            {
                try
                {
                    liberarHorasPlaza = repoPlaza.ObtenerPlazaXProfesor(IDPlaza.ID, IDProfesor.ID);
                    liberarHorasPlaza.Horas_Asignadas = horasAsignadas;
                    ActualizarXLiberacion(liberarHorasPlaza);
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
            }            
            Save();
        }

        public IQueryable obtenerProfeNombradoPorPlaza(int idPlaza)
        {
            return (from plazaProfesor in entidades.PlazaXProfesors
                    where plazaProfesor.Plaza==idPlaza && plazaProfesor.HorasEnPropiedad>0
                    select new { plazaProfesor.Profesore.ID, plazaProfesor.Profesore.Nombre });
        }

        public IQueryable obtenerProfePorPlaza(int idPlaza)
        {
            return (from plazaProfesor in entidades.PlazaXProfesors
                    where plazaProfesor.Plaza == idPlaza && plazaProfesor.Horas_Asignadas >= 0
                    select new { plazaProfesor.Profesore.ID, plazaProfesor.Profesore.Nombre });
        }
        public void Save()
        {
            entidades.SaveChanges();
        }
    }
}