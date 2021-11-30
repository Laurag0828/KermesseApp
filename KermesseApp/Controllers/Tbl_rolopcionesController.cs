using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KermesseApp.Models;
using System.Data.Entity.Validation;
using System.Data.Entity;
using Microsoft.Reporting.WebForms;
using System.IO;

namespace KermesseApp.Controllers
{
    public class tbl_rolopcionesController : Controller
    {
        private KERMESSEEntities db = new KERMESSEEntities();

        // GET: tbl_rol_opciones
        public ActionResult ListRolOpciones()
        {
            var tbl_rol_opciones = db.tbl_rol_opciones.Include(t => t.tbl_rol).Include(t => t.tbl_opciones);
            return View(tbl_rol_opciones.ToList());
        }

        // GET: tbl_rol_opciones/Details/5
        public ActionResult VerRolOpciones(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_rol_opciones tbl_rol_opciones = db.tbl_rol_opciones.Find(id);
            if (tbl_rol_opciones == null)
            {
                return HttpNotFound();
            }
            return View(tbl_rol_opciones);
        }

        // GET: tbl_rol_opciones/Create
        public ActionResult CreateRolOpciones()
        {
            ViewBag.id_rol = new SelectList(db.tbl_rol, "id_rol", "rol_desc");
            ViewBag.id_opcion = new SelectList(db.tbl_opciones, "id_opciones", "opcion");
            return View();
        }

        // POST: tbl_rol_opciones/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRolOpciones([Bind(Include = "id_rol_opcion,id_opcion,id_rol")] tbl_rol_opciones tbl_rol_opciones)
        {
            if (ModelState.IsValid)
            {
                db.tbl_rol_opciones.Add(tbl_rol_opciones);
                db.SaveChanges();
                return RedirectToAction("ListRolOpciones");
            }

            ViewBag.id_rol = new SelectList(db.tbl_rol, "id_rol", "rol_desc", tbl_rol_opciones.id_rol);
            ViewBag.id_opciones = new SelectList(db.tbl_opciones, "id_opciones", "opcion", tbl_rol_opciones.id_opcion);
            return View(tbl_rol_opciones);
        }

        // GET: tbl_rol_opciones/Edit/5
        public ActionResult EditRolOpciones(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_rol_opciones tbl_rol_opciones = db.tbl_rol_opciones.Find(id);
            if (tbl_rol_opciones == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_rol = new SelectList(db.tbl_rol, "id_rol", "rol_desc", tbl_rol_opciones.id_rol);
            ViewBag.id_opcion = new SelectList(db.tbl_opciones, "id_opciones", "opcion", tbl_rol_opciones.id_opcion);
            return View(tbl_rol_opciones);
        }

        // POST: tbl_rol_opciones/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRolOpciones([Bind(Include = "id_rol_opciones,id_opciones,id_rol")] tbl_rol_opciones tbl_rol_opciones)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_rol_opciones).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ListRolOpciones");
            }
            ViewBag.id_rol = new SelectList(db.tbl_rol, "id_rol", "rol_desc", tbl_rol_opciones.id_rol);
            ViewBag.id_opcion = new SelectList(db.tbl_opciones, "id_opciones", "opcion", tbl_rol_opciones.id_opcion);
            return View(tbl_rol_opciones);
        }

        // GET: tbl_rol_opciones/Delete/5
        public ActionResult DeleteRolOpciones(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_rol_opciones tbl_rol_opciones = db.tbl_rol_opciones.Find(id);
            if (tbl_rol_opciones == null)
            {
                return HttpNotFound();
            }
            return View(tbl_rol_opciones);
        }

        // POST: tbl_rol_opciones/Delete/5
        [HttpPost, ActionName("DeleteRolOpciones")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_rol_opciones tbl_rol_opciones = db.tbl_rol_opciones.Find(id);
            db.tbl_rol_opciones.Remove(tbl_rol_opciones);
            db.SaveChanges();
            return RedirectToAction("ListRolOpciones");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        [HttpPost]
        public ActionResult FiltrarRolOpciones(String cadena)
        {
            if (String.IsNullOrEmpty(cadena))
            {
                var list = db.tbl_rol_opciones.ToList();
                return View("ListRolOpciones", list);
            }
            else
            {
                var listFiltrada = db.tbl_rol_opciones.Where(X => X.tbl_opciones.opcion.Contains(cadena));
                return View("ListRolOpciones", listFiltrada);
            }

        }

        public ActionResult VerRptRolOpc(String tipo)
        {
            LocalReport rpt = new LocalReport();
            String mt, enc, f;
            String[] s;
            Warning[] w;

            String ruta = Path.Combine(Server.MapPath("~/Reportes"), "rptRolOpc.rdlc");
            rpt.ReportPath = ruta;

            List<vw_rol_opciones> lis = new List<vw_rol_opciones>();
            lis = db.vw_rol_opciones.ToList();

            ReportDataSource rds = new ReportDataSource("dsRptRolOpc", lis);
            rpt.DataSources.Add(rds);

            var b = rpt.Render(tipo, null, out mt, out enc, out f, out s, out w);

            return new FileContentResult(b, mt);
        }
    }
}
