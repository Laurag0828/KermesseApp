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
        // GET: Tbl_usuario
        public ActionResult ListUsuario() //METODO PARA INVOCAR LA VISTA
        {
            return View(db.tbl_usuario.ToList()); //RETORNA LA VISTA
        }
        public ActionResult VGuardarUsuario() //METODO PARA INVOCAR LA VISTA QUE REGISTRA UN NUEVO USUARIO
        {
            return View(); //RETORNA LA VISTA
        }
        public ActionResult ViewLogin()
        {
            return View();
        }


        [HttpPost]
        public ActionResult GuardarUsuario(tbl_usuario tus) //METODO PARA GUARDAR AL USUARIO
        {
            if (ModelState.IsValid)
            {
                tbl_usuario tbus = new tbl_usuario();
                tbus.usuario = tus.usuario;
                tbus.nombres = tus.nombres;
                tbus.pwd = tus.pwd;
                tbus.confirmarpwd = tus.pwd;
                tbus.email = tus.email;
                tbus.estado = 1;
                db.tbl_usuario.Add(tbus);
                db.SaveChanges(); //GUARDA LOS DATOS

            }
            ModelState.Clear();
            return View("VGuardarUsuario"); //RETORNA LA VISTA VACIA PARA GUARDAR UNA CATEGORIA



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
                var listFiltrada = db.tbl_usuario.Where(X => X.nombres.Contains(cadena) || X.usuario.Contains(cadena));
                return View("ListUsuario",listFiltrada);
            }
          
        }

        public ActionResult VerRptUsuario(string tipo, string cadena)
        {
            LocalReport rpt = new LocalReport();
            String mt, enc, f;
            String[] s;
            Warning[] w;

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "rptUsuario.rdlc");
            rpt.ReportPath = ruta;

            ReportDataSource rd = null;
            if(string.IsNullOrEmpty(cadena))
            {
                var lista = db.tbl_usuario.ToList();
                rd = new ReportDataSource("dsRptUsuario", lista);

            }
            else
            {
                var lista = db.tbl_usuario.Where(x => x.usuario.Contains(cadena) || x.nombres.Contains(cadena) || x.email.Contains(cadena));
                rd = new ReportDataSource("dsRptUsuario", lista);
            }

            rpt.DataSources.Add(rd);
            var b = rpt.Render(tipo, null, out mt, out enc, out f, out s, out w);

            return new FileContentResult(b, mt);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string username, string password)
        {
            if (String.IsNullOrEmpty(username) && String.IsNullOrEmpty(password))
            {
                return View("ViewLogin");
            }
            else
            {
                var objeto = db.tbl_usuario.Where(x => x.usuario.Equals(username) && x.pwd.Equals(password)).FirstOrDefault();
                if (objeto != null)
                {
                    Session["usuario"] = objeto;
                    return Redirect("~/Home/Index");
                    //return RedirectToRoute("Default", new { controller = "Home", action = "Index" });
                    //return this.RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.error = "¡Los datos de accesos son incorrectos, por favor intente nuevamente!";
                    return View("ViewLogin");
                }
            }
        }

        [HttpGet]
        public ActionResult Logout()
        {
            Session.Remove("usuario");
            return Redirect("~/tbl_usuario/ViewLogin");
            //return View("ViewLogin");
        }


    }
}