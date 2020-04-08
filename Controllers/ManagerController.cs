using EDa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EDa.Controllers
{
    public class ManagerController : Controller
    {
        // GET: Manager
        EdaContext db = new EdaContext();
        public ActionResult Index()
        {
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

            return View();
        }
        /*
        public ActionResult Orders() {

        }*/

    }
    public class OrdersController : Controller  {
        public ActionResult Index() {
            return View();
        }
          
    }
}