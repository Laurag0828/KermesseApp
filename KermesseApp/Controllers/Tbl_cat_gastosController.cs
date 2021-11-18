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
    public class Tbl_cat_gastosController : Controller
    {

        private KERMESSEEntities db = new KERMESSEEntities();

        // GET: Tbl_cat_gastos
        public ActionResult ListaCatGastos() //Vista de Cat Gastos
        {
            return View(db.tbl_cat_gastos.ToList());
        }

        public ActionResult GuardarCatGast()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GuardarCatGast(tbl_cat_gastos tcg)   // Metodo para Guardar
        {

            if (ModelState.IsValid)
            {
                tbl_cat_gastos tcatg = new tbl_cat_gastos();
                tcatg.nombre_cat = tcg.nombre_cat;
                tcatg.desc_cat = tcg.desc_cat;
                tcatg.estado = 1;
                db.tbl_cat_gastos.Add(tcatg);
                db.SaveChanges();
            }
            ModelState.Clear();

            return View("GuardarCatGast");
        }

        public ActionResult DeleteCatGast(int id) //Metodo para eliminar
        {

            tbl_cat_gastos tcatg = new tbl_cat_gastos();
            tcatg = db.tbl_cat_gastos.Find(id);
            db.tbl_cat_gastos.Remove(tcatg);
            db.SaveChanges();

            var list = db.tbl_cat_gastos.ToList();

            return View("ListaCatGastos", list);
        }

        public ActionResult VerCatGast(int id)  //Metodo para visualizar
        {

            var Cg = db.tbl_cat_gastos.Where(x => x.id_cat_gasto == id).First();

            return View(Cg);
        }

        public ActionResult EditCatGast(int id)
        {

            tbl_cat_gastos tcatg = db.tbl_cat_gastos.Find(id);
            if (tcatg == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(tcatg);
            }
        }

        [HttpPost]
        public ActionResult UpdateCatGast(tbl_cat_gastos tcatg) //Actualizamos el campo estado
        {

            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(tcatg).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("ListaCatGastos");
            }
            catch
            {
                return View();
            }

        }

        [HttpPost]
        public ActionResult FiltroCatGast(String cadena)
        {

            if (String.IsNullOrEmpty(cadena))
            {
                var list = db.tbl_cat_gastos.ToList();
                return View("ListaCatGastos", list);
            }
            else
            {
                var listFiltrada = db.tbl_cat_gastos.Where(x => x.nombre_cat.Contains(cadena) || x.desc_cat.Contains(cadena));
                return View("ListaCatGastos", listFiltrada);
            }

        }

        public ActionResult ReporteCatGast(string tipo)
        {

            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "rptCG.rdlc");
            rpt.ReportPath = ruta;

            List<tbl_cat_gastos> lista = new List<tbl_cat_gastos>();
            lista = db.tbl_cat_gastos.ToList();

            ReportDataSource rd = new ReportDataSource("dsrptCG", lista);
            rpt.DataSources.Add(rd);

            var b = rpt.Render(tipo, null, out mt, out enc, out f, out s, out w);
            return new FileContentResult(b, mt);

            return View();
        }
    }
}