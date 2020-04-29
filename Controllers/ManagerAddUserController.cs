using EDa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EDa.Controllers {
    public class ManagerAddUserController : Controller {
        EdaContext db = new EdaContext();
        // GET: ManagerAddUser
        public ActionResult Index() {
            if (Session["UserGroup"] != null) {
                if (Session["UserGroup"].ToString() == "manager") {
                    ViewBag.Users = db.Authorisers.ToList();
                    return View();
                } else return HttpNotFound();
            } else return HttpNotFound();
        }
        [HttpPost]
        public RedirectResult AddUser(string username, string password, string usergroup) {
            string Usergroup = "";
            if (usergroup == "Оператор") { Usergroup = "operator"; } else Usergroup = "Manager";
            db.Authorisers.Add(new AuthoriseTable { Username = username, Password = password, UserGroup = Usergroup });
            db.SaveChanges();
            return Redirect("/ManagerAddUser/Index");
        }

        public void DeleteUser(string username) {
            db.Authorisers.Remove(db.Authorisers.Where(x => x.Username == username).FirstOrDefault());
            db.SaveChanges();
        }
    }
}