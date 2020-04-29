using EDa.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace EDa.Controllers {
    public class OperatorController : Controller {
        EdaContext db = new EdaContext();
        // GET: Operator
        public ActionResult Index() {
            if (Session["UserGroup"] != null) {
                if (Session["UserGroup"].ToString() == "operator") {
                    int menuId = 0;
                    foreach (var menu in db.Menus) {
                        if (menu.Date == DateTime.Now.Date) menuId = menu.Id;
                    }

                    if (menuId != 0) {
                        List<Product> TodayMenu = new List<Product>();

                        foreach (var productInMenu in db.ProductsInMenus) {
                            if (productInMenu.MenuId == menuId) {
                                TodayMenu.Add(db.Products.Find(productInMenu.ProductId));
                            }
                        }
                        ViewBag.Menu = TodayMenu;
                    }

                    DateTime dateAndTime = DateTime.Now;
                    ViewBag.DateToday = dateAndTime.ToString("dd.MM.yyyy");

                    List<string> todayOrders = new List<string>();

                    foreach (var order in db.Orders) {
                        if (order.Date == DateTime.Today) {
                            todayOrders.Add("№ " + order.Id);
                        }
                    }
                    ViewBag.todayOrders = todayOrders;


                    return View();
                } else return HttpNotFound();
            } else return HttpNotFound();
        }
    }
}