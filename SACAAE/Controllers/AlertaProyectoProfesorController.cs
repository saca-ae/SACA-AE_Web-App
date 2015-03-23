using SACAAE.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SACAAE.Controllers
{
    public class AlertaProyectoProfesorController : Controller
    {

        private SACAAEEntities entidades = new SACAAEEntities();

        // GET: AlertaProyectoProfesor
        public ActionResult Index(int dias)
        {
            return View(AlertaProyectoProfesor.getProyectosAtrasados(dias));
        }
        /*
        // GET: AlertaProyectoProfesor/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AlertaProyectoProfesor/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AlertaProyectoProfesor/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: AlertaProyectoProfesor/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AlertaProyectoProfesor/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: AlertaProyectoProfesor/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AlertaProyectoProfesor/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
         * */
    }
}
