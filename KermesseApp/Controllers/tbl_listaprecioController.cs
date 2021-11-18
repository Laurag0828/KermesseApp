using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KermesseApp.Models;

namespace KermesseApp.Controllers
{
    public class tbl_listaprecioController : Controller
    {
        private KERMESSEEntities db = new KERMESSEEntities();
        // GET: Tbl_listaprecio
        public ActionResult ListListaPrecio()
        {
            return View(db.tbl_listaprecio.ToList());
        }
        public ActionResult ListListaPrecioDet()
        {
            return View(db.tbl_listaprecio_det.ToList());
        }
        public ActionResult VGuardarListaPrecio(tbl_listaprecio tl)
        {
            if (ModelState.IsValid)
            {
                tbl_listaprecio topc = new tbl_listaprecio();
                ViewBag.id_kermesse = new SelectList(db.tbl_kermesse.Where(model => model.estado != 3), "id_kermesse", "nombre");
                topc.nombre = tl.nombre;
                topc.descripcion = tl.descripcion;
                topc.estado = 1;
                db.tbl_listaprecio.Add(topc);
                db.SaveChanges();
            }
            ModelState.Clear();
            return View("VGuardarListaPrecio");
        
         
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
                ViewBag.id_producto = new SelectList(db.tbl_productos.Where(model => model.estado != 3), "id_productos", "nombre");
                return View(tldet);
            }
        }
    }
}
