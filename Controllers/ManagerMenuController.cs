using EDa.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace EDa.Controllers {
    public class ManagerMenuController : Controller {
        EdaContext db = new EdaContext();

        public class MenuEx {
            public int ID { get; set; }
            public string Date { get; set; }
            public dynamic Products { get; set; }
        }

        // GET: ManagerMenu
        public ActionResult Index() {
            if (Session["UserGroup"] != null) {
                if (Session["UserGroup"].ToString() == "manager") {
                    List<dynamic> Menu = new List<dynamic>();
                    foreach (var menu in db.Menus.ToList()) {
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
                } else return HttpNotFound();
            } else return HttpNotFound();
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
            editableProd.Cost = Data.NewProdCost;
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

        [HttpPost]
        public void AddMenu(string JsonString) {
            dynamic Data = JsonConvert.DeserializeObject(JsonString);
            DateTime date = DateTime.Parse(Data.Date.Value.ToString() + " 00:00");
            if (db.Menus.Where(x => x.Date == date).ToList().Count == 0) {
                db.Menus.Add(new Menu { Date = date });
                db.SaveChanges();
                int MenuId = db.Menus.Where(x => x.Date == date).FirstOrDefault().Id;
                foreach (var product in Data.Products) {
                    string ProductName = product.Name;
                    db.ProductsInMenus.Add(new ProductInMenu { MenuId = MenuId, ProductId = db.Products.Where(x => x.Name == ProductName).FirstOrDefault().Id });
                }
                db.SaveChanges();
            }
        }

        [HttpPost]
        public void AddProduct(string JsonString) {
            dynamic Data = JsonConvert.DeserializeObject(JsonString);
            string Name = Data.ProductName.ToString();
            if (db.Products.Where(x => x.Name == Name).ToList().Count == 0) {
                string CatName = Data.ProductCat;
                db.Products.Add(new Product { Name = Name, Cost = Data.ProductCost, CategoryId = db.Categories.Where(x => x.Name == CatName).FirstOrDefault().Id });
                db.SaveChanges();
            }
        }

        public void AddCategory(string name) {
            db.Categories.Add(new Category { Name = name });
            db.SaveChanges();
        }

        public void DeleteMenu(string Date) {
            DateTime date = DateTime.Parse(Date);
            Menu menu = db.Menus.Where(x => x.Date == date).FirstOrDefault();
            foreach (ProductInMenu product in db.ProductsInMenus.ToList()) {
                if (product.MenuId == menu.Id) db.ProductsInMenus.Remove(product);
            }
            db.Menus.Remove(menu);
            db.SaveChanges();
        }

        public JsonResult GetMenuProducts(string Date) {
            List<string> products = new List<string>();
            DateTime MenuDate = DateTime.Parse(Date);
            Menu menu = db.Menus.Where(x => x.Date == MenuDate).FirstOrDefault();
            foreach (var productMenu in db.ProductsInMenus.ToList()) {
                if (productMenu.MenuId == menu.Id) {
                    products.Add(db.Products.Where(x => x.Id == productMenu.ProductId).FirstOrDefault().Name);
                }
            }

            return Json(products, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void EditMenu(string JsonString) {
            dynamic Data = JsonConvert.DeserializeObject(JsonString);
            DateTime MenuDate = DateTime.Parse(Data.OldDate.Value);
            Menu menu = db.Menus.Where(x => x.Date == MenuDate).FirstOrDefault();
            if (Data.OldDate != Data.NewDate) {
                menu.Date = DateTime.Parse(Data.NewDate);
            }
            foreach (ProductInMenu product in db.ProductsInMenus.ToList()) {
                if (product.MenuId == menu.Id) db.ProductsInMenus.Remove(product);
            }

            foreach (string product in Data.Products) {
                db.ProductsInMenus.Add(new ProductInMenu { MenuId = menu.Id, ProductId = db.Products.Where(x => x.Name == product).FirstOrDefault().Id });
            }
            db.SaveChanges();
        }

        [HttpPost]
        public void EditCategory(string JsonString) {
            dynamic Data = JsonConvert.DeserializeObject(JsonString);
            string OldName = Data.OldName.Value;
            Category cat = db.Categories.Where(x => x.Name == OldName).FirstOrDefault();
            if (cat.Name != Data.NewName.Value) cat.Name = Data.NewName.Value;
            db.SaveChanges();
        }

        public void DeleteCategory(string Name) {
            Category cat = db.Categories.Where(x => x.Name == Name).FirstOrDefault();

            foreach (Product prod in db.Products.ToList()) {
                if (prod.Category == cat) {
                    foreach (OrderProduct orderProduct in db.OrderProducts.ToList()) {
                        if (orderProduct.Product == prod) db.OrderProducts.Remove(orderProduct);
                    }
                    foreach (ProductInMenu productInMenu in db.ProductsInMenus.ToList()) {
                        if (productInMenu.ProductId == prod.Id) db.ProductsInMenus.Remove(productInMenu);
                    }
                    db.Products.Remove(prod);
                }
            }

            db.Categories.Remove(cat);

            db.SaveChanges();
        }
    }
}