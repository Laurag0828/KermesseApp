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
    public class Tbl_denominacionController : Controller
    {

        private KERMESSEEntities db = new KERMESSEEntities();

        // GET: tbl_denominacion
        public ActionResult ListDen()
        {

            var obj = db.tbl_denominacion.ToList();

            return View(obj);
        }

        public ActionResult GuardarDen()
        {

            ViewBag.id_moneda = new SelectList(db.tbl_moneda, "id_moneda", "nombre");

            return View();
        }

        public ActionResult EditarDen()
        {

            ViewBag.id_moneda = new SelectList(db.tbl_moneda, "id_moneda", "nombre");

            return View();
        }

        [HttpPost]
        public ActionResult GuardarDen(tbl_denominacion td)   // --Guardar--
        {
            if (ModelState.IsValid)
            {
                tbl_denominacion tden = new tbl_denominacion();
                tden.id_moneda = td.id_moneda;
                tden.valor = td.valor;
                tden.valor_letras = td.valor_letras;
                tden.estado = 1;
                db.tbl_denominacion.Add(tden);
                db.SaveChanges();
                ModelState.Clear();
            }

            ViewBag.id_moneda = new SelectList(db.tbl_moneda, "id_moneda", "nombre");

            return View("GuardarDen");
        }

        public ActionResult DeleteDen(int id) //Metodo para eliminar
        {

            tbl_denominacion tden = new tbl_denominacion();
            tden = db.tbl_denominacion.Find(id);
            db.tbl_denominacion.Remove(tden);
            db.SaveChanges();

            var list = db.tbl_denominacion.ToList();

            return View("ListDen", list);
        }

        public ActionResult VerDen(int id)  //Metodo para visualizar
        {

            var td = db.tbl_denominacion.Where(x => x.id_denominacion == id).First();

            return View(td);
        }

        public ActionResult EditDen(int id)
        {

            tbl_denominacion td = db.tbl_denominacion.Find(id);
            ViewBag.id_moneda = new SelectList(db.tbl_moneda, "id_moneda", "nombre");
            if (td == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(td);
            }
        }

        [HttpPost]
        public ActionResult UpdateDen(tbl_denominacion td) //Actualizamos el campo estado
        {

            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(td).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("ListDen");
            }
            catch
            {
                return View();
            }

        }

        [HttpPost]
        public ActionResult FiltroDen(String cadena)
        {

            if (String.IsNullOrEmpty(cadena))
            {
                var list = db.tbl_denominacion.ToList();
                return View("ListDen", list);
            }
            else
            {
                var listFiltrada = db.tbl_denominacion.Where(x => x.valor_letras.Contains(cadena));
                return View("ListDen", listFiltrada);
            }

        }

        public ActionResult ReporteDen(string tipo, string cadena)
        {

            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "rptden.rdlc");
            rpt.ReportPath = ruta;

            ReportDataSource rd = null;
            if (string.IsNullOrEmpty(cadena))
            {
                var lista = db.Vw_Den.ToList();
                rd = new ReportDataSource("dsrptDen", lista);
            }
            else
            {
                var lista = db.Vw_Den.Where(x => x.nombre.Contains(cadena) || x.valor_letras.Contains(cadena));
                rd = new ReportDataSource("dsrptDen", lista);
            }

            rpt.DataSources.Add(rd);

            var b = rpt.Render(tipo, null, out mt, out enc, out f, out s, out w);
            return new FileContentResult(b, mt);

            return View();
        }
    }
}