using SACAAE.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SACAAE.Controllers
{
    public class ComisionController : Controller
    {
        //
        // GET: /Comision/
        private const string TempDataMessageKey = "Message";
        private RepositorioComision repositorio = new RepositorioComision();

        [Authorize]
        public ActionResult Index()
        {
            String entidad = Request.Cookies["Entidad"].Value;

            if (entidad.Equals("TEC"))
            {
                var model = repositorio.ObtenerComisionesXEntidad(1);
                return View(model);
            }
            else if (entidad.Equals("CIE"))
            {
                var model = repositorio.ObtenerComisionesXEntidad(7);
                return View(model);
            }
            else if (entidad.Equals("TAE"))
            {
                var model = repositorio.ObtenerComisionesXEntidad(5);
                return View(model);
            }
            else if (entidad.Equals("MAE"))
            {
                var model = repositorio.ObtenerComisionesXEntidad(6);
                return View(model);
            }
            else if (entidad.Equals("DDE"))
            {
                var model = repositorio.ObtenerComisionesXEntidad(11);
                return View(model);
            }
            else if (entidad.Equals("Emprendedores"))
            {
                var model = repositorio.ObtenerComisionesXEntidad(12);
                return View(model);
            }
            else if (entidad.Equals("Actualizacion_Cartago"))
            {
                var model = repositorio.ObtenerComisionesXEntidad(9);
                return View(model);
            }
            else if (entidad.Equals("CIADEG"))
            {
                var model = repositorio.ObtenerComisionesXEntidad(13);
                return View(model);
            }
            else if (entidad.Equals("MDE"))
            {
                var model = repositorio.ObtenerComisionesXEntidad(14);
                return View(model);
            }
            else if (entidad.Equals("MGP"))
            {
                var model = repositorio.ObtenerComisionesXEntidad(9);
                return View(model);
            }
            else
            {
                var model = repositorio.ObtenerComisionesXEntidad(8); //Actualización San Carlos
                return View(model);
            }


        }

        [Authorize]
        public ActionResult Crear()
        {
            var model = new Comisione();
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Crear(Comisione nuevaComision)
        {
            String entidad = Request.Cookies["Entidad"].Value;
            int entidadID;

            if (entidad.Equals("TEC")) { entidadID = 1; }
            else if (entidad.Equals("CIE")) { entidadID = 7; }
            else if (entidad.Equals("TAE")) { entidadID = 5; }
            else if (entidad.Equals("MAE")) { entidadID = 6; }
            else if (entidad.Equals("DDE")) { entidadID = 11; }
            else if (entidad.Equals("Emprendedores")) { entidadID = 12; }
            else if (entidad.Equals("Actualizacion_Cartago")) { entidadID = 9; }
            else if (entidad.Equals("CIADEG")) { entidadID = 13; }
            else if (entidad.Equals("MDE")) { entidadID = 14; }
            else if (entidad.Equals("MGE")) { entidadID = 15; }
            else { entidadID = 8; }


            repositorio.CrearComision(nuevaComision.Nombre, nuevaComision.Inicio, nuevaComision.Fin, entidadID);
            TempData[TempDataMessageKey] = "Comisión creada correctamente.";
            return RedirectToAction("Index");

        }

        [Authorize]
        public ActionResult Eliminar(int id)
        {
            var model = repositorio.ObtenerComision(id);
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Eliminar(Comisione comision)
        {
            repositorio.BorrarComision(comision);
            TempData[TempDataMessageKey] = "Comisión eliminada correctamente.";
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Editar(int id)
        {
            var model = repositorio.ObtenerComision(id);
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Editar(Comisione comision)
        {
            if (ModelState.IsValid)
            {
                repositorio.Actualizar(comision);
                TempData[TempDataMessageKey] = "Comisión editada correctamente.";
            }


            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Detalles(int id)
        {
            var model = repositorio.ObtenerComision(id);
            return View(model);
        }

    }
}
