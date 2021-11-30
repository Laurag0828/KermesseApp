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
    public class Tbl_rolusuarioController : Controller
    {
        private KERMESSEEntities4 db = new KERMESSEEntities4();

        // GET: tbl_rol_usuario
        public ActionResult ListarRolUsuario
            ()
        {
            var tbl_rol_usuario = db.tbl_rol_usuario.Include(t => t.tbl_rol).Include(t => t.tbl_usuario);
            return View(tbl_rol_usuario.ToList());
        }

        // GET: tbl_rol_usuario/Details/5
        public ActionResult VerRolUs(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_rol_usuario tbl_rol_usuario = db.tbl_rol_usuario.Find(id);
            if (tbl_rol_usuario == null)
            {
                return HttpNotFound();
            }
            return View(tbl_rol_usuario);
        }

        // GET: tbl_rol_usuario/Create
        public ActionResult CreateRolUs()
        {
            ViewBag.id_rol = new SelectList(db.tbl_rol, "id_rol", "rol_desc");
            ViewBag.id_usuario = new SelectList(db.tbl_usuario, "id_usuario", "usuario");
            return View();
        }

        // POST: tbl_rol_usuario/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRolUs([Bind(Include = "id_rol_usuario,id_usuario,id_rol")] tbl_rol_usuario tbl_rol_usuario)
        {
            if (ModelState.IsValid)
            {
                db.tbl_rol_usuario.Add(tbl_rol_usuario);
                db.SaveChanges();
                return RedirectToAction("ListarRolUsuario");
            }

            ViewBag.id_rol = new SelectList(db.tbl_rol, "id_rol", "rol_desc", tbl_rol_usuario.id_rol);
            ViewBag.id_usuario = new SelectList(db.tbl_usuario, "id_usuario", "usuario", tbl_rol_usuario.id_usuario);
            return View(tbl_rol_usuario);
        }

        // GET: tbl_rol_usuario/Edit/5
        public ActionResult EditRolUs(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_rol_usuario tbl_rol_usuario = db.tbl_rol_usuario.Find(id);
            if (tbl_rol_usuario == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_rol = new SelectList(db.tbl_rol, "id_rol", "rol_desc", tbl_rol_usuario.id_rol);
            ViewBag.id_usuario = new SelectList(db.tbl_usuario, "id_usuario", "usuario", tbl_rol_usuario.id_usuario);
            return View(tbl_rol_usuario);
        }

        // POST: tbl_rol_usuario/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRolUs([Bind(Include = "id_rol_usuario,id_usuario,id_rol")] tbl_rol_usuario tbl_rol_usuario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_rol_usuario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ListarRolUsuario");
            }
            ViewBag.id_rol = new SelectList(db.tbl_rol, "id_rol", "rol_desc", tbl_rol_usuario.id_rol);
            ViewBag.id_usuario = new SelectList(db.tbl_usuario, "id_usuario", "usuario", tbl_rol_usuario.id_usuario);
            return View(tbl_rol_usuario);
        }

        // GET: tbl_rol_usuario/Delete/5
        public ActionResult DeleteRolUs(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_rol_usuario tbl_rol_usuario = db.tbl_rol_usuario.Find(id);
            if (tbl_rol_usuario == null)
            {
                return HttpNotFound();
            }
            return View(tbl_rol_usuario);
        }

        // POST: tbl_rol_usuario/Delete/5
        [HttpPost, ActionName("DeleteRolUs")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_rol_usuario tbl_rol_usuario = db.tbl_rol_usuario.Find(id);
            db.tbl_rol_usuario.Remove(tbl_rol_usuario);
            db.SaveChanges();
            return RedirectToAction("ListarRolUsuario");
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
        public ActionResult FiltrarRolUsuario(String cadena)
        {
            if (String.IsNullOrEmpty(cadena))
            {
                var list = db.tbl_rol_usuario.ToList();
                return View("ListRolUs", list);
            }
            else
            {
                var listFiltrada = db.tbl_rol_usuario.Where(X => X.tbl_rol.rol_desc.Contains(cadena));
                return View("ListRolUs", listFiltrada);
            }

        }
        public ActionResult VerRptRolUs(String tipo)
        {
            LocalReport rpt = new LocalReport();
            String mt, enc, f;
            String[] s;
            Warning[] w;

            String ruta = Path.Combine(Server.MapPath("~/Reportes"), "rptRolUs.rdlc");
            rpt.ReportPath = ruta;

            List<vw_rol_usuario> lis = new List<vw_rol_usuario>();
            lis = db.vw_rol_usuario.ToList();

            ReportDataSource rds = new ReportDataSource("dsRptRolUs", lis);
            rpt.DataSources.Add(rds);

            var b = rpt.Render(tipo, null, out mt, out enc, out f, out s, out w);

            return new FileContentResult(b, mt);
        }
    }
}