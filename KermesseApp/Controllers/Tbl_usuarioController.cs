using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KermesseApp.Models;
using System.Data.Entity.Validation;
using System.Data.Entity;
using Microsoft.Reporting.WebForms;
using System.IO;
               

namespace KermesseApp.Controllers
{
    public class Tbl_usuarioController : Controller
    {
        private KERMESSEEntities db = new KERMESSEEntities();

        public ActionResult ListUsuario() //METODO PARA INVOCAR LA VISTA
        {
            return View(db.tbl_usuario.ToList()); //RETORNA LA VISTA
        }

        public ActionResult VGuardarUsuario() //METODO PARA INVOCAR LA VISTA QUE REGISTRA UN NUEVO USUARIO
        {
            return View(); //RETORNA LA VISTA
        }


        [HttpPost]
        public ActionResult GuardarUsuario(tbl_usuario tus) //METODO PARA GUARDAR AL USUARIO
        {
            if (ModelState.IsValid)
            {
                tbl_usuario tbus = new tbl_usuario();
                tbus.usuario = tus.usuario;
                tbus.nombres = tus.nombres;
                tbus.apellidos = tus.apellidos;
                tbus.pwd = tus.pwd;
                tbus.confirmarpwd = tus.pwd;
                tbus.email = tus.email;
                tbus.estado = 1;
                db.tbl_usuario.Add(tbus);
                db.SaveChanges(); //GUARDA LOS DATOS

            }
            ModelState.Clear();
            return RedirectToAction("ListUsuario");

        }


        public ActionResult DeleteUsuario(int id)
        {
            tbl_usuario tus = new tbl_usuario();
            tus = db.tbl_usuario.Find(id);
            db.tbl_usuario.Remove(tus);

            db.SaveChanges();
            var list = db.tbl_usuario.ToList();
            return View("ListUsuario", list);
            
        }

        public ActionResult VerUsuario(int id)
        {
            var Usuario = db.tbl_usuario.Where(x => x.id_usuario == id).First();
            return View(Usuario);
        }

        public ActionResult EditUsuario(int id)
        {
            tbl_usuario tbu = db.tbl_usuario.Find(id);
            if (tbu == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(tbu);
            }
            
        }

        [HttpPost]
        public ActionResult UpdUsuario(tbl_usuario tu)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(tu).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("ListUsuario");
            }
            catch
            {
                return View();
            }

        }

        [HttpPost]
        public ActionResult FiltrarUsuario(String cadena)
        {
            if(String.IsNullOrEmpty(cadena))
            {
                var list = db.tbl_usuario.ToList();
                return View("ListUsuario", list);
            }
            else
            {
                var listFiltrada = db.tbl_usuario.Where(X => X.nombres.Contains(cadena) || X.apellidos.Contains(cadena) || X.usuario.Contains(cadena));
                return View("ListUsuario",listFiltrada);
            }
          
        }

        public ActionResult VerRptUsuario(String tipo)
        {
            LocalReport rpt = new LocalReport();
            String mt, enc, f;
            String[] s;
            Warning[] w;

            String ruta = Path.Combine(Server.MapPath("~/Reportes"), "rptUsuario.rdlc");
            rpt.ReportPath = ruta;

            List<tbl_usuario> listaUsuario = new List<tbl_usuario>();
            listaUsuario = db.tbl_usuario.ToList();

            ReportDataSource rds = new ReportDataSource("dsRptUsuario", listaUsuario);
            rpt.DataSources.Add(rds);

            var b = rpt.Render(tipo, null, out mt, out enc, out f, out s, out w);

            return new FileContentResult(b, mt);
        }

    }
}