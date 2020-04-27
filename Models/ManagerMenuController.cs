using EDa.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EDa.Controllers {
    public class ManagerMenuController : Controller
    {
        EdaContext db = new EdaContext();

        public class MenuEx {
            public int ID { get; set; }
            public string Date { get; set; }
            public dynamic Products { get; set; }
        }

        // GET: ManagerMenu
        public ActionResult Index()
        {
            List<dynamic> Menu = new List<dynamic>();
            foreach(var menu in db.Menus.ToList()) {
                List<Product> products = new List<Product>();
                foreach (var productMenu in db.ProductsInMenus.ToList()) {
                    if (productMenu.MenuId == menu.Id) {
                        products.Add(db.Products.Where(x => x.Id == productMenu.ProductId).FirstOrDefault());
                    }
                }
                MenuEx menuEx = new MenuEx();
                menuEx.ID = menu.Id;
                menuEx.Date = menu.Date.ToShortDateString().ToString();
                menuEx.Products = products;
                Menu.Add(menuEx);
            }
            ViewBag.Menus = Menu;

            Dictionary<string, List<Product>> CatProd = new Dictionary<string, List<Product>>();
            foreach (Category cat in db.Categories.ToList()) {
                List<Product> products = new List<Product>();
                foreach (Product prod in db.Products.ToList()) {
                    if (prod.CategoryId == cat.Id) products.Add(prod);
                }
                if (products.Count > 0) CatProd.Add(cat.Name, products);
            }
            ViewBag.CatProd = CatProd;
            return View();
        }
        public JsonResult GetCategories() {
            List<string> Categories = new List<string>();
            foreach (Category cat in db.Categories.ToList()) {
                Categories.Add(cat.Name);
            }
            return Json(Categories, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public void SaveEditProd(string JsonString) {
            dynamic Data = JsonConvert.DeserializeObject(JsonString);
            string OldProdName = Data.ProdName;
            string NewCat = Data.NewProdCat;
            Product editableProd = db.Products.Where(x => x.Name == OldProdName).FirstOrDefault();
            editableProd.Name = Data.NewProdName;
            editableProd.Category = db.Categories.Where(x => x.Name == NewCat).FirstOrDefault();

            db.SaveChanges();
        }

        public void DeleteProduct(string name) {
            Product DelitableProd = db.Products.Where(x => x.Name == name).FirstOrDefault();
            foreach (OrderProduct orderProduct in db.OrderProducts.ToList()) {
                if (orderProduct.ProductId == DelitableProd.Id) db.OrderProducts.Remove(orderProduct);
            }
            foreach (ProductInMenu productInMenu in db.ProductsInMenus.ToList()) {
                if (productInMenu.ProductId == DelitableProd.Id) db.ProductsInMenus.Remove(productInMenu);
            }
            db.Products.Remove(DelitableProd);
            db.SaveChanges();
        }
    }
}