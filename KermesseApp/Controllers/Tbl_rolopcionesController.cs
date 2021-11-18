//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using KermesseApp.Models;
//using System.Data.Entity.Validation;
//using System.Data.Entity;
//using Microsoft.Reporting.WebForms;
//using System.IO;


//namespace KermesseApp.Controllers
//{
//    public class Tbl_rolopcionesController : Controller
//    {
//        private KERMESSEEntities db = new KERMESSEEntities();
//        // GET: Tbl_rolopciones
//        public ActionResult ListRolOpciones()
//        {
//            return View(db.tbl_rol_opciones.ToList());
//        }
//        public ActionResult Tbl_rol_opciones()
//        {
//            return View(db.tbl_rol_opciones.ToList());
//        }

//        public ActionResult VistaInsertRolOpciones()
//        {
//            ViewBag.id_rol = new SelectList(db.tbl_rol, "id_rol", "rol_desc");
//            ViewBag.id_opciones = new SelectList(db.tbl_opciones, "id_opciones", "opcion");
//            return View();
//        }
//        [HttpPost]
//        public ActionResult GuardarRolOpciones(tbl_rol_opciones trolop)
//        {
//            if (ModelState.IsValid)
//            {
//                tbl_rol_opciones trolopc = new tbl_rol_opciones();
//                trolopc.id_rol = trolop.id_rol;
//                trolopc.id_opcion = trolop.id_opcion;
//                db.tbl_rol_opciones.Add(trolopc);
//                db.SaveChanges();
//                ModelState.Clear();
//            }



//            ViewBag.id_rol = new SelectList(db.tbl_rol, "id_rol", "rol_desc");
//            ViewBag.id_opciones = new SelectList(db.tbl_opciones, "id_opciones", "opcion");
//            return View("VistaInsertRolOpciones");
//        }

//        public ActionResult DeleteRolOpciones(int id)
//        {
//            tbl_rol_opciones top = new tbl_rol_opciones();

//            //Ambos metodos son para eliminar...

//            //tp = db.tbl_productos.Find(id);
//            //db.tbl_productos.Remove(tp);

//            var del = db.tbl_rol_opciones.Where(x => x.id_rol_opcion == id).FirstOrDefault();
//            db.tbl_rol_opciones.Remove(del);

//            db.SaveChanges();

//            var list = db.tbl_rol_opciones.ToList();
//            return View("ListRolOpciones", list);

//        }


//        public ActionResult VerRolOpciones(int id)
//        {
//            var RolOpciones = db.tbl_rol_opciones.Where(x => x.id_rol_opcion == id).First();
//            return View(RolOpciones);
//        }

//        public ActionResult EditRolOpciones(int id)
//        {

//            {
//                tbl_rol_opciones tru = db.tbl_rol_opciones.Find(id);

//                ViewBag.id_rol = new SelectList(db.tbl_rol, "id_rol", "rol_desc");
//                ViewBag.id_rol_opciones = new SelectList(db.tbl_opciones, "id_opciones", "usuario");

//                return View(tru);
//            }

//        }

//        [HttpPost]
//        public ActionResult UpdRolOpciones(tbl_rol_opciones to)
//        {
//            ViewBag.id_rol = new SelectList(db.tbl_rol, "id_rol", "rol_desc");
//            ViewBag.id_opciones = new SelectList(db.tbl_opciones, "id_opciones", "opcion");

//            try
//            {
//                if (ModelState.IsValid)
//                {
//                    db.Entry(to).State = EntityState.Modified;
//                    db.SaveChanges();
//                }
//                return RedirectToAction("ListRolOpciones");
//            }
//            catch
//            {
//                return View();
//            }


//        }

//        [HttpPost]
//        public ActionResult FiltrarRolOpciones(String cadena)
//        {
//            if (String.IsNullOrEmpty(cadena))
//            {
//                var list = db.tbl_rol_opciones.ToList();
//                return View("ListRolOpciones", list);
//            }
//            else
//            {
//                var listFiltrada = db.tbl_rol_opciones.Where(X => X.tbl_opciones.opcion.Contains(cadena));
//                return View("ListRolOpciones", listFiltrada);
//            }

//        }

//        public ActionResult VerRptRolOpciones(String tipo)
//        {
//            LocalReport rpt = new LocalReport();
//            String mt, enc, f;
//            String[] s;
//            Warning[] w;

//            String ruta = Path.Combine(Server.MapPath("~/Reportes"), "rptRolOpciones.rdlc");
//            rpt.ReportPath = ruta;

//            List<tbl_rol_opciones> listaOpciones = new List<tbl_rol_opciones>();
//            listaOpciones = db.tbl_rol_opciones.ToList();

//            ReportDataSource rds = new ReportDataSource("dsRptRolOpciones", listaOpciones);
//            rpt.DataSources.Add(rds);

//            var b = rpt.Render(tipo, null, out mt, out enc, out f, out s, out w);

//            return new FileContentResult(b, mt);
//        }

//    }
//}