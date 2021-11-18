using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KermesseApp.Models;

namespace KermesseApp.Controllers
{
    public class tbl_ProductoController : Controller
    {
        private KERMESSEEntities db = new KERMESSEEntities();

        //Metodo para generar la lista de productos
        public ActionResult listar_Producto()
        {
            return View(db.vw_productos.ToList());
        }

        //Metodo para listar un producto especifico
        public ActionResult ver_Producto(int id)
        {
            var list = db.vw_productos.Where(x => x.id_producto == id).First();
            
            return View(list);
        }

        //Metodo para generar la vista que registra nuevo producto
        public ActionResult guardar_Producto()
        {

            ViewBag.id_cat_producto = new SelectList(db.tbl_cat_producto, "id_cat_producto", "nombre");
            ViewBag.id_comunidad = new SelectList(db.tbl_comunidad, "id_comunidad", "nombre");

            return View();
        }

        //Metodo para guardar nuevo producto
        [HttpPost]
        public ActionResult guardar_Producto(tbl_productos tp)
        {
            if (ModelState.IsValid)
            {
                tbl_productos tProd = new tbl_productos();

                tProd.nombre = tp.nombre;
                tProd.desc_presentacion = tp.desc_presentacion;
                tProd.cantidad = tp.cantidad;
                tProd.precio_venta = tp.precio_venta;
                tProd.id_cat_producto = tp.id_cat_producto;
                tProd.id_comunidad = tp.id_comunidad;
                tProd.estado = 1;

                db.tbl_productos.Add(tProd);
                db.SaveChanges();

                ModelState.Clear();
                return RedirectToAction("listar_Producto");
            }

            ViewBag.id_cat_producto = new SelectList(db.tbl_cat_producto, "id_cat_producto", "nombre");
            ViewBag.id_comunidad = new SelectList(db.tbl_comunidad, "id_comunidad", "nombre");

            return View("guardar_Producto");
        }

        //Metodo para editar un producto (Obtener el ID)
        public ActionResult editar_Producto(int id)
        {
            tbl_productos tp = db.tbl_productos.Find(id);

            ViewBag.id_cat_producto = new SelectList(db.tbl_cat_producto, "id_cat_producto", "nombre");
            ViewBag.id_comunidad = new SelectList(db.tbl_comunidad, "id_comunidad", "nombre");

            return View(tp);
        }

        //Metodo para actualizar un producto
        [HttpPost]
        public ActionResult actualizar_Producto(tbl_productos tp)
        {
            try 
            {
                if (ModelState.IsValid)
                {
                    tp.estado = 2;
                    db.Entry(tp).State = EntityState.Modified;
                    db.SaveChanges();

                }

                return RedirectToAction("listar_Producto");

            }
            catch
            {
                return View();
            }

        }

        //Metodo para eliminar producto
        public ActionResult eliminar_Producto(int id)
        {
            tbl_productos tp = new tbl_productos();

            //Ambos metodos son para eliminar...

            //tp = db.tbl_productos.Find(id);
            //db.tbl_productos.Remove(tp);

            var del = db.tbl_productos.Where(x => x.id_producto == id).First();
            db.tbl_productos.Remove(del);

            db.SaveChanges();

            var list = db.tbl_productos.ToList();
            return View("listar_Producto", list);
        }

        //Metodo para filtrar
        [HttpPost]
        public ActionResult filtrar_Producto(String Cadena)
        {
            if (String.IsNullOrEmpty(Cadena))
            {
                var list = db.vw_productos.ToList();
                return View("listar_Producto", list);
            }
            else
            {
                var list_encontrada = db.vw_productos.Where(x => x.nombre.Contains(Cadena) ||
                                       x.desc_presentacion.Contains(Cadena) || x.comunidad.Contains(Cadena) ||
                                       x.catprod.Contains(Cadena));

                return View("listar_Producto", list_encontrada);
            }
        }
    }
}