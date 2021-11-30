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
using Microsoft.Reporting.WebForms;
using System.IO;


namespace KermesseApp.Controllers
{
    public class tbl_listaprecioController : Controller
    {
        private KERMESSEEntities4 db = new KERMESSEEntities4();
        // GET: Tbl_listaprecio
        public ActionResult ListListaPrecio()
        {
            return View(db.tbl_listaprecio.Where(x => x.estado != 3));
        }
        public ActionResult ListListaPrecioDet(int id)
        {
            var detalle = db.tbl_listaprecio_det.Where(model => model.id_listaprecio == id);
            return View(detalle);
        }
        public ActionResult VistaRptListaPrecio()
        {
            ViewBag.id_listaprecio = new SelectList(db.tbl_listaprecio, "id_listaprecio", "nombre");
            ViewBag.id_producto = new SelectList(db.tbl_productos, "id_producto", "nombre");
            return View();
        }
        public ActionResult VGuardarListaPrecio()
        { 
            ViewBag.id_kermesse = new SelectList(db.tbl_kermesse.Where(model => model.estado != 3), "id_kermesse", "nombre");
            ViewBag.id_producto = new SelectList(db.tbl_productos.Where(model => model.estado != 3), "id_producto", "nombre");
            return View();
        }

        public ActionResult VGuardarListaPrecioDet(int id)
        {
            tbl_listaprecio tlp = db.tbl_listaprecio.Find(id);
            if (tlp == null)
            {
                return HttpNotFound();
            }
            else
            {
                ViewBag.id_listaprecio = new SelectList(db.tbl_listaprecio.Where(model => model.estado != 3), "id_listaprecio", "nombre");
                ViewBag.id_producto = new SelectList(db.tbl_productos.Where(model => model.estado != 3), "id_producto", "nombre");
                return View(tlp);
            }
        }
        public ActionResult EditarListaPrecio(int id)
        {
            tbl_listaprecio tl = db.tbl_listaprecio.Find(id);
            if(tl == null)
            {
                return HttpNotFound();
            }
            else
            {
                ViewBag.id_kermesse = new SelectList(db.tbl_kermesse.Where(model => model.estado != 3), "id_kermesse", "nombre");
                return View(tl);
            }
        }

        public ActionResult EditarListaPrecioDet (int id)
        {
            tbl_listaprecio_det tldet = db.tbl_listaprecio_det.Find(id);
            if(tldet == null)
            {
                return HttpNotFound();
            }
            else
            {
                ViewBag.id_lista_precio = new SelectList(db.tbl_listaprecio.Where(model => model.estado != 3), "id_listaprecio", "nombre");
                ViewBag.id_producto = new SelectList(db.tbl_productos.Where(model => model.estado != 3), "id_producto", "nombre");
                return View(tldet);
            }
        }
        [HttpPost]
        public ActionResult GuardarListaPrecio(tbl_listaprecio tl)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    tbl_listaprecio tlp = new tbl_listaprecio();

                    tlp.id_kermesse = tl.id_kermesse;
                    tlp.nombre = tl.nombre;
                    tlp.descripcion = tl.descripcion;
                    tlp.estado = 1;

                    db.tbl_listaprecio.Add(tlp);
                    db.SaveChanges();

