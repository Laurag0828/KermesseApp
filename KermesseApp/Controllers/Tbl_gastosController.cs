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
    public class Tbl_gastosController : Controller
    {

        private KERMESSEEntities db = new KERMESSEEntities();

        // GET: Tbl_gastos
        public ActionResult ListGastos()
        {

            var obj = db.tbl_gastos.ToList();

            return View(obj);
        }
        public ActionResult VistaRptGastos()
        {

            ViewBag.id_cat_gasto = new SelectList(db.tbl_cat_gastos, "id_cat_gasto", "nombre_cat");

            return View();
        }

        public ActionResult GuardarGastos()
        {

            ViewBag.id_kermesse = new SelectList(db.tbl_kermesse, "id_kermesse", "nombre");
            ViewBag.id_cat_gasto = new SelectList(db.tbl_cat_gastos, "id_cat_gasto", "nombre_cat");

            return View();
        }

        public ActionResult EditarGastos()
        {

            ViewBag.id_kermesse = new SelectList(db.tbl_kermesse, "id_kermesse", "nombre");
            ViewBag.id_cat_gasto = new SelectList(db.tbl_cat_gastos, "id_cat_gasto", "nombre_cat");

            return View();
        }

        [HttpPost]
        public ActionResult GuardarGastos(tbl_gastos tg)   // Metodo para Guardar
        {

            if (ModelState.IsValid)
            {
                tbl_gastos tgt = new tbl_gastos();
                tgt.id_kermesse = tg.id_kermesse;
                tgt.id_cat_gasto = tg.id_cat_gasto;
                tgt.concepto = tg.concepto;
                tgt.monto = tg.monto;
                tgt.fecha_gasto = tg.fecha_gasto;
                tgt.id_cat_gasto = tg.id_cat_gasto;
                tgt.usuario_creacion = 1;
                tgt.estado = 1;
                db.tbl_gastos.Add(tgt);
                db.SaveChanges();
                ModelState.Clear();
                //return RedirectToAction("ListGastos");
            }

            ViewBag.id_kermesse = new SelectList(db.tbl_kermesse, "id_kermesse", "nombre");
            ViewBag.id_cat_gasto = new SelectList(db.tbl_cat_gastos, "id_cat_gasto", "nombre_cat");

            return View("GuardarGastos");
        }

        public ActionResult DeleteGasto(int id) //Metodo para eliminar
        {

            tbl_gastos tgt = new tbl_gastos();
            tgt = db.tbl_gastos.Find(id);
            db.tbl_gastos.Remove(tgt);
            db.SaveChanges();

            var list = db.tbl_gastos.ToList();

            return View("ListGastos", list);
        }

        public ActionResult VerGastos(int id)  //Metodo para visualizar
        {

            var gast = db.tbl_gastos.Where(x => x.id_gasto == id).First();

            return View(gast);
        }

        public ActionResult EditGastos(int id)
        {

            tbl_gastos tgt = db.tbl_gastos.Find(id);
            ViewBag.id_kermesse = new SelectList(db.tbl_kermesse, "id_kermesse", "nombre");
            ViewBag.id_cat_gasto = new SelectList(db.tbl_cat_gastos, "id_cat_gasto", "nombre_cat");
            if (tgt == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(tgt);
            }
        }

        [HttpPost]
        public ActionResult UpdateGasto(tbl_gastos tg) //Actualizamos el campo estado
        {

           
                    tg.estado = 2;
                    db.Entry(tg).State = EntityState.Modified;
                    db.SaveChanges();
              
                return RedirectToAction("ListGastos");
           

        }

        [HttpPost]
        public ActionResult FiltroGastos(String cadena)
        {

            if (String.IsNullOrEmpty(cadena))
            {
                var list = db.tbl_gastos.ToList();
                return View("ListGastos", list);
            }
            else
            {
                var listFiltrada = db.tbl_gastos.Where(x => x.concepto.Contains(cadena));
                return View("ListGastos", listFiltrada);
            }

        }

        [HttpPost]
        public ActionResult ReporteGast(tbl_gastos tg)
        {

            LocalReport rpt = new LocalReport();
            string mt, enc, f, tipo;
            string[] s;
            Warning[] w;

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "rptfiltraGastos.rdlc");
            rpt.ReportPath = ruta;

            //List<tbl_moneda> lista = new List<tbl_moneda>();
            var listafiltrada = db.Vw_gastos_cat.Where(x => x.id_cat_gasto == tg.id_cat_gasto);

            ReportDataSource rd = new ReportDataSource("dsFiltraGastos", listafiltrada);
            rpt.DataSources.Add(rd);
            tipo = "PDF";
            var b = rpt.Render(tipo, null, out mt, out enc, out f, out s, out w);
            return new FileContentResult(b, mt);

            return View();
        }
    }
}