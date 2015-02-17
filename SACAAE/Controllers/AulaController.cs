using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SACAAE.Models;

namespace SACAAE.Controllers
{
     [HandleError]
    public class AulaController : Controller
    {
        private RepositorioAulas vRepoAulas = new RepositorioAulas();
        private repositorioSedes vRepoSedes = new repositorioSedes();
        private const string TempDataMessageKey = "MessageError";
        private const string TempDataMessageKeySuccess = "MessageSuccess";

        // GET: /Aula/
        [Authorize]
        public ActionResult Index()
        {
            ViewBag.Sedes = vRepoSedes.ObtenerTodosSedes();
            var model = vRepoAulas.ListarAulas();
            return View(model);
        } 

        [Authorize]
        public ActionResult CrearAula()
        {
            var model = new Aula();
            ViewBag.Sedes = vRepoSedes.ObtenerTodosSedes();
            return View(model);
        }

        public ActionResult ObtenerAula(int sede)
        {
            IQueryable listaAulas = vRepoAulas.ListarAulasXSede(sede);
            if (HttpContext.Request.IsAjaxRequest())
            {
                return Json(listaAulas, JsonRequestBehavior.AllowGet);
            }
            return View(listaAulas);
        }

        [Authorize]
        public ActionResult EliminarAula(int id)
        {
            var model = vRepoAulas.ObtenerAula(id);
            return View(model);
        }

        [Authorize]
        public ActionResult ModificarAula(int id)
        {
            var model = vRepoAulas.ObtenerAula(id);
            ViewBag.Sedes = vRepoSedes.ObtenerTodosSedes();
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult CrearAula(Aula model, string selectSede, int capacidad)
        {
            int vSedeID = Int16.Parse(selectSede);

            if (model.Codigo == null)
            {
                TempData[TempDataMessageKey] = "Ingrese un Código Válido";
                return RedirectToAction("CrearAula");
            }
            if (vRepoAulas.existeAula(vSedeID, model.Codigo))
            {
                TempData[TempDataMessageKey] = "Esta sede ya cuenta con un aula con el código provisto. Por Favor intente de nuevo.";
                return RedirectToAction("CrearAula");
            }

            model.SedeID = vSedeID;
            model.Espacio = capacidad;
            model.Activa = true;
            vRepoAulas.agregarAula(model);
            TempData[TempDataMessageKeySuccess] = "El aula ha sido creada exitosamente";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize]
        public ActionResult EliminarAula(Aula pAula)
        {
            vRepoAulas.eliminarAula(pAula);
            TempData[TempDataMessageKey] = "El registro ha sido eliminado correctamente.";
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpPost]
        public ActionResult ModificarAula(Aula pAula, string selectSede,string Codigo)
        {
            int vSedeID = Int16.Parse(selectSede);
            pAula.SedeID = vSedeID;
            if (Codigo != pAula.Codigo)
            {
                if (vRepoAulas.existeAula(pAula.SedeID, pAula.Codigo))
                {
                    TempData[TempDataMessageKey] = "Esta sede ya cuenta con un aula con el código provisto. Por Favor intente de nuevo." + Codigo;
                    return RedirectToAction("ModificarAula");
                }
            }
            

            if (ModelState.IsValid)
            {
                vRepoAulas.ModificarAula(pAula);
                TempData[TempDataMessageKey] = "El registro ha sido editado correctamente.";
            }
            return RedirectToAction("Index");
        }
    }
}