using EDa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EDa.Controllers
{
    public class ManagerOrdersController : Controller
    {
        EdaContext db = new EdaContext();
        // GET: ManagerOrders
        public ActionResult Index()
        {
            List<int> Orders = new List<int>();
            foreach (var order in db.Orders) {
                Orders.Add(order.Id);
            }
            ViewBag.Orders = Orders;
            return View();
        }

        [HttpPost]
        public ActionResult GetOrder(int id) {
            foreach (var order in db.Orders) {
                if (order.Id == id) 
                    return PartialView(order);
            }
            return HttpNotFound();
        }
    }
}