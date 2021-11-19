using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KermesseApp.Models;

namespace KermesseApp.Models
{
    public class tbl_ComunidadController : Controller
    {
        private KERMESSEEntities db = new KERMESSEEntities();

        //Metodo para generar la lista de comunidades
        public ActionResult listar_Comunidad()
        {
            return View(db.tbl_comunidad.Where(model => model.estado != 3));
        }

        //Metodo para listar una comunidad especifica
        public ActionResult ver_Comunidad(int id)
        {
            var list = db.tbl_comunidad.Where(x => x.id_comunidad == id).First();
            
            return View(list);
        }

        //Metodo para generar la vista que registra nueva comunidad
        public ActionResult guardar_Comunidad()
        {
            return View();
        }

        //Metodo para guardar la nueva comunidad
        [HttpPost]
        public ActionResult guardar_Comunidad(tbl_comunidad tc)
        {
            if (ModelState.IsValid)
            {
                tbl_comunidad tCom = new tbl_comunidad();

                tCom.nombre = tc.nombre;
                tCom.responsable = tc.responsable;
                tCom.desc_contribucion = tc.desc_contribucion;
                tCom.estado = 1;

                db.tbl_comunidad.Add(tCom);
                db.SaveChanges();
            }
            ModelState.Clear();

            return RedirectToAction("listar_Comunidad");
        }

        //Metodo para editar una comunidad (Obtener el ID)
        public ActionResult editar_Comunidad(int id)
        {
            //Ambos es lo mismo...

            tbl_comunidad tc = db.tbl_comunidad.Find(id);

            var add = db.tbl_comunidad.Where(x => x.id_comunidad == id).First();

            return View(db.tbl_comunidad.Where(model => model.estado != 3));
        }

        //Metodo para actualizar una comunidad (Proceso de guardado)
        [HttpPost]
        public ActionResult actualizar_Comunidad(tbl_comunidad tc)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    tc.estado = 2;
                    db.Entry(tc).State = EntityState.Modified;
                    db.SaveChanges();
                }

                return RedirectToAction("listar_Comunidad");
            }
            catch
            {
                return View();
            }
        }

        //Metodo para eliminar una comunidad
        public ActionResult eliminar_Comunidad(int id)
        {
            tbl_comunidad tc = db.tbl_comunidad.Find(id);
            var del = db.tbl_comunidad.Where(x => x.id_comunidad == id).First();

            eliminar_Logico(del);

            return RedirectToAction("listar_Comunidad");


            //Ambos son para eliminar...

            //tc = db.tbl_comunidad.Find(id);
            //db.tbl_comunidad.Remove(tc);


            //var del = db.tbl_comunidad.Where(x => x.id_comunidad == id).First();
            //db.tbl_comunidad.Remove(del);

            //db.SaveChanges();

            //var list = db.tbl_comunidad.ToList();
            //return View("listar_Comunidad", list);

        }

        //Metodo para eliminar una comunidad de manera logica
        public ActionResult eliminar_Logico(tbl_comunidad tc)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    tc.estado = 3;
                    db.Entry(tc).State = EntityState.Modified;
                    db.SaveChanges();
                }

                return RedirectToAction("listar_Comunidad");
            }
            catch
            {
                return View();
            }
        }

        //Metodo para filtrar 
        [HttpPost]
        public ActionResult filtrar_Comunidad(string Cadena)
        {
            if (String.IsNullOrEmpty(Cadena))
            {
                var list = db.tbl_comunidad.Where(model => model.estado != 3);
                return View("listar_Comunidad", list);
            }
            else
            {
                var list_encontrada = db.tbl_comunidad.Where(x => (x.nombre.Contains(Cadena) ||
                                       x.responsable.Contains(Cadena) || x.desc_contribucion.Contains(Cadena)) && x.estado != 3);

                return View("listar_Comunidad", list_encontrada);
            }
        }
    }
}