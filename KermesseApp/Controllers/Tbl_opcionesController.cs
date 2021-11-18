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
    public class Tbl_opcionesController : Controller
    {
        private KERMESSEEntities db = new KERMESSEEntities();
        // GET: Tbl_opciones
        public ActionResult ListOpciones()
        {
            return View(db.tbl_opciones.ToList());
        }
        public ActionResult VGuardarOpciones()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GuardarOpciones(tbl_opciones top)
        {
            if (ModelState.IsValid)
            {
            tbl_opciones topc = new tbl_opciones();
            topc.opcion = top.opcion;
            topc.estado = 1;
            db.tbl_opciones.Add(topc);
            db.SaveChanges();
            }
            ModelState.Clear();
            return View("VGuardarOpciones");
        }


        public ActionResult DeleteOpciones(int id)
        {
            tbl_opciones top = new tbl_opciones();
            top = db.tbl_opciones.Find(id);
            db.tbl_opciones.Remove(top);



            db.SaveChanges();
            var list = db.tbl_opciones.ToList();
            return View("ListOpciones", list);

        }


        public ActionResult VerOpciones(int id)
        {
            var Opciones = db.tbl_opciones.Where(x => x.id_opciones == id).First();
            return View(Opciones);
        }

        public ActionResult EditOpciones(int id)
        {
            tbl_opciones tbo = db.tbl_opciones.Find(id);
            if (tbo == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(tbo);
            }

        }

        [HttpPost]
        public ActionResult UpdOpciones(tbl_opciones to)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(to).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("ListOpciones");
            }
            catch
            {
                return View();
            }


        }

        [HttpPost]
        public ActionResult FiltrarOpciones(String cadena)
        {
            if (String.IsNullOrEmpty(cadena))
            {
                var list = db.tbl_opciones.ToList();
                return View("ListOpciones", list);
            }
            else
            {
                var listFiltrada = db.tbl_opciones.Where(X => X.opcion.Contains(cadena));
                return View("ListOpciones", listFiltrada);
            }

        }

        public ActionResult VerRptOpciones(String tipo)
        {
            LocalReport rpt = new LocalReport();
            String mt, enc, f;
            String[] s;
            Warning[] w;

            String ruta = Path.Combine(Server.MapPath("~/Reportes"), "rptOpciones.rdlc");
            rpt.ReportPath = ruta;

            List<tbl_opciones> listaOpciones = new List<tbl_opciones>();
            listaOpciones = db.tbl_opciones.ToList();

            ReportDataSource rds = new ReportDataSource("dsRptOpciones", listaOpciones);
            rpt.DataSources.Add(rds);

            var b = rpt.Render(tipo, null, out mt, out enc, out f, out s, out w);

            return new FileContentResult(b, mt);
        }


    }
}