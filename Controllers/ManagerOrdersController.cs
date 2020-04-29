using EDa.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace EDa.Controllers {
    public class ManagerOrdersController : Controller {
        EdaContext db = new EdaContext();
        // GET: ManagerOrders
        public ActionResult Index() {
            List<int> Orders = new List<int>();
            foreach (var order in db.Orders) {
                Orders.Add(order.Id);
            }
            Orders.Reverse();
            ViewBag.Orders = Orders;
            return View();
        }

        [HttpPost]
        public ActionResult GetOrder(int id) {
            var Orders = db.Orders.Include(x => x.Products).Where(x => x.Id == id).ToList()[0];
            ViewBag.order = Orders;
            double Amount = 0;
            foreach (var product in Orders.Products) {
                Amount += product.Product.Cost * product.Amount;
            }
            ViewBag.Amount = Amount;

            return PartialView();
            //return HttpNotFound();
        }

        [HttpGet]
        public ActionResult EditOrder(int id) {
            ViewBag.id = id;
            return PartialView();
        }

        public JsonResult GetOrderJson(int id) {
            var Order = db.Orders.Where(x => x.Id == id).ToList<Order>()[0];
            return Json(Order, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetOrderProductsJson(int id) {
            List<OrderProduct> lst = db.OrderProducts.Where(x => x.OrderId == id).ToList();
            List<dynamic> ProductsList = new List<dynamic>();
            foreach (var product in lst) {
                var prod = new { Name = product.Product.Name, Amount = product.Amount };
                ProductsList.Add(prod);
            }
            return Json(ProductsList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetProducts() {
            List<string> Products = new List<string>();
            foreach (Product prod in db.Products.ToList()) {
                Products.Add(prod.Name);
            }
            return Json(Products, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void SaveEditOrder(string JsonString) {
            dynamic Data = JsonConvert.DeserializeObject(JsonString);
            int id = Data.Id;
            var order = db.Orders.Where(c => c.Id == id).FirstOrDefault();
            order.ClientName = Data.ClientName;
            order.ClientAdress = Data.ClientAdress;
            order.ClientPhone = Data.ClientPhone;
            foreach (var product in db.OrderProducts.Where(x => x.OrderId == id)) {
                db.OrderProducts.Remove(product);
            }
            foreach (var product in Data.Products) {
                string Name = product.Name;
                db.OrderProducts.Add(new OrderProduct { Order = order, ProductId = db.Products.Where(x => x.Name.Equals(Name)).FirstOrDefault().Id, Amount = product.Amount });
            }
            db.SaveChanges();
        }

        public void DeleteOrder(int id) {
            db.Orders.Remove(db.Orders.Where(c => c.Id == id).FirstOrDefault());
            foreach (var product in db.OrderProducts.Where(x => x.OrderId == id)) {
                db.OrderProducts.Remove(product);
            }
            db.SaveChanges();
        }
    }
}