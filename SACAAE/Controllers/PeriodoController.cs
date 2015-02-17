using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SACAAE.Models;

namespace SACAAE.Controllers
{
    public class PeriodoController : Controller
    {
        private RepositorioPeriodos repoPeriodo = new RepositorioPeriodos(); 
        private const string TempDataMessageKey = "MessageError";
        private const string TempDataMessageKeySuccess = "MessageSuccess";
        //
        // GET: /Periodo/
        [Authorize]
        public ActionResult AgregarPeriodo()
        {
            var model = new Periodo(); 
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult AgregarPeriodo(Periodo model)
        {
            if (model.Nombre == null)
            {
                TempData[TempDataMessageKey] = "Ingrese un Nombre";
                return RedirectToAction("AgregarPeriodo");
            }
            if (repoPeriodo.existe(model.Nombre) != null)
            {
                TempData[TempDataMessageKey] = "Ya existe un periodo con ese nombre";
                return RedirectToAction("AgregarPeriodo");
            }
            repoPeriodo.agregarPeriodo(model);
            TempData[TempDataMessageKeySuccess] = "Periodo Creado Exitosamente";
            return RedirectToAction("AgregarPeriodo");
        }

        
	}
}