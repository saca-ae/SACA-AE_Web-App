using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SACAAE.Models;

namespace SACAAE.Controllers
{
    using Models; 
    public class CoursesController : Controller
    {
        

        private RepositorioCursos repoCursos = new RepositorioCursos();
        private repositorioPlanesEstudio repoPlanes = new repositorioPlanesEstudio();
        private const string TempDataMessageKey = "MessageError";
        private const string TempDataMessageKeySuccess = "MessageSuccess";


        [Authorize]
        public ActionResult CrearCurso()
        {
            var model = new Curso();
            return View(model);
         
        }

        [Authorize]
        public ActionResult Index()
        {
            //var model = repoCursos.ObtenerCursos();

            String entidad = Request.Cookies["Entidad"].Value;

            if (entidad.Equals("TEC"))
            {
                var model = repoCursos.ObtenerCursosXEntidad(1);
                return View(model);
            }
            else if (entidad.Equals("CIE"))
            {
                var model = repoCursos.ObtenerCursosXEntidad(7);
                return View(model);
            }
            else if (entidad.Equals("TAE"))
            {
                var model = repoCursos.ObtenerCursosXEntidad(5);
                return View(model);
            }
            else if (entidad.Equals("MAE"))
            {
                var model = repoCursos.ObtenerCursosXEntidad(6);
                return View(model);
            }
            else if (entidad.Equals("DDE"))
            {
                var model = repoCursos.ObtenerCursosXEntidad(11);
                return View(model);
            }
            else if (entidad.Equals("Emprendedores"))
            {
                var model = repoCursos.ObtenerCursosXEntidad(12);
                return View(model);
            }
            else if (entidad.Equals("Actualizacion_Cartago"))
            {
                var model = repoCursos.ObtenerCursosXEntidad(9);
                return View(model);
            }
            else
            {
                var model = repoCursos.ObtenerCursosXEntidad(8); //Actualización San Carlos
                return View(model);
            }

        }

        [Authorize]
        [HttpPost]
        public ActionResult CrearCurso(Curso curso, int HorasPracticas, int HorasTeoricas, int Bloque)
        {


            if (curso != null && (HorasPracticas > 0 || HorasTeoricas > 0))
            {
                curso.HorasPracticas = HorasPracticas;
                curso.HorasTeoricas = HorasTeoricas;
                curso.Bloque = Bloque;
                repoCursos.guardarCurso(curso);
                TempData[TempDataMessageKeySuccess] = "Curso Ingresado";
                return RedirectToAction("Index");
            }
            TempData[TempDataMessageKey] = "Datos ingresados son inválidos";
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult EliminarCurso(int id)
        {
            var model = repoCursos.ObtenerCurso(id);
            return View(model);
        }

        [Authorize]
        public ActionResult ModificarCurso(int id)
        {
            var model = repoCursos.ObtenerCurso(id);
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult EliminarCurso(Curso curso)
        {
            TempData[TempDataMessageKey]  = repoCursos.borrarCurso(curso.ID);
                 
          
            return RedirectToAction("Index");
       

        }

        [Authorize]
        [HttpPost]
        public ActionResult ModificarCurso(Curso pCurso,string Nombre,string Codigo)
        {
            
            if(pCurso.Nombre!=Nombre)
            {
                if (repoCursos.existeCurso(pCurso.Nombre))
                {
                    TempData[TempDataMessageKey] = "Es posible que exista un curso con el mismo nombre. Por Favor intente de nuevo.";
                    return RedirectToAction("ModificarCurso");
                }
            }
            if(pCurso.Codigo!=Codigo)
            {
                if (repoCursos.existeCurso(pCurso.Codigo))
                {
                    TempData[TempDataMessageKey] = "Es posible que exista un curso con el mismo codigo. Por Favor intente de nuevo.";
                    return RedirectToAction("ModificarCurso");
                }
            }               
            
            

            if (ModelState.IsValid)
            {
                repoCursos.ModificarCurso(pCurso);
                TempData[TempDataMessageKey] = "El registro ha sido editado correctamente.";
            }
            return RedirectToAction("Index");
        }
 

        public ActionResult planesLista()
        {
            IQueryable listaPlanes = repoPlanes.ObtenerTodosPlanes();
            if (HttpContext.Request.IsAjaxRequest())
            {
                return Json(new SelectList(
                        listaPlanes,
                        "ID", 
                        "Nombre"), JsonRequestBehavior.AllowGet
                        ); 
            }
            return View(listaPlanes); 
        }

        public ActionResult cursosPlanLista(string idPlan)
        {
            IQueryable listaCursos = repoCursos.ObtenerCursosDePlan(Int16.Parse(idPlan));
            if (HttpContext.Request.IsAjaxRequest())
            {
                return Json(new SelectList(listaCursos, "ID", "Nombre"), JsonRequestBehavior.AllowGet); 
            }
            return View(listaCursos); 
        }




      
    }
}
