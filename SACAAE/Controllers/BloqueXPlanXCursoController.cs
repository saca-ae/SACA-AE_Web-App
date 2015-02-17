using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SACAAE.Models;

namespace SACAAE.Controllers
{
    [HandleError]
    public class BloqueXPlanXCursoController : Controller
    {
        private RepositorioPlanesDeEstudio vRepoPlanes = new RepositorioPlanesDeEstudio();
        private RepositorioBloqueAcademico vRepoBloques = new RepositorioBloqueAcademico();
        private RepositorioBloqueXPlan vRepoBloqueXPlan = new RepositorioBloqueXPlan();
        private RepositorioCursos vRepoCursos = new RepositorioCursos();
        private RepositorioBloqueXPlanXCurso vRepoBloquesXPlanXCurso = new RepositorioBloqueXPlanXCurso();
        private const string TempDataMessageKey = "MessageError";
        private const string TempDataMessageKeySuccess = "MessageSuccess";
        // GET: BloqueXPlanXCurso
        [Authorize]
        public ActionResult CrearBloqueXPlanXCurso(int plan)
        {
            var vBloqueXPlan = new BloqueAcademicoXPlanDeEstudio();
            ViewBag.Planes = vRepoPlanes.ObtenerUnPlanDeEstudio(plan);
            ViewBag.Bloques = vRepoBloques.obtenerBloques(plan);
            ViewBag.Cursos = vRepoCursos.ObtenerCursos();
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult CrearBloqueXPlanXCurso(BloqueXPlanXCurso pBloqueXPlanXCurso, string selectPlanDeEstudio, string selectBloqueAcademico,string selectCurso)
        {
            int PlanID = Int16.Parse(selectPlanDeEstudio); 
            if (pBloqueXPlanXCurso != null && selectPlanDeEstudio != null && selectBloqueAcademico != null && selectCurso != null)
            {
                int BloqueID = Int16.Parse(selectBloqueAcademico);
                int CursoID = Int16.Parse(selectCurso);

                pBloqueXPlanXCurso.CursoID = CursoID;
                pBloqueXPlanXCurso.BloqueXPlanID = vRepoBloqueXPlan.idBloqueXPlan(PlanID,BloqueID);
                if (vRepoBloquesXPlanXCurso.existeRelacionBloqueXPlanXCurso(pBloqueXPlanXCurso.BloqueXPlanID, pBloqueXPlanXCurso.CursoID))
                {
                    TempData[TempDataMessageKey] = "El Bloque académico de este plan de estudio ya cuenta con el curso seleccionado. Por Favor intente de nuevo.";
                    return RedirectToAction("CrearBloqueXPlanXCurso", new { plan = PlanID });
                }
                if (vRepoBloquesXPlanXCurso.existeRelacionCursoEnPlan(PlanID, pBloqueXPlanXCurso.CursoID))
                {
                    TempData[TempDataMessageKey] = "El Plan de estudio  ya cuenta con el curso seleccionado. Por Favor intente de nuevo.";
                    return RedirectToAction("CrearBloqueXPlanXCurso", new { plan = PlanID });
                }
                vRepoBloquesXPlanXCurso.crearRelacionBloqueXPlanXCurso(pBloqueXPlanXCurso);
                TempData[TempDataMessageKeySuccess] = "El curso ha sido asignado al bloque académico del plan de estudio exitosamente";
                return RedirectToAction("CrearBloqueXPlanXCurso", new { plan = PlanID });

            }
            TempData[TempDataMessageKey] = "Datos ingresados son inválidos";
            return RedirectToAction("CrearBloqueXPlanXCurso", new { plan = PlanID });
        }

        public ActionResult ObtenerCursos(int plan, int bloque)
        {
            IQueryable vListaCursos = vRepoCursos.ObtenerCursos(plan,bloque);
            if (HttpContext.Request.IsAjaxRequest())
            {
                return Json(new SelectList(
                        vListaCursos,
                        "ID",
                        "Nombre"), JsonRequestBehavior.AllowGet
                        );
            }
            return View(vListaCursos);
        }

        
    }
}