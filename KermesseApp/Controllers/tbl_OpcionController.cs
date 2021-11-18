using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KermesseApp.Models;

namespace KermesseApp.Controllers
{
    public class tbl_OpcionController : Controller
    {
        private KERMESSEEntities db = new KERMESSEEntities();

        //Metodo para generar la lista de opciones
        public ActionResult listar_Opcion()
        {
            return View(db.tbl_opciones.ToList());
        }

        //Metodo para listar una opcion especifica
        public ActionResult ver_Opcion(int id)
        {
            var list = db.tbl_opciones.Where(x => x.id_opciones == id).First();

            return View(list);
        }

        //Metodo para generar la vista que registra nueva opcion
        public ActionResult guardar_Opcion()
        {
            return View();
        }

        //Metodo para guardar la nueva opcion
        [HttpPost]
        public ActionResult guardar_Opcion(tbl_opciones to)
        {

            if (ModelState.IsValid)
            {
                tbl_opciones tOp = new tbl_opciones();

                tOp.opcion = to.opcion;
                tOp.estado = 1;

                db.tbl_opciones.Add(tOp);
                db.SaveChanges();
            }

            ModelState.Clear();
            return RedirectToAction("listar_Opcion");
        }

        //Metodo para editar una opcion (Obtener el ID)
        public ActionResult editar_Opcion(int id)
        {
            //Ambos es lo mismo...

            tbl_opciones to = db.tbl_opciones.Find(id);

            var add = db.tbl_opciones.Where(x => x.id_opciones == id).First();

            return View(to);
        }

        //Metodo para actualizar una opcion (Proceso de guardado)
        [HttpPost]
        public ActionResult actualizar_Opcion(tbl_opciones to)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    to.estado = 2;
                    db.Entry(to).State = EntityState.Modified;
                    db.SaveChanges();
                }

                return RedirectToAction("listar_Opcion");
            }
            catch
            {
                return View();
            }
        }

        //Metodo para eliminar opcion
        public ActionResult eliminar_Opcion(int id)
        {
            tbl_opciones to = new tbl_opciones();

            //Ambos metodos son para eliminar...

            //o = db.tbl_opciones.Find(id);
            //db.tbl_opciones.Remove(to);

            var del = db.tbl_opciones.Where(x => x.id_opciones == id).First();
            db.tbl_opciones.Remove(del);

            db.SaveChanges();

            var list = db.tbl_opciones.ToList();
            return View("listar_Opcion", list);
        }

        //Metodo para filtrar
        [HttpPost]
        public ActionResult filtrar_Opcion(String Cadena)
        {
            if (String.IsNullOrEmpty(Cadena))
            {
                var list = db.tbl_opciones.ToList();
                return View("listar_Opcion", list);
            }
            else
            {
                var list_encontrada = db.tbl_opciones.Where(x => x.opcion.Contains(Cadena));
                return View("listar_Opcion", list_encontrada);
            }
        }
    }
}