using EDa.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EDa.Controllers {
    public class OperatorNewOrderController : Controller {
        EdaContext db = new EdaContext();
        // GET: OperatorNewOrder
        public ActionResult Index() {
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

        [HttpPost]
        public void CreateNewOrder(string JsonString) {
            dynamic Data = JsonConvert.DeserializeObject(JsonString);

            Order NewOrder = new Order { ClientName = Data.ClientName, ClientAdress = Data.ClientAdress, Date = DateTime.Now, ClientPhone = Data.ClientPhone };
            db.Orders.Add(NewOrder);

            foreach (var product in Data.Products) {
                string Name = product.Name;
                db.OrderProducts.Add(new OrderProduct { Order = NewOrder, ProductId = db.Products.Where(x => x.Name.Equals(Name)).FirstOrDefault().Id, Amount = product.Amount });
            }

            db.SaveChanges();
        }
    }
}