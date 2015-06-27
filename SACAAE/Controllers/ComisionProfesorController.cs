using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SACAAE.Models;
using Newtonsoft.Json;

namespace SACAAE.Controllers
{
    public class ComisionProfesorController : Controller
    {
        //
        // GET: /ComisionProfesor/
        private RepositorioProfesor repositorioProfesor = new RepositorioProfesor();
        private RepositorioComision repositorioComision = new RepositorioComision();
        private RepositorioComisionesProfesor repositoriocomisionesprofesor = new RepositorioComisionesProfesor();
        private RepositorioPeriodos repoPeriodos = new RepositorioPeriodos();
        private const string TempDataMessageKey = "Message";

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Asignar()
        {

            List<String> HorasInicio = new List<String>();
            List<String> HorasFin = new List<String>();
            for (int i = 7; i < 23; i++)
            {
                HorasInicio.Add(i.ToString() + ":00");
                HorasFin.Add(i.ToString() + ":00");
            }
            ViewBag.HorasInicio = HorasInicio;
            ViewBag.HorasFin = HorasFin;


            if (Request.UrlReferrer != null)
            {
                ViewBag.returnUrl = Request.UrlReferrer.ToString();
            }
            else
            {
                ViewBag.returnUrl = null;
            }
            List<Profesore> ListaProfesores = repositorioProfesor.ObtenerTodosProfesores().ToList<Profesore>();
            List<Comisione> ListaComisiones = repositorioComision.ObtenerTodasComisiones().ToList<Comisione>();

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

            ViewBag.profesores = repositorioProfesor.ObtenerTodosProfesores();
            ViewBag.comisiones = repositorioComision.ObtenerComisionesXEntidad(entidadID);


            if (ListaProfesores.Count > 0)
                ViewBag.profesores = ListaProfesores;
            else
                ViewBag.profesores = null;

            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Asignar(String profesor, String comision)
        {
            int Cantidad;
            try
            {
                Cantidad = Convert.ToInt32(Request.Cookies["Cantidad"].Value);
                Cantidad++;
            }
            catch (Exception e)
            {
                Cantidad = 0;
            }

            String Periodo = Request.Cookies["Periodo"].Value;
            int IdPeriodo = repoPeriodos.IdPeriodo(Periodo);

            for (int i = 1; i < Cantidad; i++)
            {
                String Detalles = Request.Cookies["DiaSeleccionadoCookie" + i].Value;//Obtiene los datos de la cookie
                string[] Partes = Detalles.Split('|');

                String Dia = Partes[0];
                String HoraInicio = Partes[1];
                String HoraFin = Partes[2];


                if (Dia != "d")
                {
                    var creado = repositoriocomisionesprofesor.CrearComisionProfesor(profesor, comision, Dia, HoraInicio, HoraFin, IdPeriodo);
                    if (creado)
                    {
                        TempData[TempDataMessageKey] = "Profesor asignado correctamente.";
                    }
                    else
                    {
                        TempData[TempDataMessageKey] = "Ocurrió un error al asignar el profesor.";
                    }
                }
            }
            return RedirectToAction("Asignar");
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

        public ActionResult ObtenerComisionesXProfesor(int idProfesor)
        {
            IQueryable listaComisiones = repositoriocomisionesprofesor.ObtenerComisionesXProfesor(idProfesor);
            if (HttpContext.Request.IsAjaxRequest())
            {
                var json = JsonConvert.SerializeObject(listaComisiones);

                return Content(json);
            }
            return View(listaComisiones);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Revocar(int sltComisiones)
        {
            var revocado = false;

            revocado = repositoriocomisionesprofesor.revocarProfesor(sltComisiones);

            if (revocado)
            {
                TempData[TempDataMessageKey] = "Profesor revocado de comisión correctamente.";
            }
            else
            {
                TempData[TempDataMessageKey] = "Ocurrió un error al revocar el profesor.";
            }

            return RedirectToAction("Revocar");
        }
    }
}
