using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace EDa.Models {
    public class ProductDbInitializer : CreateDatabaseIfNotExists<EdaContext> {
        protected override void Seed(EdaContext db) {
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
        }
    }
}