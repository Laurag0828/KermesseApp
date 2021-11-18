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

            var obj = db.Vw_Arqueo.ToList();

            return View(obj);
        }

        public ActionResult GuardarArqueo()
        {

            ViewBag.id_arqueocaja = new SelectList(db.tbl_arqueocaja, "id_arqueocaja", "fecha_arqueo");
            ViewBag.id_moneda = new SelectList(db.tbl_moneda, "id_moneda", "nombre");
            ViewBag.id_denominacion = new SelectList(db.tbl_denominacion, "id_denominacion", "valor");
            ViewBag.id_kermesse = new SelectList(db.tbl_kermesse, "id_kermesse", "nombre");

            return View();
        }

        [HttpPost]
        public ActionResult GuardarArqueo(string kermesse, DateTime fecha_arqueo, string total, tbl_arqueocaja_det[] detalle)
        {

            ViewBag.id_moneda = new SelectList(db.tbl_moneda, "id_moneda", "nombre");
            ViewBag.id_denominacion = new SelectList(db.tbl_denominacion, "id_denominacion", "valor");
            ViewBag.id_kermesse = new SelectList(db.tbl_kermesse, "id_kermesse", "nombre");

            string result = "Error! Order Is Not Complete!";
                
                tbl_arqueocaja model = new tbl_arqueocaja();
                model.id_kermesse = Int32.Parse(kermesse);
                model.fecha_arqueo = fecha_arqueo;
                model.gran_total = Decimal.Parse(total);
                model.usuario_creacion = 1;
                model.estado = 1;
                db.tbl_arqueocaja.Add(model);

                foreach (var item in detalle)
                {
                    
                    tbl_arqueocaja_det O = new tbl_arqueocaja_det();
                    O.id_arqueocaja = model.id_arqueocaja;
                    O.id_moneda = 1;
                    O.id_denominacion = item.id_denominacion;
                    O.cantidad = item.cantidad;
                    O.subtotal = item.subtotal;
                    db.tbl_arqueocaja_det.Add(O);
                }
                db.SaveChanges();
                result = "Success! Order Is Complete!";

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public ActionResult GuardarArqueo(tbl_arqueocaja ta, tbl_arqueocaja_det td)   // Metodo para Guardar
        //{

        //    if (ModelState.IsValid)
        //    {
        //        tbl_arqueocaja_det tad = new tbl_arqueocaja_det();
        //        tad.id_arqueocaja = td.id_arqueocaja;
        //        tad.id_moneda = td.id_moneda;
        //        tad.id_denominacion = td.id_denominacion;
        //        tad.cantidad = td.cantidad;
        //        tad.subtotal = td.subtotal;

        //        db.tbl_arqueocaja_det.Add(tad);
        //        db.SaveChanges();


        //        tbl_arqueocaja tac = new tbl_arqueocaja();
        //        tac.id_kermesse = ta.id_kermesse;
        //        tac.fecha_arqueo = ta.fecha_arqueo;
        //        tac.gran_total = ta.gran_total;
        //        tac.usuario_creacion = 1;
        //        tac.estado = 1;
        //        db.tbl_arqueocaja.Add(tac);
        //        db.SaveChanges();

        //        ModelState.Clear();
        //        //return RedirectToAction("ListArqueo");
        //    }

        //    ViewBag.id_arqueocaja = new SelectList(db.tbl_arqueocaja, "id_arqueocaja", "fecha_arqueo");
        //    ViewBag.id_moneda = new SelectList(db.tbl_moneda, "id_moneda", "nombre");
        //    ViewBag.id_denominacion = new SelectList(db.tbl_denominacion, "id_denominacion", "valor");
        //    ViewBag.id_kermesse = new SelectList(db.tbl_kermesse, "id_kermesse", "nombre");

        //    return View("GuardarArqueo");
        //}

    }
}