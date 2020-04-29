using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace EDa.Models {
    public class ProductDbInitializer : DropCreateDatabaseAlways<EdaContext> {
        protected override void Seed(EdaContext db) {
            db.Menus.Add(new Menu { Date = DateTime.Now.Date });

            db.Categories.Add(new Category { Name = "Супы" });
            db.Categories.Add(new Category { Name = "Вторые блюда" });
            db.Categories.Add(new Category { Name = "Салаты" });
            db.Categories.Add(new Category { Name = "Напитки" });
            db.Categories.Add(new Category { Name = "Десерты" });

            db.Products.Add(new Product { Name = "Борщ", Cost = 120, CategoryId = 1 });
            db.Products.Add(new Product { Name = "Солянка", Cost = 110, CategoryId = 1 });
            db.Products.Add(new Product { Name = "Фрикасе из курицы", Cost = 350, CategoryId = 2 });
            db.Products.Add(new Product { Name = "Котлета", Cost = 70, CategoryId = 2 });
            db.Products.Add(new Product { Name = "Цезарь", Cost = 250, CategoryId = 3 });
            db.Products.Add(new Product { Name = "Чай", Cost = 50, CategoryId = 4 });
            db.Products.Add(new Product { Name = "Кофе", Cost = 80, CategoryId = 4 });

            db.ProductsInMenus.Add(new ProductInMenu { MenuId = 1, ProductId = 1});
            db.ProductsInMenus.Add(new ProductInMenu { MenuId = 1, ProductId = 3 });
            db.ProductsInMenus.Add(new ProductInMenu { MenuId = 1, ProductId = 6 });

            Order o1 = new Order { ClientName = "Саша", ClientAdress = "Наставников 5/3", Date = DateTime.Today, ClientPhone = "22505" };
            db.Orders.Add(o1);
            db.Orders.Add(new Order { ClientName = "Марина", ClientAdress = "Ленская 5/3", Date = DateTime.Today, ClientPhone = "5552425" });
            db.SaveChanges();
            db.OrderProducts.Add(new OrderProduct { Order = o1, ProductId = 1, Amount = 1 });
            db.OrderProducts.Add(new OrderProduct { Order = o1, ProductId = 3, Amount = 1});
            db.OrderProducts.Add(new OrderProduct { Order = o1, ProductId = 6, Amount = 2 });
            db.Configuration.ProxyCreationEnabled = false;

            db.SaveChanges();
        }
    }
}