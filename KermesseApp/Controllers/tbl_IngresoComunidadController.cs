using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KermesseApp.Models;

namespace KermesseApp.Controllers
{
    public class tbl_IngresoComunidadController : Controller
    {
        private KERMESSEEntities db = new KERMESSEEntities();

        //Metodo para generar la lista de IngresoComunidad
        public ActionResult listar_IngresoComunidad()
        {
            return View(db.vw_ingresocomunidad.Where(model => model.estado != 3));
        }

        //Metodo para listar IngresoComunidad especifico
        public ActionResult ver_IngresoComunidad(int id)
        {
            var list = db.vw_ingresocomunidad.Where(x => x.id_ingresocom == id).First();

            return View(list);
        }

        //Metodo para generar la vista que registra nuevo ingreso comunidad
        public ActionResult guardar_IngresoComunidad()
        {

            ViewBag.id_kermesse = new SelectList(db.tbl_kermesse, "id_kermesse", "nombre");
            ViewBag.id_comunidad = new SelectList(db.tbl_comunidad, "id_comunidad", "nombre");
            ViewBag.id_producto = new SelectList(db.tbl_productos, "id_producto", "nombre");

            ViewBag.id_bono = new SelectList(db.tbl_bonos, "id_bono", "nombre");

            return View();
        }

        //Metodo para guardar nuevo ingreso comunidad
        [HttpPost]
        public ActionResult guardar_IngresoComunidada(int kermesse, int comunidad, int producto, int cantidadp, int preciop, string total, tbl_ingresocom_det[] detalle)
        {

            ViewBag.id_kermesse = new SelectList(db.tbl_kermesse, "id_kermesse", "nombre");
            ViewBag.id_comunidad = new SelectList(db.tbl_comunidad, "id_comunidad", "nombre");
            ViewBag.id_producto = new SelectList(db.tbl_productos, "id_producto", "nombre");

            ViewBag.id_bono = new SelectList(db.tbl_bonos, "id_bono", "nombre");

            string result = "Error! Order Is Not Complete!";
            tbl_ingreso_com ingreso = new tbl_ingreso_com();
            ingreso.id_kermesse = kermesse;
            ingreso.id_comunidad = comunidad;
            ingreso.id_producto = producto;
            ingreso.cant_producto = cantidadp;
            ingreso.precio_producto = preciop;
            ingreso.total_bonos = int.Parse(total);
            ingreso.fecha_creacion = DateTime.Now;
            ingreso.estado = 1;
            db.tbl_ingreso_com.Add(ingreso);

            foreach (var item in detalle)
            {
                tbl_ingresocom_det det = new tbl_ingresocom_det();
                det.id_ingresocom = ingreso.id_ingresocom;
                det.id_bono = item.id_bono;
                det.denominacion = item.denominacion;
                det.cantidad = item.cantidad;
                det.subtotal = item.subtotal;
                db.tbl_ingresocom_det.Add(det);
            }

            db.SaveChanges();
            result = "Success! Order Is Complete!";
            return Json(result, JsonRequestBehavior.AllowGet);


            //if (ModelState.IsValid)
            //{
            //    tbl_ingreso_com tICom = new tbl_ingreso_com();

            //    tICom.cant_producto = tic.cant_producto;
            //    tICom.precio_producto = tic.precio_producto;
            //    tICom.total_bonos = tic.total_bonos;
            //    tICom.usuario_creacion = 1;
            //    tICom.fecha_creacion = DateTime.Now;
            //    tICom.estado = 1;


            //    db.tbl_ingreso_com.Add(tICom);

            //    db.SaveChanges();

            //    ModelState.Clear();
            //}

            //ViewBag.id_kermesse = new SelectList(db.tbl_kermesse, "id_kermesse", "nombre");
            //ViewBag.id_comunidad = new SelectList(db.tbl_comunidad, "id_comunidad", "nombre");
            //ViewBag.id_producto = new SelectList(db.tbl_productos, "id_producto", "nombre");

            //return View("guardar_IngresoComunidad");


        }

        public ActionResult eliminar_IngresoComunidad(int id)
        {
            tbl_ingreso_com topc = new tbl_ingreso_com();
            topc = db.tbl_ingreso_com.Find(id);
            topc.estado = 3;
            db.Entry(topc).State = EntityState.Modified;
            db.SaveChanges();
            var list = db.tbl_ingreso_com.ToList();

            return RedirectToAction("listar_IngresoComunidad");
        }
    }
}