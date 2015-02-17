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
    public class SeleccionarController : Controller
    {
        

        private RepositorioCursos repoCuros = new RepositorioCursos();
        private RepositorioPeriodos repoPeriodos = new RepositorioPeriodos();
        private RepositorioPlanesDeEstudio repoPlanes = new RepositorioPlanesDeEstudio();
        private RepositorioPlanesDeEstudio PlanesDeEstudio = new RepositorioPlanesDeEstudio();
        private repositorioSedes Sedes = new repositorioSedes();
        private repositorioModalidades Modalidades = new repositorioModalidades();
        private repositorioGrupos Grupos = new repositorioGrupos();
       


        public ActionResult SeleccionarGrupo()
        {
            List<String> NombrePlanesDeEstudio = new List<String>();
            foreach (var item in PlanesDeEstudio.ObtenerTodosPlanesDeEstudio())
            {
                NombrePlanesDeEstudio.Add(item.Nombre);
            }
            ViewBag.PlanesDeEstudio = NombrePlanesDeEstudio.Distinct();

            List<String> NombreSedes = new List<String>();
            foreach (var item in Sedes.ObtenerTodosSedes())
            {
                NombreSedes.Add(item.Nombre);
            }
            ViewBag.Sedes = NombreSedes.Distinct();

            List<String> NombreModalidades = new List<String>();
            foreach (var item in Modalidades.ObtenerTodosModalidades())
            {
                NombreModalidades.Add(item.Nombre);
            }
            ViewBag.Modalidades = NombreModalidades.Distinct();

            List<String> NombreGrupos = new List<String>();
            foreach (var item in Grupos.ObtenerTodosGrupos())
            {
                NombreGrupos.Add(item.Numero.ToString());
            }
            ViewBag.Grupos = NombreGrupos.Distinct();

            ViewBag.GruposCompleto = Grupos.ObtenerTodosGrupos();
           
            return View();
        }
 
        public ActionResult cursosPlanLista(string idPlan)
        {
            string[] Partes = idPlan.Split('|');
            String PlanDeEstudio = Partes[0];
            int Bloque = Convert.ToInt32(Partes[1]);
            String Sede = Partes[2];
            String Modalidad = Partes[3];

            int IdPlanDeEstudioXSede = PlanesDeEstudio.IdPlanDeEstudioXSede(PlanDeEstudio, Modalidad, Sede);
            //Periodo
            String Periodo = Request.Cookies["Periodo"].Value;
            int IdPeriodo = repoPeriodos.IdPeriodo(Periodo);
            IQueryable ListaGrupos = Grupos.ListaGrupos(IdPlanDeEstudioXSede, IdPeriodo, Bloque); 
            if (HttpContext.Request.IsAjaxRequest())
            {
                return Json(new SelectList(ListaGrupos, "ID", "Nombre"), JsonRequestBehavior.AllowGet);
            }
            return View(ListaGrupos); 
        }

        [HttpPost]
        public ActionResult Enviar()
        {
            return Redirect("/Horarios/Horarios");
        }




      
    }
}
