using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SACAAE.Models;

namespace SACAAE.Controllers
{
    public class ProyectoController : Controller
    {
        //
        // GET: /Proyecto/
        private const string TempDataMessageKey = "Message";
        private RepositorioProyecto repositorio = new RepositorioProyecto();

        [Authorize]
        public ActionResult Index()
        {
            //var model = repositorio.ObtenerTodosProyectos();
            String entidad = Request.Cookies["Entidad"].Value;

            if (entidad.Equals("TEC"))
            {
                var model = repositorio.ObtenerProyectoXEntidad(1);
                return View(model);
            }
            else if (entidad.Equals("CIE"))
            {
                var model = repositorio.ObtenerProyectoXEntidad(7);
                return View(model);
            }
            else if (entidad.Equals("TAE"))
            {
                var model = repositorio.ObtenerProyectoXEntidad(5);
                return View(model);
            }
            else if (entidad.Equals("MAE"))
            {
                var model = repositorio.ObtenerProyectoXEntidad(6);
                return View(model);
            }
            else if (entidad.Equals("DDE"))
            {
                var model = repositorio.ObtenerProyectoXEntidad(11);
                return View(model);
            }
            else if (entidad.Equals("Emprendedores"))
            {
                var model = repositorio.ObtenerProyectoXEntidad(12);
                return View(model);
            }
            else if (entidad.Equals("Actualizacion_Cartago"))
            {
                var model = repositorio.ObtenerProyectoXEntidad(9);
                return View(model);
            }
            else
            {
                var model = repositorio.ObtenerProyectoXEntidad(8); //Actualización San Carlos
                return View(model);
            }
        }

        [Authorize]
        public ActionResult Crear()
        {
            var model = new Proyecto();
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Crear(Proyecto nuevoProyecto)
        {
            String entidad = Request.Cookies["Entidad"].Value;
            int entidadID;

            if (entidad.Equals("TEC")){ entidadID = 1; }
            else if (entidad.Equals("CIE")){ entidadID = 7;}
            else if (entidad.Equals("TAE")){ entidadID = 5;}
            else if (entidad.Equals("MAE")){ entidadID = 6;}
            else if (entidad.Equals("DDE")){ entidadID = 11;}
            else if (entidad.Equals("Emprendedores")){  entidadID = 12; }
            else if (entidad.Equals("Actualizacion_Cartago")) { entidadID = 9; }
            else { entidadID = 8; }

            repositorio.CrearProyecto(nuevoProyecto.Nombre, nuevoProyecto.Inicio, nuevoProyecto.Fin, nuevoProyecto.Link, entidadID);
            TempData[TempDataMessageKey] = "Proyecto creado correctamente.";
            return RedirectToAction("Index");
            
        }

        [Authorize]
        public ActionResult Eliminar(int id)
        {
            var model = repositorio.ObtenerProyecto(id);
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Eliminar(Proyecto proyecto)
        {
            repositorio.BorrarProyecto(proyecto);
            TempData[TempDataMessageKey] = "Proyecto eliminado correctamente.";
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Editar(int id)
        {
            var model = repositorio.ObtenerProyecto(id);
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Editar(Proyecto proyecto)
        {
            if (ModelState.IsValid)
            {
                repositorio.Actualizar(proyecto);
                TempData[TempDataMessageKey] = "Proyecto editado correctamente.";
            }
            
            
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Detalles(int id)
        {
            var model = repositorio.ObtenerProyecto(id);
            return View(model);
        }

    }
}
