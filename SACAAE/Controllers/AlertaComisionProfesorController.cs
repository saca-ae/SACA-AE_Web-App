using SACAAE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SACAAE.Controllers
{
    public class AlertaComisionProfesorController : Controller
    {
        // GET: AlertaComisionProfesor
        public ActionResult Index(int dias)
        {
            return View(AlertaComisionProfesor.getComisionesAtrasados(dias));
        }
        /*
        // GET: AlertaComisionProfesor/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AlertaComisionProfesor/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AlertaComisionProfesor/Create
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

        // GET: AlertaComisionProfesor/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AlertaComisionProfesor/Edit/5
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

        // GET: AlertaComisionProfesor/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AlertaComisionProfesor/Delete/5
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
    }
         * */
    }
}
