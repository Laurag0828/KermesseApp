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
    public class Tbl_rolusuarioController : Controller
    {
        private KERMESSEEntities db = new KERMESSEEntities();
        // GET: Tbl_rolusuario
        public ActionResult ListRolUsuario()
        {
            return View(db.tbl_rol_usuario.ToList());
        }
        public ActionResult VGuardarRolUsuario()
        {
            return View();
        }

        public ActionResult VistaInsertRolUsuario()
        {
            ViewBag.id_rol = new SelectList(db.tbl_rol, "id_rol", "rol_desc");
            ViewBag.id_usuario = new SelectList(db.tbl_usuario, "id_usuario", "usuario");
            return View();
        }

        [HttpPost]
        public ActionResult GuardarRolUsuario(tbl_rol_usuario trolu)
        {
            if (ModelState.IsValid)
            {
                tbl_rol_usuario trolus = new tbl_rol_usuario();
                trolus.id_rol = trolu.id_rol;
                trolus.id_usuario = trolu.id_usuario;
                db.tbl_rol_usuario.Add(trolus);
                db.SaveChanges();
                ModelState.Clear();
            }
            
           

            ViewBag.id_rol = new SelectList(db.tbl_rol, "id_rol", "rol_desc");
            ViewBag.id_usuario = new SelectList(db.tbl_usuario, "id_usuario", "usuario");
            return View("VistaInsertRolUsuario");
        }

  
        public ActionResult DeleteRolUsuario(int id)
        {
            tbl_rol_usuario tus = new tbl_rol_usuario();
            tus = db.tbl_rol_usuario.Find(id);
            int idrolusuario = tus.id_rol_usuario;
            db.tbl_rol_usuario.Remove(tus);
            db.SaveChanges();

            var list = db.tbl_rol_usuario.Where(model => model.id_rol_usuario == idrolusuario);
            return View("ListRolUsuario", list);
        }

       
        public ActionResult VerRolUsuario(int id)
        {
            var RolUsuario = db.tbl_rol_usuario.Where(x => x.id_rol_usuario == id).First();
            return View(RolUsuario);
        }

        public ActionResult EditRolUsuario(int id)
        {
            {
                tbl_rol_usuario tru = db.tbl_rol_usuario.Find(id);

                ViewBag.id_rol = new SelectList(db.tbl_rol, "id_rol", "rol_desc");
                ViewBag.id_usuario = new SelectList(db.tbl_usuario, "id_usuario", "usuario");

                return View(tru);
            }

        }

        [HttpPost]
        public ActionResult UpdRolUsuario(tbl_rol_usuario to)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(to).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("ListRolUsuario");
            }
            catch
            {
                return View();
            }


        }

        [HttpPost]
        public ActionResult FiltrarRolUsuario(String cadena)
        {
            if (String.IsNullOrEmpty(cadena))
            {
                var list = db.tbl_rol_usuario.ToList();
                return View("ListRolUsuario", list);
            }
            else
            {
                var listFiltrada = db.tbl_rol_usuario.Where(X => X.tbl_rol.rol_desc.Contains(cadena));
                return View("ListRolUsuario", listFiltrada);
            }

        }

        public ActionResult VerRptRolUsuario(String tipo)
        {
            LocalReport rpt = new LocalReport();
            String mt, enc, f;
            String[] s;
            Warning[] w;

            String ruta = Path.Combine(Server.MapPath("~/Reportes"), "rptRolUsuario.rdlc");
            rpt.ReportPath = ruta;

            List<tbl_rol_usuario> listaRolUsuario = new List<tbl_rol_usuario>();
            listaRolUsuario = db.tbl_rol_usuario.ToList();

            ReportDataSource rds = new ReportDataSource("dsRptRolUsuario", listaRolUsuario);
            rpt.DataSources.Add(rds);

            var b = rpt.Render(tipo, null, out mt, out enc, out f, out s, out w);

            return new FileContentResult(b, mt);
        }


    }
}