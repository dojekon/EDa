using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace EDa.Models {
    public class OperatorOrdersController : Controller {
        EdaContext db = new EdaContext();
        // GET: OperatorOrders
        public ActionResult Index() {
            if (Session["UserGroup"] != null) {
                if (Session["UserGroup"].ToString() == "operator") {
                    List<int> Orders = new List<int>();
                    foreach (var order in db.Orders) {
                        Orders.Add(order.Id);
                    }
                    Orders.Reverse();
                    ViewBag.Orders = Orders;
                    return View();
                } else return HttpNotFound();
            } else return HttpNotFound();
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
    }
}