using EDa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace EDa.Controllers {

    public static class DateTimeExtensions {
        public static bool IsInRange(this DateTime dateToCheck, DateTime startDate, DateTime endDate) {
            return dateToCheck >= startDate && dateToCheck < endDate;
        }

    }
    public class ManagerStatisticsController : Controller {
        EdaContext db = new EdaContext();
        // GET: ManagerStatistics
        public ActionResult Index() {
            if (Session["UserGroup"] != null) {
                if (Session["UserGroup"].ToString() == "manager") {
                    return View();
                } else return HttpNotFound();
            } else return HttpNotFound();
        }

        public JsonResult GetStat(string dates) {
            string[] Dates = dates.Split(new char[] { ',' });
            DateTime startDate = DateTime.Parse(Dates[0]);
            DateTime endDate = DateTime.Parse(Dates[1]);

            Dictionary<string, int> OrderStat = new Dictionary<string, int>();
            DateTime tempDays = startDate;
            while (tempDays.IsInRange(startDate, endDate)) {
                OrderStat.Add(tempDays.ToShortDateString(), 0);
                tempDays = tempDays.AddDays(1);
            }
            foreach (Order order in db.Orders.ToList()) {
                if (OrderStat.ContainsKey(order.Date.ToShortDateString())) {
                    OrderStat[order.Date.ToShortDateString()]++;
                }
            }

            Dictionary<string, int> FoodStat = new Dictionary<string, int>();
            foreach (Product product in db.Products.ToList()) {
                FoodStat.Add(product.Name, 0);
            }

            foreach (OrderProduct orderProduct in db.OrderProducts.ToList()) {
                if (db.Orders.Find(orderProduct.OrderId).Date.IsInRange(startDate, endDate)) {
                    FoodStat[orderProduct.Product.Name]++;
                }
            }
            var Stats = new { orderStat = OrderStat, foodStat = FoodStat };
            return Json(Stats, JsonRequestBehavior.AllowGet);
        }
    }
}