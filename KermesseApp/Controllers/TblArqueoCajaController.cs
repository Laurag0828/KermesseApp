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
    public class TblArqueoCajaController : Controller
    {

        private KERMESSEEntities db = new KERMESSEEntities();

        // GET: TblArqueoCaja
        public ActionResult ListArqueo()
        {

            //var obj = db.Vw_Arqueo.ToList();

            return View(db.tbl_arqueocaja.Where(model => model.estado != 3));
        }

        public ActionResult VistaRptArqueo()
        {

            ViewBag.id_kermesse = new SelectList(db.tbl_kermesse.Where(model => model.estado != 3), "id_kermesse", "nombre");

            return View();
        }

        public ActionResult GuardarArqueo()
        {

            ViewBag.id_moneda = new SelectList(db.tbl_moneda, "id_moneda", "nombre");
            ViewBag.id_denominacion = new SelectList(db.tbl_denominacion, "id_denominacion", "valor");
            ViewBag.id_kermesse = new SelectList(db.tbl_kermesse, "id_kermesse", "nombre");

            return View();
        }

        public ActionResult EditarArqueo()
        {

            ViewBag.id_kermesse = new SelectList(db.tbl_kermesse.Where(model => model.estado != 3), "id_kermesse", "nombre");

            return View();
        }

        public ActionResult EditarArqueoDetalle()
        {

            ViewBag.id_arqueocaja = new SelectList(db.tbl_arqueocaja.Where(model => model.estado != 3), "id_arqueocaja", "id_arqueocaja");
            ViewBag.id_moneda = new SelectList(db.tbl_moneda.Where(model => model.estado != 3), "id_moneda", "nombre");
            ViewBag.id_denominacion = new SelectList(db.tbl_denominacion.Where(model => model.estado != 3), "id_denominacion", "valor");

            return View();
        }

        public ActionResult GuardarArqueoA(int kermesse, DateTime fecha_arqueo, string total, tbl_arqueocaja_det[] detalle)
        {

            ViewBag.id_moneda = new SelectList(db.tbl_moneda, "id_moneda", "nombre");
            ViewBag.id_denominacion = new SelectList(db.tbl_denominacion, "id_denominacion", "valor");
            ViewBag.id_kermesse = new SelectList(db.tbl_kermesse, "id_kermesse", "nombre");

            string result = "Error! Order Is Not Complete!";
                
                tbl_arqueocaja model = new tbl_arqueocaja();
                model.id_kermesse = kermesse;
                model.fecha_arqueo = fecha_arqueo;
                model.gran_total = Decimal.Parse(total);
                model.usuario_creacion = 1;
                model.estado = 1;
                db.tbl_arqueocaja.Add(model);

                foreach (var item in detalle)
                {
                    
                    tbl_arqueocaja_det O = new tbl_arqueocaja_det();
                    O.id_arqueocaja = model.id_arqueocaja;
                    O.id_moneda = item.id_moneda;
                    O.id_denominacion = item.id_denominacion;
                    O.cantidad = item.cantidad;
                    O.subtotal = item.subtotal;
                    O.estado = 1;
                    db.tbl_arqueocaja_det.Add(O);
                }
                db.SaveChanges();
                result = "Success! Order Is Complete!";

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteArqueo(int id)
        {
            tbl_arqueocaja topc = new tbl_arqueocaja();
            topc = db.tbl_arqueocaja.Find(id);
            topc.estado = 3;
            db.Entry(topc).State = EntityState.Modified;
            db.SaveChanges();
            var list = db.tbl_arqueocaja.ToList();

            return RedirectToAction("ListArqueo");
        }

        public ActionResult VerArqueo(int id)  //Metodo para visualizar
        {

            var gast = db.Vw_Arqueo.Where(x => x.id_arqueocaja == id);

            return View(gast);
        }

        public ActionResult VerDetalle(int id)  //Metodo para visualizar
        {

            var gast = db.tbl_arqueocaja_det.Where(x => x.id_arqueocaja_det == id).First();

            return View(gast);
        }

        public ActionResult DeleteD(int id) //Metodo para eliminar
        {

            tbl_arqueocaja_det tgt = new tbl_arqueocaja_det();
            tgt = db.tbl_arqueocaja_det.Find(id);
            db.tbl_arqueocaja_det.Remove(tgt);
            db.SaveChanges();

            var list = db.tbl_arqueocaja_det.ToList();

            return RedirectToAction("VerArqueo", new { id = tgt.id_arqueocaja, list});
            //return RedirectToAction("VerArqueo", list);
        }

        public ActionResult EditArqueo(int id)
        {

            ViewBag.id_kermesse = new SelectList(db.tbl_kermesse.Where(model => model.estado != 3), "id_kermesse", "nombre");

            tbl_arqueocaja tgt = db.tbl_arqueocaja.Find(id);
            
            if (tgt == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(tgt);
            }

        }

        public ActionResult EditArqueoD(int id)
        {

            ViewBag.id_arqueocaja = new SelectList(db.tbl_arqueocaja.Where(model => model.estado != 3), "id_arqueocaja", "id_arqueocaja");
            ViewBag.id_moneda = new SelectList(db.tbl_moneda.Where(model => model.estado != 3), "id_moneda", "nombre");
            ViewBag.id_denominacion = new SelectList(db.tbl_denominacion.Where(model => model.estado != 3), "id_denominacion", "valor");

            tbl_arqueocaja_det tad = db.tbl_arqueocaja_det.Find(id);

            if (tad == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(tad);
            }

        }

        [HttpPost]
        public ActionResult UpdateArqueo(tbl_arqueocaja tg, tbl_arqueocaja_det td) //Actualizamos el campo estado
        {

            ViewBag.id_kermesse = new SelectList(db.tbl_kermesse.Where(model => model.estado != 3), "id_kermesse", "nombre");

            var det = db.tbl_arqueocaja_det.Where(Model => Model.id_arqueocaja == tg.id_arqueocaja);
            tg.gran_total = 0;

            foreach (var item in det)
            {
                tg.gran_total = tg.gran_total + item.subtotal;
            }

                    tg.estado = 2;
                    db.Entry(tg).State = EntityState.Modified;
                    db.SaveChanges();

                return RedirectToAction("ListArqueo");

        }

        [HttpPost]
        public ActionResult UpdateArqueoD(tbl_arqueocaja_det td, tbl_denominacion tde) //Actualizamos el campo estado
        {

            ViewBag.id_arqueocaja = new SelectList(db.tbl_arqueocaja.Where(model => model.estado != 3), "id_arqueocaja", "id_arqueocaja");
            ViewBag.id_moneda = new SelectList(db.tbl_moneda.Where(model => model.estado != 3), "id_moneda", "nombre");
            ViewBag.id_denominacion = new SelectList(db.tbl_denominacion.Where(model => model.estado != 3), "id_denominacion", "valor");

            var det = db.tbl_denominacion.Where(Model => Model.id_denominacion == td.id_denominacion);
            td.subtotal = 0;

            foreach (var item in det)
            {
                td.subtotal = item.valor * td.cantidad;
            }

                    td.estado = 2;
                    db.Entry(td).State = EntityState.Modified;
                    db.SaveChanges();

            //return RedirectToAction("VerArqueo");
            return RedirectToAction("VerArqueo", new { id = td.id_arqueocaja });

        }

        [HttpPost]
        public ActionResult FiltroArqueo(String cadena)
        {

            if (String.IsNullOrEmpty(cadena))
            {
                var list = db.Vw_Arqueo.ToList();
                return View("ListArqueo", list);
            }
            else
            {
                var listFiltrada = db.Vw_Arqueo.Where(x => x.nombre.Contains(cadena));
                return View("ListArqueo", listFiltrada);
            }

        }

        [HttpPost]
        public ActionResult ReporteArqueo(tbl_arqueocaja tg)
        {

            LocalReport rpt = new LocalReport();
            string mt, enc, f, tipo;
            string[] s;
            Warning[] w;

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "rptArqueo.rdlc");
            rpt.ReportPath = ruta;

            //List<tbl_moneda> lista = new List<tbl_moneda>();
            var listafiltrada = db.Vw_Arqueo.Where(x => x.id_kermesse == tg.id_kermesse);

            ReportDataSource rd = new ReportDataSource("dsArqueo", listafiltrada);
            rpt.DataSources.Add(rd);
            tipo = "PDF";
            var b = rpt.Render(tipo, null, out mt, out enc, out f, out s, out w);
            return new FileContentResult(b, mt);

            return View();
        }

    }
}