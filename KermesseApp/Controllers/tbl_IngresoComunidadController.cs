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
            return View(db.tbl_ingreso_com.Where(model => model.estado != 3));
        }

        public ActionResult actualizar_IngresoComunidad()
        {
            ViewBag.id_kermesse = new SelectList(db.tbl_kermesse, "id_kermesse", "nombre");
            ViewBag.id_comunidad = new SelectList(db.tbl_comunidad, "id_comunidad", "nombre");
            ViewBag.id_producto = new SelectList(db.tbl_productos, "id_producto", "nombre");

            return View();
        }

        public ActionResult editar_IngresoComunidad(int id)
        {

            ViewBag.id_kermesse = new SelectList(db.tbl_kermesse.Where(model => model.estado != 3), "id_kermesse", "nombre");
            ViewBag.id_comunidad = new SelectList(db.tbl_comunidad.Where(model => model.estado != 3), "id_comunidad", "nombre");
            ViewBag.id_producto = new SelectList(db.tbl_productos.Where(model => model.estado != 3), "id_producto", "nombre");

            tbl_ingreso_com tic = db.tbl_ingreso_com.Find(id);

            if (tic == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(tic);
            }

        }

        [HttpPost]
        public ActionResult update_IngresoComunidad(tbl_ingreso_com tic, tbl_ingresocom_det ticd) //Actualizamos el campo estado
        {

            ViewBag.id_kermesse = new SelectList(db.tbl_kermesse.Where(model => model.estado != 3), "id_kermesse", "nombre");
            ViewBag.comunidad = new SelectList(db.tbl_comunidad.Where(model => model.estado != 3), "id_comunidad", "nombre");
            ViewBag.producto = new SelectList(db.tbl_productos.Where(model => model.estado != 3), "id_producto", "nombre");

            var det = db.tbl_ingresocom_det.Where(Model => Model.id_ingresocom == tic.id_ingresocom);
            tic.total_bonos = 0;

            foreach (var item in det)
            {
                tic.total_bonos = (tic.total_bonos + item.subtotal);
            }

            tic.estado = 2;
            db.Entry(tic).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("listar_IngresoComunidad");

        }

        //Metodo para listar IngresoComunidad especifico
        public ActionResult ver_IngresoComunidad(int id)
        {
            var list = db.tbl_ingreso_com.Where(x => x.id_ingresocom == id).First();

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



        // -- METODOS PARA EL DETALLE DE INGRESO COMUNIDAD


        public ActionResult ver_IngresoComDet(int id)  //Metodo para visualizar
        {

            var det = db.vw_ingresocomunidad.Where(x => x.id_ingresocom == id);

            return View(det);
        }

        public ActionResult actualizar_IngresoComDet()
        {

            ViewBag.id_bono = new SelectList(db.tbl_bonos, "id_bono", "nombre");
            ViewBag.id_ingresocom = new SelectList(db.tbl_ingreso_com, "id_ingresocom", "id_ingresocom");

            return View();
        }

        public ActionResult editar_IngresoComDet(int id)
        {

            ViewBag.id_bono = new SelectList(db.tbl_bonos.Where(model => model.estado != 3), "id_bono", "nombre");
            ViewBag.id_ingresocom = new SelectList(db.tbl_ingreso_com.Where(model => model.estado != 3), "id_ingresocom", "id_ingresocom");

            tbl_ingresocom_det ticd = db.tbl_ingresocom_det.Find(id);

            if (ticd == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(ticd);
            }

        }

        [HttpPost]
        public ActionResult update_IngresoComDet(tbl_ingresocom_det ticd, tbl_bonos tb) //Actualizamos el campo estado
        {

            ViewBag.id_bono = new SelectList(db.tbl_bonos.Where(model => model.estado != 3), "id_bono", "nombre");
            ViewBag.id_ingresocom = new SelectList(db.tbl_ingreso_com.Where(model => model.estado != 3), "id_ingresocom", "id_ingresocom");

            var det = db.tbl_bonos.Where(Model => Model.id_bono == ticd.id_bono);
            ticd.subtotal = 0;

            foreach (var item in det)
            {
                ticd.subtotal = item.valor * ticd.cantidad;
            }

            ticd.estado = 2;
            db.Entry(ticd).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("ver_IngresoComDet", new { id = ticd.id_ingresocom });

        }

        public ActionResult ver_Detalle(int id)  //Metodo para visualizar
        {

            var gast = db.tbl_ingresocom_det.Where(x => x.id_ingresocom_det == id).First();

            return View(gast);
        }

        public ActionResult eliminar_Detalle(int id) //Metodo para eliminar
        {

            tbl_ingresocom_det ticd = new tbl_ingresocom_det();
            ticd = db.tbl_ingresocom_det.Find(id);
            db.tbl_ingresocom_det.Remove(ticd);
            db.SaveChanges();

            var list = db.tbl_ingresocom_det.ToList();

            return RedirectToAction("ver_IngresoComDet", new { id = ticd.id_ingresocom, list });
            //return RedirectToAction("VerArqueo", list);
        }

    }
}