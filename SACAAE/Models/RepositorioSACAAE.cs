using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;


namespace SACAAE.Models
{
    public class RepositorioSACAAE
    {
        private SACAAEEntities entidades = new SACAAEEntities();

        private const string FaltaUsuario = "Usuario no existe";
        private const string MuchoUsuario = "Usuario ya existe";


        public int NumeroUsuarios
        {
            get
            {
                return this.entidades.Usuarios.Count();
            }
        }

        public RepositorioSACAAE()
        {
            this.entidades = new SACAAEEntities();
        }


        public IQueryable<Usuario> ObtenerTodosUsuarios()
        {
            return from user in entidades.Usuarios
                   orderby user.NombreUsuario
                   select user;
        }

        public Usuario ObtenerUsuario(int id)
        {
            return entidades.Usuarios.SingleOrDefault(user => user.ID == id);
        }

        public Usuario ObtenerUsuario(string NombreUsuario)
        {
            return entidades.Usuarios.SingleOrDefault(user => user.NombreUsuario == NombreUsuario);
        }

        private void AgregarUsuario(Usuario usuario)
        {
            if (ExisteUsuario(usuario))
                throw new ArgumentException(MuchoUsuario);

            entidades.Usuarios.Add(usuario);
        }

        public void CrearUsuario(Usuario model)
        {
            if (ExisteUsuario(model))
                return;
            else
            {
                string tempContrasenia = model.Contrasenia;
                model.Contrasenia = FormsAuthentication.HashPasswordForStoringInConfigFile(tempContrasenia, "md5");
                entidades.Usuarios.Add(model);
                Save();
            }
        }

        public void EliminarUsuario(Usuario pUsuario)
        {
            var vUsuario = entidades.Usuarios.SingleOrDefault(user => user.ID == pUsuario.ID);
            if (vUsuario != null)
            {
                entidades.Usuarios.Remove(vUsuario);
                Save();
            }
            else
                return;
        }

        public void modificarUsuario(Usuario pUsuario)
        {
            var vUsuario = entidades.Usuarios.SingleOrDefault(usuario => usuario.ID == pUsuario.ID);
            if (vUsuario != null)
            {
                entidades.Entry(vUsuario).Property(usuario => usuario.Nombre).CurrentValue = pUsuario.Nombre;
                entidades.Entry(vUsuario).Property(usuario => usuario.NombreUsuario).CurrentValue = pUsuario.NombreUsuario;
                entidades.Entry(vUsuario).Property(usuario => usuario.Contrasenia).CurrentValue = FormsAuthentication.HashPasswordForStoringInConfigFile( pUsuario.Contrasenia, "md5");
                entidades.Entry(vUsuario).Property(usuario => usuario.Correo).CurrentValue = pUsuario.Correo;
                Save();
            }
            else
                return;
        }
       

        public void Save()
        {
            entidades.SaveChanges();
        }

        public bool ExisteUsuario(Usuario usuario)
        {
            if (usuario == null)
                return false;

            return (entidades.Usuarios.SingleOrDefault(u => u.ID == usuario.ID ||
                u.NombreUsuario == usuario.NombreUsuario) != null);
        }

    }
}