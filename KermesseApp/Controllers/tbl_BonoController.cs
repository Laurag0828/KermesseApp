using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KermesseApp.Models;

namespace KermesseApp.Controllers
{
    public class tbl_BonoController : Controller
    {
        private KERMESSEEntities db = new KERMESSEEntities();
        
        //Metodo para generar la lista de bonos
        public ActionResult listar_Bono()
        {
            return View(db.tbl_bonos.ToList());
        }

        //Metodo para listar un bono especifico
        public ActionResult ver_Bono(int id)
        {
            var list = db.tbl_bonos.Where(x => x.id_bono == id).First();

            return View(list);
        }

        //Metodo para generar la vista que registra nuevo bono
        public ActionResult guardar_Bono()
        {
            return View();
        }

        //Metodo para guardar nuevo bono
        [HttpPost]
        public ActionResult guardar_Bono(tbl_bonos tb)
        {

            if (ModelState.IsValid)
            {
                tbl_bonos tBon = new tbl_bonos();

                tBon.nombre = tb.nombre;
                tBon.valor = tb.valor;
                tBon.estado = 1;

                db.tbl_bonos.Add(tBon);
                db.SaveChanges();
            }

            ModelState.Clear();
            return RedirectToAction("listar_Bono");
        }

        //Metodo para editar un bono (Obtener el ID)
        public ActionResult editar_Bono(int id)
        {
            //Ambos es lo mismo...

            tbl_bonos tb = db.tbl_bonos.Find(id);

            var add = db.tbl_bonos.Where(x => x.id_bono == id).First();

            return View(tb);
        }

        //Metodo para actualizar un bono (Proceso de guardado)
        [HttpPost]
        public ActionResult actualizar_Bono(tbl_bonos tb)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    tb.estado = 2;
                    db.Entry(tb).State = EntityState.Modified;
                    db.SaveChanges();
                }

                return RedirectToAction("listar_Bono");
            }
            catch
            {
                return View();
            }

        }

        //Metodo para eliminar bono
        public ActionResult eliminar_Bono(int id)
        {
            tbl_bonos tb = new tbl_bonos();

            //Ambos metodos son para eliminar...

            //tb = db.tbl_bonos.Find(id);
            //db.tbl_bonos.Remove(tb);

            var del = db.tbl_bonos.Where(x => x.id_bono == id).First();
            db.tbl_bonos.Remove(del);

            db.SaveChanges();

            var list = db.tbl_bonos.ToList();
            return View("listar_Bono", list);
        }

        //Metodo para filtrar
        [HttpPost]
        public ActionResult filtrar_Bono(String Cadena)
        {
            if (String.IsNullOrEmpty(Cadena))
            {
                var list = db.tbl_bonos.ToList();
                return View("listar_Bono", list);
            }
            else
            {
                var list_encontrada = db.tbl_bonos.Where(x => x.nombre.Contains(Cadena));
                return View("listar_Bono", list_encontrada);
            }
        }
    }
}