using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using KermesseApp.Models;
using Microsoft.Reporting.WebForms;
using System.IO;

namespace KermesseApp.Controllers
{
    public class Tbl_monedaController : Controller
    {

        private KERMESSEEntities db = new KERMESSEEntities();

        // GET: Tbl_moneda
        public ActionResult tbl_Moneda() //Vista de la Moneda
        {
            return View(db.tbl_moneda.ToList());
        }

        public ActionResult GuardarMon()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GuardarMon(tbl_moneda tm)   // Metodo para Guardar
        {

            if( ModelState.IsValid)
            {
                tbl_moneda tmon = new tbl_moneda();
                tmon.nombre = tm.nombre;
                tmon.signo = tm.signo;
                tmon.estado = 1;
                db.tbl_moneda.Add(tmon);
                db.SaveChanges();
            }
            ModelState.Clear();

            return View("GuardarMon");
        }

        public ActionResult DeleteMon(int id) //Metodo para eliminar
        {

            tbl_moneda tmon = new tbl_moneda();
            tmon = db.tbl_moneda.Find(id);
            db.tbl_moneda.Remove(tmon);
            db.SaveChanges();

            var list = db.tbl_moneda.ToList();

            return View("tbl_Moneda", list);
        }

        public ActionResult VerMon(int id)  //Metodo para visualizar
        {

            var Mon = db.tbl_moneda.Where(x => x.id_moneda == id).First();

            return View(Mon);
        }

        public ActionResult EditMon(int id)
        {

            tbl_moneda tmon = db.tbl_moneda.Find(id);
            if (tmon == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(tmon);
            }
        }

        [HttpPost]
        public ActionResult UpdateMon(tbl_moneda tmon) //Actualizamos el campo estado
        {

            try
            {
                if(ModelState.IsValid)
                {
                    db.Entry(tmon).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("tbl_Moneda");
            }
            catch
            {
                return View();
            }

        }

        [HttpPost]
        public ActionResult FiltroMon(String cadena)
        {

            if(String.IsNullOrEmpty(cadena))
            {
                var list = db.tbl_moneda.ToList();
                return View("tbl_Moneda", list);
            }
            else
            {
                var listFiltrada = db.tbl_moneda.Where(x => x.nombre.Contains(cadena) || x.signo.Contains(cadena));
                return View("tbl_Moneda", listFiltrada);
            }

        }

        public ActionResult ReporteMon(string tipo, string cadena)
        {

            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "rptMon.rdlc");
            rpt.ReportPath = ruta;

            ReportDataSource rd = null;
            if (string.IsNullOrEmpty(cadena))
            {
                var lista = db.tbl_moneda.ToList();
                rd = new ReportDataSource("dsrptMon", lista);
            }
            else
            {
                var lista = db.tbl_moneda.Where(x => x.nombre.Contains(cadena) || x.signo.Contains(cadena));
                rd = new ReportDataSource("dsrptMon", lista);
            }
            
            rpt.DataSources.Add(rd);

            var b = rpt.Render(tipo, null, out mt, out enc, out f, out s, out w);
            return new FileContentResult(b, mt);

            return View();
        }
    }
}