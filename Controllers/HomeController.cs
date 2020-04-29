using EDa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace EDa.Controllers {
    public class HomeController : Controller {
        EdaContext db = new EdaContext();

        public ActionResult Index() {
            using (EdaContext db = new EdaContext()) {
               
                return View();
            }
        }

        [HttpPost]
        public RedirectResult Login(string login, string pass) {
            string UserGroup = db.Authorisers.Where(x => x.Username == login).Where(x => x.Password == pass).FirstOrDefault().UserGroup;
            if (UserGroup != null) {
                if (UserGroup == "manager") {
                    Session["UserGroup"] = "manager";
                    return Redirect("/Manager/Index");
                } if (UserGroup == "operator") {
                    Session["UserGroup"] = "operator";
                    return Redirect("/Operator/Index");
                }
            }
            return Redirect("/Home/LoginError");
        }
        public RedirectResult LogOut() {
            Session["UserGroup"] = "null";
            return Redirect("/Home/Index");
        }
    }
}