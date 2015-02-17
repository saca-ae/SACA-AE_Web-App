using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SACAAE.Models;

namespace SACAAE.Controllers
{
    [HandleError]
    public class BloqueController : Controller
    {
        private RepositorioBloqueAcademico vRepoBloqueAcademico = new RepositorioBloqueAcademico();
        private const string TempDataMessageKey = "MessageError";
        private const string TempDataMessageKeySuccess = "MessageSuccess";

        // GET: /Bloque/
        [Authorize]
        public ActionResult Index()
        {
            var model = vRepoBloqueAcademico.ListarBloquesAcademicos();
            return View(model);
        }

        [Authorize]
        public ActionResult crearBloqueAcademico()
        {
            var model = new BloqueAcademico();
            return View(model);
        }

        [Authorize]
        public ActionResult eliminarBloqueAcademico(int id)
        {
            var model = vRepoBloqueAcademico.obtenerBloqueAcademico(id);
            return View(model);
        }

        [Authorize]
        public ActionResult modificarBloqueAcademico(int id)
        {
            var model = vRepoBloqueAcademico.obtenerBloqueAcademico(id);
            ViewBag.Bloque = model;
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult crearBloqueAcademico(BloqueAcademico model)
        {

            if (model.Descripcion == null)
            {
                TempData[TempDataMessageKey] = "Ingrese una Descripcion Válida";
                return RedirectToAction("CrearBloqueAcademico");
            }
            if (vRepoBloqueAcademico.existeBloque(model.Descripcion))
            {
                TempData[TempDataMessageKey] = "El sistema ya cuenta con un Bloque Académico bajo esa descripción. Por Favor intente de nuevo.";
                return RedirectToAction("CrearBloqueAcademico");
            }

            vRepoBloqueAcademico.agregarBloqueAcademico(model);
            TempData[TempDataMessageKeySuccess] = "El bloque académico ha sido creado exitosamente";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize]
        public ActionResult eliminarBloqueAcademico(BloqueAcademico pBloqueAcademico)
        {
            vRepoBloqueAcademico.eliminarBloqueAcademico(pBloqueAcademico);
            TempData[TempDataMessageKey] = "El registro ha sido eliminado correctamente.";
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpPost]
        public ActionResult modificarBloqueAcademico(BloqueAcademico pBloqueAcademico, string oDescripcion)
        {
            if (pBloqueAcademico.Descripcion != oDescripcion)
            {
                if (vRepoBloqueAcademico.existeBloque(pBloqueAcademico.Descripcion))
                {
                    TempData[TempDataMessageKey] = "El sistema ya cuenta con un Bloque Académico bajo esa descripción. Por Favor intente de nuevo." ;
                    return RedirectToAction("ModificarBloqueAcademico");
                }
            }
            

            if (ModelState.IsValid)
            {
                vRepoBloqueAcademico.modificarBloqueAcademico(pBloqueAcademico);
                TempData[TempDataMessageKey] = "El registro ha sido editado correctamente.";
            }
            return RedirectToAction("Index");
        }
    }
}