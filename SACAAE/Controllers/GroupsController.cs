using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SACAAE.Models;

namespace SACAAE.Controllers
{
    public class GroupsController : Controller
    {
        private repositorioSedes repoSedes = new repositorioSedes();
        private repositorioPlanesEstudio repoPlanes = new repositorioPlanesEstudio();
        private repositorioModalidades repoModalidades = new repositorioModalidades();
        private RepositorioCursos repoCursos = new RepositorioCursos();
        private repositorioGrupos repoGrupos = new repositorioGrupos();
        private repositorioPlanesXSedes repoPlanesXSedes = new repositorioPlanesXSedes();
        private RepositorioPeriodos repoPeriodos = new RepositorioPeriodos();
        private const string TempDataMessageKey = "MessageError";
        private const string TempDataMessageKeySuccess = "MessageSuccess";


        //public ActionResult Index()
        //{

        //    return View();
        //}
        [Authorize]
        public ActionResult CrearGrupo()
        {
            var model = new Grupo();
            return View(model);
        }

        //[Authorize]
        //[HttpPost]
        //public ActionResult CrearGrupo(Grupo grupo, string selectSede, string selectModalidad,
        //                       string selectPlan, string selectBloque)
        //{


           
        //    if (selectPlan == null)
        //    {
        //        TempData[TempDataMessageKey] = "No ha elegido el plan. Por favor vuelva a ingresar los datos.";
        //        return RedirectToAction("CrearGrupo");
        //    }

        //    int sede = Int16.Parse(selectSede);
        //    int modalidad = Int16.Parse(selectModalidad);
        //    int plan = Int16.Parse(selectPlan);
        //    int bloque = Int16.Parse(selectBloque);
        //    var planXsede = repoPlanesXSedes.tomarIDPlanXSede(sede, plan);

        //    String Periodo = Request.Cookies["Periodo"].Value;
        //    int IdPeriodo = repoPeriodos.IdPeriodo(Periodo);  

        //    if (!repoCursos.existeCursoEnBloque(plan, bloque))
        //    {
        //        TempData[TempDataMessageKey] = "No existe ese bloque en el plan seleccionado.";
        //        return RedirectToAction("CrearGrupo");
        //    }
        //    if (repoGrupos.existeGrupo(grupo.Nombre, IdPeriodo, planXsede.ID))
        //    {
        //        TempData[TempDataMessageKey] = "Ya ha creado un grupo con ese nombre.";
        //        return RedirectToAction("CrearGrupo");
        //    }
        //    //debo colocar una fk, no al plan de estudio exactamente, si no al registro de planesXsedes que representa mi plan de estuido y mi sede

           

        //    grupo.Bloque = bloque;
        //    grupo.PlanDeEstudio = planXsede.ID;
        //    repoGrupos.guardaGrupo(grupo, IdPeriodo);

        //    TempData[TempDataMessageKeySuccess] = "Grupo Creado Exitosamente";
        //    return RedirectToAction("CrearGrupo");
        //}
        //[Authorize]
        //public ActionResult eliminarGrupo()
        //{
        //    return View(); 
        //}
        //[Authorize]
        //[HttpPost]
        //public ActionResult eliminarGrupo(string selectGrupo)
        //{
        //    if (selectGrupo == null)
        //    {
        //        TempData[TempDataMessageKey] = "Seleccione un grupo.";
        //        return RedirectToAction("eliminarGrupo");
        //    }
        //    int grupo = Int16.Parse(selectGrupo);
        //    repoGrupos.eliminarGrupo(grupo);
        //    TempData[TempDataMessageKey] = "Grupo Eliminado";
        //    return RedirectToAction("eliminarGrupo"); 
        //}

        //public ActionResult sedesLista()
        //{
        //    IQueryable listaSedes = repoSedes.tomarTodasLasSedes();
        //    if (HttpContext.Request.IsAjaxRequest())
        //    {
        //        return Json(new SelectList(
        //                listaSedes,
        //                "ID",
        //                "Nombre"), JsonRequestBehavior.AllowGet
        //                );
        //    }
        //    return View(listaSedes);
        //}
        //public ActionResult planesSedeModalidadLista(int IDModalidad, int IDSede)
        //{
        //    IQueryable listaPlanes = repoPlanes.ObtenerPlanesConModalidad(IDModalidad, IDSede);
        //    if (HttpContext.Request.IsAjaxRequest())
        //    {
        //        return Json(new SelectList(
        //                listaPlanes,
        //                "ID",
        //                "Nombre"), JsonRequestBehavior.AllowGet
        //                );
        //    }
        //    return View(listaPlanes);
        //}
        //public ActionResult gruposLista(int plan, int bloque, int sede)
        //{
        //    PlanesDeEstudioXSede planXsede = repoPlanesXSedes.tomarIDPlanXSede(sede, plan);
        //    String Periodo = Request.Cookies["Periodo"].Value;
        //    int periodo = repoPeriodos.IdPeriodo(Periodo);  
        //    IQueryable listaGrupos = repoGrupos.tomarGruposconCondiciones(planXsede.ID, bloque, periodo);
        //    if (HttpContext.Request.IsAjaxRequest())
        //    {
        //        return Json(new SelectList(
        //                listaGrupos,
        //                "ID",
        //                "Nombre"), JsonRequestBehavior.AllowGet
        //                );
        //    }
        //    return View(listaGrupos);
        //}

        //public ActionResult modalidadesLista()
        //{
        //    IQueryable listaModalidades = repoModalidades.tomarModalidades();
        //    if (HttpContext.Request.IsAjaxRequest())
        //    {
        //        return Json(new SelectList(
        //                listaModalidades,
        //                "ID",
        //                "Nombre"), JsonRequestBehavior.AllowGet
        //                );
        //    }
        //    return View(listaModalidades);
        //}
    }
}