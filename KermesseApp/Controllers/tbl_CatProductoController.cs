using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KermesseApp.Models;
using Microsoft.Reporting.WebForms;

namespace KermesseApp.Controllers
{
    public class tbl_CatProductoController : Controller
    {
        private KERMESSEEntities db = new KERMESSEEntities();

        //Metodo para generar la lista de categoria de productos
        public ActionResult listar_CatProducto()
        {
            return View(db.tbl_cat_producto.Where(model => model.estado != 3));
        }

        //Metodo para listar una categoria de producto especifica
        public ActionResult ver_CatProducto(int id)
        {
            var list = db.tbl_cat_producto.Where(x => x.id_cat_producto == id).First();
           
            return View(list);
        }

        //Metodo para generar la vista que registra nueva categoria de producto
        public ActionResult guardar_CatProducto()
        {
            return View();
        }

        //Metodo para guardar la nueva categoria de producto
        [HttpPost]
        public ActionResult guardar_CatProducto(tbl_cat_producto tcp)
        {

            if (ModelState.IsValid)
            {
                tbl_cat_producto tcProd = new tbl_cat_producto();

                tcProd.nombre = tcp.nombre;
                tcProd.descripcion = tcp.descripcion;
                tcProd.estado = 1;

                db.tbl_cat_producto.Add(tcProd);
                db.SaveChanges();
            }

            ModelState.Clear();
            return RedirectToAction("listar_CatProducto");
        }

        //Metodo para editar una categoria de producto (Obtener el ID)
        public ActionResult editar_CatProducto(int id)
        {
            //Ambos es lo mismo...

            tbl_cat_producto tcp = db.tbl_cat_producto.Find(id);

            var add = db.tbl_cat_producto.Where(x => x.id_cat_producto == id).First();

            return View(tcp);
        }

        //Metodo para actualizar una categoria de producto (Proceso de guardado)
        [HttpPost]
        public ActionResult actualizar_CatProducto(tbl_cat_producto tcp)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    tcp.estado = 2;
                    db.Entry(tcp).State = EntityState.Modified;
                    db.SaveChanges();
                }

                return RedirectToAction("listar_CatProducto");
            }
            catch 
            {
                return View();
            }
        }

        //Metodo para eliminar categoria de producto 
        public ActionResult eliminar_CatProducto(int id)
        {
            tbl_cat_producto tcp = new tbl_cat_producto();

            var del = db.tbl_cat_producto.Where(x => x.id_cat_producto == id).First();

            eliminar_Logico(del);

            return RedirectToAction("listar_CatProducto");


            //Ambos metodos son para eliminar...

            //tcp = db.tbl_cat_producto.Find(id);
            //db.tbl_cat_producto.Remove(tcp);


            //var del = db.tbl_cat_producto.Where(x => x.id_cat_producto == id).First();
            //db.tbl_cat_producto.Remove(del);

            //db.SaveChanges();

            //var list = db.tbl_cat_producto.ToList();
            //return View("listar_CatProducto", list);
        }

        //Metodo para eliminar una parroquia de manera logica
        public ActionResult eliminar_Logico(tbl_cat_producto tcp)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    tcp.estado = 3;
                    db.Entry(tcp).State = EntityState.Modified;
                    db.SaveChanges();
                }

                return RedirectToAction("listar_CatProducto");
            }
            catch
            {
                return View();
            }
        }

        //Metodo para filtrar
        [HttpPost]
        public ActionResult filtrar_CatProducto(String Cadena)
        {
            if (String.IsNullOrEmpty(Cadena))
            {
                var list = db.tbl_cat_producto.Where(model => model.estado != 3);
                return View("listar_CatProducto", list);
            }
            else
            {
                var list_encontrada = db.tbl_cat_producto.Where(x => (x.nombre.Contains(Cadena) || x.descripcion.Contains(Cadena)) && x.estado != 3);
                return View("listar_CatProducto", list_encontrada);
            }
        }

        [HttpPost]
        public ActionResult reporteCatProd(string tipo)
        {

            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "rptCatProd.rdlc");
            rpt.ReportPath = ruta;

            var list = db.tbl_cat_producto.Where(model => model.estado != 3);

            ReportDataSource rd = new ReportDataSource("dsCatProd", list);
            rpt.DataSources.Add(rd);

            var b = rpt.Render(tipo, null, out mt, out enc, out f, out s, out w);
            return new FileContentResult(b, mt);

            return View();
        }

        public ActionResult reporteCatProd(string tipo, string cadena)
        {

            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "rptCatProd.rdlc");
            rpt.ReportPath = ruta;

            ReportDataSource rd = null;
            if (string.IsNullOrEmpty(cadena))
            {
                var lista = db.tbl_cat_producto.Where(model => model.estado != 3);
                rd = new ReportDataSource("dsCatProd", lista);
            }
            else
            {
                var lista = db.tbl_cat_producto.Where(x => x.nombre.Contains(cadena) || x.descripcion.Contains(cadena));
                rd = new ReportDataSource("dsCatProd", lista);
            }

            rpt.DataSources.Add(rd);

            var b = rpt.Render(tipo, null, out mt, out enc, out f, out s, out w);
            return new FileContentResult(b, mt);

            return View();
        }
    }
}