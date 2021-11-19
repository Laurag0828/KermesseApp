using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KermesseApp.Models;
using System.Data.Entity.Validation;
using System.Data.Entity;
using Microsoft.Reporting.WebForms;
using System.IO;


namespace KermesseApp.Controllers
{
    public class Tbl_rolController : Controller
    {
        private KERMESSEEntities db = new KERMESSEEntities();
        // GET: Tbl_rol
        public ActionResult ListRol()
        {
            return View(db.tbl_rol.ToList());
        }
        public ActionResult VGuardarRol()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GuardarRol(tbl_rol trl)
        {
            if (ModelState.IsValid)
            {
                tbl_rol trol = new tbl_rol();
                trol.rol = trl.rol;
                trol.rol_desc = trl.rol_desc;
                trol.estado = 1;
                db.tbl_rol.Add(trol);
                db.SaveChanges();
            }
            ModelState.Clear();

            return View("VGuardarRol");
        }

        public ActionResult DeleteRol(int id)
        {
            tbl_rol trol = new tbl_rol();
            trol = db.tbl_rol.Find(id);
            db.tbl_rol.Remove(trol);

            db.SaveChanges();
            var list = db.tbl_rol.ToList();
            return View("ListRol", list);

        }

        public ActionResult VerRol(int id)
        {
            var Rol = db.tbl_rol.Where(x => x.id_rol == id).First();
            return View(Rol);
        }

        public ActionResult EditRol(int id)
        {
            tbl_rol tbr = db.tbl_rol.Find(id);
            if (tbr == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(tbr);
            }
        }

        [HttpPost]
            public ActionResult UpdRol(tbl_rol tr)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        db.Entry(tr).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    return RedirectToAction("ListRol");
                }
                catch
                {
                    return View();
                }            

        }

        [HttpPost]
        public ActionResult FiltrarRol(String cadena)
        {
            if (String.IsNullOrEmpty(cadena))
            {
                var list = db.tbl_rol.ToList();
                return View("ListRol", list);
            }
            else
            {
                var listFiltrada = db.tbl_rol.Where(X => X.rol.Contains(cadena) || X.rol_desc.Contains(cadena) );
                return View("ListRol", listFiltrada);
            }

        }

        public ActionResult VerRptRol(String tipo)
        {
            LocalReport rpt = new LocalReport();
            String mt, enc, f;
            String[] s;
            Warning[] w;

            String ruta = Path.Combine(Server.MapPath("~/Reportes"), "rptRol.rdlc");
            rpt.ReportPath = ruta;

            List<tbl_rol> listaRol = new List<tbl_rol>();
            listaRol = db.tbl_rol.ToList();

            ReportDataSource rds = new ReportDataSource("dsRptRol", listaRol);
            rpt.DataSources.Add(rds);

            var b = rpt.Render(tipo, null, out mt, out enc, out f, out s, out w);

            return new FileContentResult(b, mt);
        }
    }
}