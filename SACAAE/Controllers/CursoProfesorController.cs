﻿using Newtonsoft.Json;
using SACAAE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace SACAAE.Controllers
{
    public class CursoProfesorController : Controller
    {
        //
        // GET: /CursoProfesor/
        private RepositorioProfesor repositorioProfesor = new RepositorioProfesor();
        private RepositorioCursoProfesor repositorioCursoProfesor = new RepositorioCursoProfesor();
        private repositorioGrupos vRepositorioGrupos =new repositorioGrupos();
        private const string TempDataMessageKey = "Message";

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
        
        [Authorize]
        public ActionResult Asignar()
        {
            if (Request.UrlReferrer != null)
            {
                ViewBag.returnUrl = Request.UrlReferrer.ToString();
            }
            else
            {
                ViewBag.returnUrl = null;
            }

            /* Se obtiene la lista de profesores */
            List<Profesore> ListaProfesores = repositorioProfesor.ObtenerTodosProfesores().ToList<Profesore>();
            /* Se obtiene la lista de sedes */
            List<Sede> ListaSedes = repositorioCursoProfesor.obtenerTodasSedes().ToList<Sede>();
            /* Se obtiene la lista de modalidades */
            List<Modalidade> ListaModalidades = repositorioCursoProfesor.obtenerTodasModalidades().ToList<Modalidade>();

            if (ListaProfesores.Count > 0)
                ViewBag.Profesores = ListaProfesores;
            else
                ViewBag.Profesores = null;

            if (ListaSedes.Count > 0)
                ViewBag.Sedes = ListaSedes;
            else
                ViewBag.Sedes = null;

            if (ListaModalidades.Count > 0)
                ViewBag.Modalidades = ListaModalidades;
            else
                ViewBag.Modalidades = null;

            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Asignar(int sltProfesor, int sltGrupo, int txtHoras, int txtHorasEstimadas)
        {
            var creado = 0 ;
            var idProfesorXCurso = 0; 
            var idDetalleGrupo = vRepositorioGrupos.obtenerUnDetalleGrupo(sltGrupo);
            idProfesorXCurso = repositorioCursoProfesor.asignarProfesor(sltProfesor, txtHoras+txtHorasEstimadas);
            if (idProfesorXCurso != 0)
            {
                creado = repositorioCursoProfesor.actualizarDetalleGrupo(idProfesorXCurso, idDetalleGrupo.Id);

                if (creado !=0)
                {
                    TempData[TempDataMessageKey] = "Profesor asignado correctamente.";
                }
                else
                {
                    TempData[TempDataMessageKey] = "Ocurrió un error al asignar el profesor.";
                }
            }
            else
            {
                TempData[TempDataMessageKey] = "No se pudo obtener el id de profesor x curso.";
            }
            
            return RedirectToAction("Asignar");
        }

        public ActionResult ObtenerPlanesEstudio(int sede, int modalidad)
        {
            IQueryable listaPlanes = repositorioCursoProfesor.obtenerPlanesEstudio(sede, modalidad);
            if (HttpContext.Request.IsAjaxRequest())
            {
                return Json(listaPlanes, JsonRequestBehavior.AllowGet);
            }
            return View(listaPlanes);
        }

        public ActionResult ObtenerPlanesEstudioSede(int sede, int modalidad)
        {
            IQueryable listaPlanes = repositorioCursoProfesor.obtenerPlanesEstudioSede(sede, modalidad);
            if (HttpContext.Request.IsAjaxRequest())
            {
                return Json(listaPlanes, JsonRequestBehavior.AllowGet);
            }
            return View(listaPlanes);
        }

        public ActionResult ObtenerCursos(int plan)
        {
            IQueryable listaCursos = repositorioCursoProfesor.obtenerCursos(plan);
            if (HttpContext.Request.IsAjaxRequest())
            {
                /*return Json(new SelectList(
                    listaCursos,
                    "ID",
                    "Nombre"), JsonRequestBehavior.AllowGet
                    );*/

                var json = JsonConvert.SerializeObject(listaCursos);

                return Content(json);
            }
            return View(listaCursos);
        }

        public ActionResult ObtenerGrupos(int curso, int plan, int sede, int bloque, int periodo)
        {
            IQueryable listaGrupos = repositorioCursoProfesor.obtenerGrupos(curso,plan,sede,bloque, periodo);
            if (HttpContext.Request.IsAjaxRequest())
            {
                return Json(new SelectList(
                    listaGrupos,
                    "ID",
                    "Numero"), JsonRequestBehavior.AllowGet
                    );

                //var json = JsonConvert.SerializeObject(listaGrupos);
            }
            return View(listaGrupos);
        }

        public ActionResult ObtenerGruposSinProfe(int curso, int plan, int bloque,int sede)
        {
            IQueryable listaGrupos = repositorioCursoProfesor.obtenerGruposSinProfe(curso, plan, bloque,sede);
            if (HttpContext.Request.IsAjaxRequest())
            {
                return Json(new SelectList(
                    listaGrupos,
                    "ID",
                    "Numero"), JsonRequestBehavior.AllowGet
                    );

                //var json = JsonConvert.SerializeObject(listaGrupos);
            }
            return View(listaGrupos);
        }


        public ActionResult ObtenerInfo(int cursoxgrupo)
        {
            IQueryable listaInfo = repositorioCursoProfesor.obtenerInfo(cursoxgrupo);
            if (HttpContext.Request.IsAjaxRequest())
            {
                var json = JsonConvert.SerializeObject(listaInfo);

                return Content(json);
            }
            return View(listaInfo);
        }


        public ActionResult ObtenerHorario(int cursoxgrupo)
        {
            int idHorario = repositorioCursoProfesor.obtenerHorario(cursoxgrupo);
            IQueryable listaHorario = null;

            if (idHorario != 0)
            {
                listaHorario = repositorioCursoProfesor.obtenerInfoHorario(idHorario);

                var json = JsonConvert.SerializeObject(listaHorario);

               return Content(json);
            }

            return View(listaHorario);
        }

        [Authorize]
        public ActionResult Revocar()
        {
            if (Request.UrlReferrer != null)
            {
                ViewBag.returnUrl = Request.UrlReferrer.ToString();
            }
            else
            {
                ViewBag.returnUrl = null;
            }

            /* Se obtiene la lista de profesores */
            List<Profesore> ListaProfesores = repositorioProfesor.ObtenerTodosProfesores().ToList<Profesore>();

            if (ListaProfesores.Count > 0)
                ViewBag.Profesores = ListaProfesores;
            else
                ViewBag.Profesores = null;

            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult revocar(int sltCursosImpartidos)
        {
            var revocado = false;

            revocado = repositorioCursoProfesor.revocarProfesor(sltCursosImpartidos);

            if (revocado)
            {
                TempData[TempDataMessageKey] = "Profesor revocado del curso correctamente.";
            }
            else
            {
                TempData[TempDataMessageKey] = "Ocurrió un error al revocar el profesor.";
            }

            return RedirectToAction("Revocar");
        }


        public ActionResult ObtenerCursosPorProfesor(int idProfesor)
        {
            IQueryable listaCursos = repositorioCursoProfesor.obtenerCursosPorProfesor(idProfesor);
            if (HttpContext.Request.IsAjaxRequest())
            {
                var json = JsonConvert.SerializeObject(listaCursos);

                return Content(json);
            }
            return View(listaCursos);
        }
    }
}
