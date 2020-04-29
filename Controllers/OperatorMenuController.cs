using EDa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;


namespace EDa.Controllers {
    public class OperatorMenuController : Controller {
        EdaContext db = new EdaContext();
        // GET: OperatorMenu
        public ActionResult Index() {
            if (Session["UserGroup"] != null) {
                if (Session["UserGroup"].ToString() == "operator") {
                    return View();
                } else return HttpNotFound();
            } else return HttpNotFound();
        }
        [HttpPost]
        public ActionResult TodayMenu(string none) {
            int MenuID = 0;
            foreach (var menu in db.Menus.ToList()) {
                if (menu.Date == DateTime.Today) {
                    MenuID = menu.Id;
                }
            }
            if (MenuID > 0) {
                List<string> Products = new List<string>();
                foreach (ProductInMenu product in db.ProductsInMenus.ToList()) {
                    if (product.MenuId == MenuID) {
                        Products.Add(db.Products.Where(x => x.Id == product.ProductId).FirstOrDefault().Name);
                    }
                }
                ViewBag.Products = Products;
            } else return HttpNotFound();
            return PartialView();
        }

        [HttpPost]
        public ActionResult WeekMenu(string none) {
            List<int> MenuIds = new List<int>();
            for (int i = 0; i <= 7; i++) {
                DateTime currDate = DateTime.Today.AddDays(i);
                if (db.Menus.Where(x => x.Date == currDate).FirstOrDefault() != null) {
                    MenuIds.Add(db.Menus.Where(x => x.Date == currDate).FirstOrDefault().Id);
                }
            }
            Dictionary<string, List<string>> Menus = new Dictionary<string, List<string>>();
            foreach (int MenuID in MenuIds) {
                List<string> Products = new List<string>();
                foreach (ProductInMenu product in db.ProductsInMenus.ToList()) {
                    if (product.MenuId == MenuID) Products.Add(db.Products.Where(x => x.Id == product.ProductId).FirstOrDefault().Name);
                }
                Menus.Add(db.Menus.Find(MenuID).Date.ToShortDateString(), Products);
            }
            ViewBag.Menus = Menus;
            return PartialView();
        }
    }
}