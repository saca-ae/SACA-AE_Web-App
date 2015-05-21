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
    [HandleError]
    public class SedesController : Controller
    {
        private repositorioSedes repositorio = new repositorioSedes();
        private const string TempDataMessageKey = "MessageError";
        private const string TempDataMessageKeySuccess = "MessageSuccess";

        // GET: Sedes
        [Authorize]
        public ActionResult Index()
        {
            var model = repositorio.ObtenerTodosSedes();
            return View(model);
        }

        // GET: Sedes/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            /**
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sede sede = db.Sedes.Find(id);
            if (sede == null)
            {
                return HttpNotFound();
            }
            return View(sede);*/
            var model = repositorio.ObtenerSede(id);
            return View(model);
        }

        // GET: Sedes/Create
        [Authorize]
        public ActionResult Create()
        {
            var model = new Sede();
            return View(model);
        }

        // POST: Sedes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "ID,Nombre")] Sede sede)
        {
            
            if (ModelState.IsValid)
            {
              //  db.Sedes.Add(sede);
               // db.SaveChanges();
                repositorio.crearSede(sede);
                return RedirectToAction("Index");
            }

            return View(sede);
           
        }
        /**
        // GET: Sedes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sede sede = db.Sedes.Find(id);
            if (sede == null)
            {
                return HttpNotFound();
            }
            return View(sede);
        }
    
        // POST: Sedes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Nombre")] Sede sede)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sede).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sede);
        }
        */
        // GET: Sedes/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            var model = repositorio.ObtenerSede(id);

            return View(model);
        }
        
        // POST: Sedes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Sede sede)
        {
            repositorio.borrarSede(sede);
            TempData[TempDataMessageKey] = "El registro ha sido eliminado correctamente.";
            return RedirectToAction("Index");
        }

        /**
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }*/
    }
}
