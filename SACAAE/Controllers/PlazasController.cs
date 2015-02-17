using SACAAE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace SACAAE.Controllers
{
    public class PlazasController : Controller
    {
        //
        // GET: /Plazas/

        private const string TempDataMessageKey = "Message";
        private RepositorioPlazas repositorio = new RepositorioPlazas();
        private RepositorioPlazaProfesor repositorioPlazaXProfesor = new RepositorioPlazaProfesor();

        [Authorize]
        public ActionResult Index()
        {
            var model = repositorio.ObtenerTodasPlazas();
            return View(model);
        }

        //
        // GET: /Plazas/Details/5
        [Authorize]
        public ActionResult Detalles(int id)
        {
            var model = repositorio.ObtenerPlaza(id);
            return View(model);
        }

        //
        // GET: /Plazas/Create
        [Authorize]
        public ActionResult Crear()
        {
            var model = new Plaza();
            return View(model);
        }

        //
        // POST: /Plazas/Create
        [Authorize]
        [HttpPost]
        public ActionResult Crear(Plaza nuevaPlaza)
        {
            repositorio.CrearPlaza(nuevaPlaza.Codigo_Plaza, nuevaPlaza.Tipo_de_plaza, nuevaPlaza.Tipo_segun_tiempo, nuevaPlaza.Horas_Totales, nuevaPlaza.Tiempo_de_vigencia);
            TempData[TempDataMessageKey] = "Plaza creada correctamente.";
            return RedirectToAction("Index");
        }

        //
        // GET: /Plazas/Edit/5
        [Authorize]
        public ActionResult Editar(int id)
        {
            var model = repositorio.ObtenerPlaza(id);
            return View(model);
        }

        //
        // POST: /Plazas/Edit/5
        [Authorize]
        [HttpPost]
        public ActionResult Editar(Plaza plaza)
        {
            if (ModelState.IsValid)
            {
                repositorio.Actualizar(plaza);
                TempData[TempDataMessageKey] = "Plaza editada correctamente";
            }
            return RedirectToAction("Index");
        }

        //
        // GET: /Plazas/Delete/5
        [Authorize]
        public ActionResult Eliminar(int id)
        {
            var model = repositorio.ObtenerPlaza(id);
            return View(model);
        }

        //
        // POST: /Plazas/Delete/5
        [Authorize]
        [HttpPost]
        public ActionResult Eliminar(Plaza plaza)
        {
            bool res=repositorio.BorrarPlaza(plaza);
            if(res)
            {
                TempData[TempDataMessageKey] = "Plaza eliminada correctamente";
            }
            else
            {

                TempData[TempDataMessageKey] = "Debe liberar las horas de la plaza";
            }
            return RedirectToAction("Index");
        }

        //
        // GET: /Plazas/Assign/5
        [Authorize]
        public ActionResult Asignar()
        {
           
            ViewBag.Plaza = repositorio.ObtenerTodasPlazas();
            ViewBag.Profesores = repositorio.ObtenerProfesoresPlaza();
            ViewBag.PlazasAsignadas = repositorio.ObtenerPlazasAsignadas();
            return View();            
        }

        // POST: /Plazas/Delete/5
        [Authorize]
        [HttpPost]
        public ActionResult Asignar(String sltPlaza, String sltProfesor, PlazaXProfesor plaza) 
        {
            repositorioPlazaXProfesor.AsignarPlaza(sltPlaza, sltProfesor, plaza.Horas_Asignadas);
            TempData[TempDataMessageKey] = "Plaza creada correctamente.";
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Nombrar()
        {

            ViewBag.Plaza = repositorio.ObtenerTodasPlazas();
            ViewBag.Profesores = repositorio.ObtenerProfesoresPlaza();
            ViewBag.PlazasAsignadas = repositorio.ObtenerPlazasAsignadas();
            return View();
        }

        [Authorize]
        public ActionResult QuitarNombramiento()
        {

            ViewBag.Plaza = repositorio.ObtenerTodasPlazas();
            ViewBag.Profesores = repositorio.ObtenerProfesoresPlaza();
            ViewBag.PlazasAsignadas = repositorio.ObtenerPlazasAsignadas();
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Nombrar(String sltPlaza, String sltProfesor, PlazaXProfesor plaza)
        {
            bool res=repositorioPlazaXProfesor.NombrarPlaza(sltPlaza, sltProfesor);
            if (res) { TempData[TempDataMessageKey] = "Plaza nombrada correctamente."; }
            else
            { TempData[TempDataMessageKey] = "Debe liberar las horas de la plaza"; }
            
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpPost]
        public ActionResult QuitarNombramiento(String sltPlaza, String sltProfesor, PlazaXProfesor plaza)
        {
            repositorioPlazaXProfesor.QuitarNombramiento(sltPlaza, sltProfesor);
            TempData[TempDataMessageKey] = "Nombramiento removido satisfactoriamente";

            return RedirectToAction("Index");
        }
        public ActionResult Liberar()
        {

            ViewBag.Plaza = repositorio.ObtenerTodasPlazas();
            ViewBag.Profesores = repositorio.ObtenerProfesoresPlaza();
            ViewBag.PlazasAsignadas = repositorio.ObtenerPlazasAsignadas();
            return View();
        }

        // POST: /Plazas/Delete/5
        [Authorize]
        [HttpPost]
        public ActionResult Liberar(String sltPlaza, String sltProfesor, PlazaXProfesor plaza)
        {
            repositorioPlazaXProfesor.LiberarHoras(sltPlaza, sltProfesor, plaza.Horas_Asignadas);
            TempData[TempDataMessageKey] = "Horas de Plaza liberada correctamente.";
            return RedirectToAction("Index");
        }



        public ActionResult getPlaza(int idPlaza)
        {
            var Plaza = repositorio.ObtenerHorasTotalesPlaza(idPlaza);
            return Json(Plaza, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getPlazaXProfesor(int idPlaza, int idProfesor)
        {
            var Plaza = repositorio.ObtenerHorasTotalesPlazaXProfesor(idPlaza,idProfesor);
            return Json(Plaza, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getPlazaXProfesorNombrado(int idPlaza)
        {
            var Profesor = repositorioPlazaXProfesor.obtenerProfeNombradoPorPlaza(idPlaza);
            return Json(Profesor, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getListPlazaXProfesor(int idPlaza)
        {
            var Profesor = repositorioPlazaXProfesor.obtenerProfePorPlaza(idPlaza);
            return Json(Profesor, JsonRequestBehavior.AllowGet);
        }
    }
}
