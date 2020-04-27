using EDa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace EDa.Controllers {
    public class HomeController : Controller {
        
        public ActionResult Index() {
            using (EdaContext db = new EdaContext()) {
               
                return View();
            }
        }
    }
}