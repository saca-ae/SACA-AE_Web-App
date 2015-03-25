using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using SACAAE.Models;
using SACAAE.Helpers;

namespace SACAAE.Controllers
{
    /// <summary>
    /// Controlador encargado del inicio de sesión y el manejo de los usuarios.
    /// </summary>
    [Authorize]
    public class AccountController : Controller
    {
        public ProveedorMembersia MembershipService { get; set; }
        private RepositorioPeriodos RepoPeriodos = new RepositorioPeriodos();
        private RepositorioSACAAE vRepoCuentas = new RepositorioSACAAE();
        private const string TempDataMessageKey = "MessageError";
        private const string TempDataMessageKeySuccess = "MessageSuccess";

        protected override void Initialize(RequestContext requestContext)
        {
            if (MembershipService == null)
                MembershipService = new ProveedorMembersia();

            base.Initialize(requestContext);
        }

        //
        // GET: /Account/LogOn
        [AllowAnonymous]
        public ActionResult Login()
        {
            List<String> Periodos = new List<String>();
            IQueryable<Periodo> ListaPeriodos = RepoPeriodos.ListaPeriodos();
            foreach (var item in ListaPeriodos)
            {
                Periodos.Add(item.Nombre);
            }
            ViewBag.Periodos = Periodos;
            return View();
        }

        //
        // POST: /Account/LogOn
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            List<String> Periodos = new List<String>();
            IQueryable<Periodo> ListaPeriodos = RepoPeriodos.ListaPeriodos();
            foreach (var item in ListaPeriodos)
            {
                Periodos.Add(item.Nombre);
            }
            ViewBag.Periodos = Periodos;


            if (ModelState.IsValid)
            {
                if (MembershipService.ValidateUser(model.NombreUsuario, model.Contrasenia))
                {
                    FormsAuthentication.SetAuthCookie(model.NombreUsuario, model.Recordarme);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return RedirectToAction("Login", "Account");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Profesor");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Nombre de usuario y/o contraseña incorrectos.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/LogOff
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Login", "Account");
        }

        //
        // GET: /Account/Register
        public ActionResult Register()
        {
            return View();
        }


        //
        // GET: /Account/ChangePassword
        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword
        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                if (MembershipService.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword))
                    return RedirectToAction("ChangePasswordSuccess");
                else
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        //
        // GET: /Account/ChangePasswordSuccess
        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        /*Alexis Boza 2 Semestre 2014*/
        [Authorize]
        public ActionResult Index()
        {
            var model = vRepoCuentas.ObtenerTodosUsuarios();
            return View(model);
        }

        /*Alexis Boza 2 Semestre 2014*/
        [Authorize]
        public ActionResult CrearUsuario()
        {
            var model = new Usuario();
            return View(model);
        }

        [Authorize]
        public ActionResult EliminarUsuario(int id)
        {
            var model = vRepoCuentas.ObtenerUsuario(id);
            return View(model);
        }

        [Authorize]
        public ActionResult ModificarUsuario(int id)
        {
            var model = vRepoCuentas.ObtenerUsuario(id);
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult CrearUsuario(Usuario pUsuario, string validarContrasenia)
        {
            if (ModelState.IsValid)
            {
                if (pUsuario.NombreUsuario == null)
                {
                    TempData[TempDataMessageKey] = "Nombre de Usuario no Válido";
                    return View(pUsuario);
                }
                if (vRepoCuentas.ExisteUsuario(pUsuario))
                {
                    TempData[TempDataMessageKey] = "Ya existe una cuenta con ese nombre de usuario. Por Favor intente de nuevo.";
                    return View(pUsuario);
                }
                if (!pUsuario.Contrasenia.Equals(validarContrasenia))
                {
                    TempData[TempDataMessageKey] = "La contraseñas no concuerdan.Por Favor intente de nuevo";
                    return View(pUsuario);
                }
                vRepoCuentas.CrearUsuario(pUsuario);
                TempData[TempDataMessageKeySuccess] = "El usuario ha sido creado exitosamente";
                return RedirectToAction("Index");
            }
            return View(pUsuario);
        }

        [Authorize]
        [HttpPost]
        public ActionResult EliminarUsuario(Usuario pUsuario)
        {
            vRepoCuentas.EliminarUsuario(pUsuario);
            TempData[TempDataMessageKey] = "El registro ha sido eliminado correctamente.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ModificarUsuario(Usuario pUsuario, string Contrasenia, string validarContrasenia)
        {
            if (ModelState.IsValid)
            {
                if (String.IsNullOrEmpty(Contrasenia))
                {
                    TempData[TempDataMessageKey] = "Para modificar el usuario es necesario proveer una nueva contraseña. Por Favor intente de nuevo.";
                    return View(pUsuario);
                }
                if (!Contrasenia.Equals(validarContrasenia))
                {
                    TempData[TempDataMessageKey] = "La contraseñas no concuerdan.Por Favor intente de nuevo";
                    return View(pUsuario);
                }

                pUsuario.Contrasenia = Contrasenia;
                vRepoCuentas.modificarUsuario(pUsuario);
                TempData[TempDataMessageKey] = "El registro ha sido editado correctamente.";
            }
            return RedirectToAction("Index");
        }
    }
}
