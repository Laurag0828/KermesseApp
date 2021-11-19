using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KermesseApp.Models;

namespace KermesseApp.Controllers
{
    public class tbl_ParroquiaController : Controller
    {
        private KERMESSEEntities db = new KERMESSEEntities();
        
        //Metodo para generar la lista de parroquias
        public ActionResult listar_Parroquia()
        {
            return View(db.tbl_parroquia.Where(model => model.estado != 3));
        }

        //Metodo para listar una parroquia especifica
        public ActionResult ver_Parroquia(int id)
        {
            var list = db.tbl_parroquia.Where(x => x.idparroquia == id).First();

            return View(list);
        }

        //Metodo para generar la vista que registra nueva parroquia
        public ActionResult guardar_Parroquia()
        {
            return View();
        }

        //Metodo para guardar la nueva parroquia
        [HttpPost]
        public ActionResult guardar_Parroquia(tbl_parroquia tp)
        {
            if (ModelState.IsValid)
            {
                tbl_parroquia tParr = new tbl_parroquia();

                tParr.nombre = tp.nombre;
                tParr.direccion = tp.direccion;
                tParr.telefono = tp.telefono;
                tParr.parroco = tp.parroco;
                tParr.url_logo = tp.url_logo;
                tParr.sitio_web = tp.sitio_web;
                tParr.estado = 1;

                db.tbl_parroquia.Add(tParr);
                db.SaveChanges();
            }

            ModelState.Clear();
            return RedirectToAction("listar_Parroquia");
        }

        //Metodo para editar una parroquia (Obtener el ID)
        public ActionResult editar_Parroquia(int id)
        {
            //Ambos es lo mismo...

            tbl_parroquia tp = db.tbl_parroquia.Find(id);

            var add = db.tbl_parroquia.Where(x => x.idparroquia == id).First();

            return View(add);
        }

        //Metodo para actualizar una parroquia (Proceso de guardado)
        [HttpPost]
        public ActionResult actualizar_Parroquia(tbl_parroquia tp)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    tp.estado = 2;
                    db.Entry(tp).State = EntityState.Modified;
                    db.SaveChanges();
                }

                return RedirectToAction("listar_Parroquia");
            }
            catch
            {
                return View();
            }
        }

        //Metodo para eliminar una parroquia
        public ActionResult eliminar_Parroquia(int id)
        {
            tbl_parroquia tp = new tbl_parroquia();

            var del = db.tbl_parroquia.Where(x => x.idparroquia == id).First();

            eliminar_Logico(del);

            return RedirectToAction("listar_Parroquia");


            //Ambos metodos son para eliminar...

            //tp = db.tbl_parroquia.Find(id);
            //db.tbl_parroquia.Remove(tp);


            //var del = db.tbl_parroquia.Where(x => x.idparroquia == id).First();
            //db.tbl_parroquia.Remove(del);

            //db.SaveChanges();

            //var list = db.tbl_parroquia.ToList();
            //return View("listar_Parroquia", list);
        }

        //Metodo para eliminar una parroquia de manera logica
        public ActionResult eliminar_Logico(tbl_parroquia tp)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    tp.estado = 3;
                    db.Entry(tp).State = EntityState.Modified;
                    db.SaveChanges();
                }

                return RedirectToAction("listar_Parroquia");
            }
            catch
            {
                return View();
            }
        }

        //Metodo para filtrar 
        [HttpPost]
        public ActionResult filtrar_Parroquia(String Cadena)
        {
            if (String.IsNullOrEmpty(Cadena))
            {
                var list = db.tbl_parroquia.Where(model => model.estado != 3);
                return View("listar_Parroquia", list);
            }
            else
            {
                var list_encontrada = db.tbl_parroquia.Where(x => (x.nombre.Contains(Cadena) ||
                                       x.direccion.Contains(Cadena) || x.telefono.Contains(Cadena) ||
                                       x.parroco.Contains(Cadena) || x.sitio_web.Contains(Cadena)) && x.estado != 3);

                return View("listar_Parroquia", list_encontrada);
            }
        }
    }
}