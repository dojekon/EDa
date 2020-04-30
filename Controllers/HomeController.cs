using EDa.Models;
using System.Linq;
using System.Web.Mvc;

namespace EDa.Controllers {
    public class HomeController : Controller {
        EdaContext db = new EdaContext();

        public ActionResult Index() {
            using (EdaContext db = new EdaContext()) {

                return View();
            }
        }

        public ActionResult LoginError() {
                return View();          
        }

        [HttpPost]
        public RedirectResult Login(string login, string pass) {
            if (db.Authorisers.Where(x => x.Username == login).Where(x => x.Password == pass).FirstOrDefault().UserGroup != null) {
                string UserGroup = db.Authorisers.Where(x => x.Username == login).Where(x => x.Password == pass).FirstOrDefault().UserGroup;
                if (UserGroup != null) {
                    if (UserGroup == "manager") {
                        Session["UserGroup"] = "manager";
                        return Redirect("/Manager/Index");
                    }
                    if (UserGroup == "operator") {
                        Session["UserGroup"] = "operator";
                        return Redirect("/Operator/Index");
                    }
                }
            } else return Redirect("/Home/LoginError");
            return Redirect("/Home/LoginError");
        }


        public RedirectResult LogOut() {
            Session["UserGroup"] = "null";
            return Redirect("/Home/Index");
        }
    }
}