                    foreach (var tlpd in tl.tbl_listaprecio_det)
                    {
                        tbl_listaprecio_det tblpdet = new tbl_listaprecio_det();

                        tblpdet.id_listaprecio = tlp.id_listaprecio;
                        tblpdet.id_producto = tlpd.id_producto;
                        tblpdet.precio_venta = tlpd.precio_venta;
                        
                        db.tbl_listaprecio_det.Add(tblpdet);
                    }
                    db.SaveChanges();
                }
                ModelState.Clear();
                return RedirectToAction("ListListaPrecio");
            }
            catch (Exception e)
            {
                return View("VGuardarListaPrecio");
            }
        }

        [HttpPost]
        public ActionResult GuardarListaPrecioDet(tbl_listaprecio tlp)
        {
            int idListaPrecio = tlp.id_listaprecio;

            try
            {
                tbl_listaprecio tlpr = new tbl_listaprecio();

                tlpr.id_listaprecio = tlp.id_listaprecio;

                foreach (var tlpd in tlp.tbl_listaprecio_det)
                {
                    tbl_listaprecio_det tlpre = new tbl_listaprecio_det();
                    tlpre.id_listaprecio = tlpr.id_listaprecio;
                    tlpre.id_producto = tlpd.id_producto;
                    tlpre.precio_venta = tlpd.precio_venta;

                    db.tbl_listaprecio_det.Add(tlpre);
                }
                db.SaveChanges();
                ModelState.Clear();
                return RedirectToAction("ListListaPrecio", "Tbl_listaprecio", new { id = idListaPrecio });
            }
            catch (Exception e)
            {
                return RedirectToAction("ListListaPrecio", "Tbl_listaprecio", new { id = idListaPrecio });
                throw;
            }
        }


        // Eliminar
        public ActionResult EliminarListaPrecio(int id)
        {
            tbl_listaprecio tlp = new tbl_listaprecio();

            tlp = db.tbl_listaprecio.Find(id);
            this.LogicalDeleteLista(tlp);
            return RedirectToAction("ListListaPrecio");
        }

        public ActionResult EliminarListaPrecioDet(int id)
        {
            tbl_listaprecio_det tlpdet = new tbl_listaprecio_det();

            tlpdet = db.tbl_listaprecio_det.Find(id);
            int idlistaprecio = tlpdet.id_listaprecio;

            db.tbl_listaprecio_det.Remove(tlpdet);
            db.SaveChanges();

            var list = db.tbl_listaprecio_det.Where(model => model.id_listaprecio == idlistaprecio);
            return View("ListListaPrecioDet", list);
        }

        [HttpPost]
        public ActionResult LogicalDeleteLista(tbl_listaprecio tlp)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    tlp.estado = 3;
                    db.Entry(tlp).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("ListListaPrecio");
            }
            catch (Exception)
            {
                return View();
                throw;
            }
        }


        // Editar
        [HttpPost]
        public ActionResult ActualizarListaPrecio(tbl_listaprecio tlp)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    tlp.estado = 2;
                    db.Entry(tlp).State = EntityState.Modified;
                    db.SaveChanges();
                }
                ViewBag.id_kermesse = new SelectList(db.tbl_kermesse.Where(model => model.estado != 3), "id_kermesse", "nombre");
                return RedirectToAction("ListListaPrecio");
            }
            catch (Exception)
            {
                return View();
                throw;
            }
        }

        [HttpPost]
        public ActionResult ActualizarListaPrecioDet(tbl_listaprecio_det tlpdet)
        {
            int idLista = tlpdet.id_listaprecio;
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(tlpdet).State = EntityState.Modified;
                    db.SaveChanges();
                }
                ViewBag.id_moneda = new SelectList(db.tbl_listaprecio.Where(model => model.estado != 3), "id_listaprecio", "nombre");
                ViewBag.id_producto = new SelectList(db.tbl_productos.Where(model => model.estado != 3), "id_producto", "nombre");
                return RedirectToAction("ListListaPrecio", "Tbl_listaprecio", new { id = idLista });
            }
            catch (Exception)
            {
                return RedirectToAction("ListListaPrecio", "Tbl_listaprecio", new { id = idLista });
                throw;
            }
        }

        // Filtros

        [HttpPost]
        public ActionResult FiltrarListaPrecio(String cadena)
        {
            if (String.IsNullOrEmpty(cadena))
            {
                var list = db.tbl_listaprecio.Where(x => x.estado != 3);
                return View("ListListaPrecio", list);
            }
            else
            {
                var listFiltrada = db.tbl_listaprecio.Where(x => x.nombre.Contains(cadena)
                || x.descripcion.ToString().Contains(cadena));
                var newList = listFiltrada.Where(x => x.estado != 3);
                return View("ListListaPrecio", newList);
            }
        }

        [HttpPost]
        public ActionResult FiltrarListaPrecioDet(String cadena, int id)
        {
            if (String.IsNullOrEmpty(cadena))
            {
                var list = db.tbl_listaprecio_det.Where(model => model.id_listaprecio == id);
                return View("ListListaPrecioDet", list);
            }
            else
            {
                var listFiltrada = db.tbl_listaprecio_det.Where(x => x.id_listaprecio == id &&
                (x.precio_venta.ToString().Contains(cadena)));
                return View("ListListaPrecioDet", listFiltrada);
            }
        }



        
        public ActionResult VerRptListaPrecio(tbl_listaprecio_det tlp)
        {
            LocalReport rpt = new LocalReport();
            string mt, enc, f, tipo;
            string[] s;
            Warning[] w;

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "rptFiltraListaPrecio.rdlc");
            rpt.ReportPath = ruta;

           
            var listaFiltrada = db.vw_listaprecio.Where(x => x.id_listaprecio == tlp.id_listaprecio && x.id_producto == tlp.id_producto);

            ReportDataSource rd = new ReportDataSource("dsFiltraListaPrecio", listaFiltrada);
            rpt.DataSources.Add(rd);
            tipo = "PDF";
            var b = rpt.Render(tipo, null, out mt, out enc, out f, out s, out w);
            return new FileContentResult(b, mt);
        }

    }
}
