using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SACAAE.Models;

namespace SACAAE.Controllers
{
    public class OfertaAcademicaController : Controller
    {
        private RepositorioPlanesDeEstudio vRepoPlanes = new RepositorioPlanesDeEstudio();
        private repositorioModalidades vRepoModalidades = new repositorioModalidades();
        private repositorioSedes vRepoSedes = new repositorioSedes();
        private RepositorioPeriodos vRepoPeriodos = new RepositorioPeriodos();
        private repositorioPlanesXSedes vRepoPlanXSedes = new repositorioPlanesXSedes();
        private RepositorioBloqueXPlan vRepoBloqueXPlan = new RepositorioBloqueXPlan();
        private RepositorioBloqueXPlanXCurso vRepoBloqueXPlanXCurso = new RepositorioBloqueXPlanXCurso();
        private repositorioGrupos vRepoGrupos = new repositorioGrupos();
        private const string TempDataMessageKey = "MessageError";
        private const string TempDataMessageKeySuccess = "MessageSuccess";
        // GET: OfertaAcademica
        [Authorize]
        public ActionResult CrearOfertaAcademica()
        {
            ViewBag.Modalidades = vRepoModalidades.ObtenerTodosModalidades();
            ViewBag.Sedes = vRepoSedes.ObtenerTodosSedes();
            ViewBag.Periodos = vRepoPeriodos.obtenerTodosPeriodos();
            return View();
        }

        [Authorize]
        public ActionResult Index()
        {
            ViewBag.Modalidades = vRepoModalidades.ObtenerTodosModalidades();
            ViewBag.Sedes = vRepoSedes.ObtenerTodosSedes();
            ViewBag.Periodos = vRepoPeriodos.obtenerTodosPeriodos();
            return View();
        }

        [Authorize]
        public ActionResult EliminarOferta(int id)
        {
            var model = vRepoGrupos.obtenerUnGrupo(id);
            return View(model);
        }


        [Authorize]
        [HttpPost]
        public ActionResult CrearOfertaAcademica(string sltPeriodo, string sltSede, string sltPlan,
            string sltBloque, string sltCurso, int cantidadGrupos)
        {
            if (String.IsNullOrEmpty(sltPeriodo))
            {
                TempData[TempDataMessageKey] = "Es necesario que seleccione un Periodo";
                return RedirectToAction("CrearOfertaAcademica");
            }
            if (String.IsNullOrEmpty(sltSede))
            {
                TempData[TempDataMessageKey] = "Es necesario que seleccione una Sede";
                return RedirectToAction("CrearOfertaAcademica");
            }
            if (String.IsNullOrEmpty(sltPlan))
            {
                TempData[TempDataMessageKey] = "Es necesario que seleccione un Plan";
                return RedirectToAction("CrearOfertaAcademica");
            }
            if (String.IsNullOrEmpty(sltBloque))
            {
                TempData[TempDataMessageKey] = "Es necesario que seleccione un Bloque";
                return RedirectToAction("CrearOfertaAcademica");
            }
            if (String.IsNullOrEmpty(sltCurso))
            {
                TempData[TempDataMessageKey] = "Es necesario que seleccione un Curso";
                return RedirectToAction("CrearOfertaAcademica");
            }

            int vPeriodoID = Int16.Parse(sltPeriodo);
            int vSedeID = Int16.Parse(sltSede);
            int vPlanID = Int16.Parse(sltPlan);
            int vBloqueID = Int16.Parse(sltBloque);
            int vCursoID = Int16.Parse(sltCurso);

            int vPlanXSedeID = vRepoPlanXSedes.tomarIDPlanXSede(vSedeID, vPlanID).ID;
            int vBloqueXPlanID = vRepoBloqueXPlan.obtenerIdBloqueXPlan(vPlanID, vBloqueID);
            int vBloqueXPlanXCursoID = vRepoBloqueXPlanXCurso.obtenerBloqueXPlanXCursoID(vBloqueXPlanID, vCursoID);
            for (int vContadorGrupos = 0; vContadorGrupos < cantidadGrupos; vContadorGrupos++)
            {
                int vNumeroGrupo = vRepoGrupos.ObtenerUltimoNumeroGrupo(vPlanXSedeID, vPeriodoID, vBloqueXPlanXCursoID) + 1;
                Grupo vNewGrupo = new Grupo();
                vNewGrupo.Numero = vNumeroGrupo;
                vNewGrupo.PlanDeEstudio = vPlanXSedeID;
                vNewGrupo.Periodo = vPeriodoID;
                vNewGrupo.BloqueXPlanXCursoID = vBloqueXPlanXCursoID;

                vRepoGrupos.agregarGrupo(vNewGrupo);
            }
            TempData[TempDataMessageKeySuccess] = "Los Grupos fueron creados correctamente";
            return RedirectToAction("Index");
        }

        public ActionResult ObtenerOfertasAcademicas(int sede,int plan, int periodo)
        {
            IQueryable listaOfertas = vRepoGrupos.ListarGruposXSedeXPeriodo(plan, periodo);
            if (HttpContext.Request.IsAjaxRequest())
            {
                return Json(listaOfertas, JsonRequestBehavior.AllowGet);
            }
            return View(listaOfertas);
        }

        [Authorize]
        [HttpPost]
        public ActionResult EliminarOferta(Grupo grupo)
        {
            vRepoGrupos.eliminarGrupo(grupo);
            TempData[TempDataMessageKey] = "El registro ha sido borrado correctamente.";
            return RedirectToAction("Index");
        }
    }
}