using System.Data.Entity;


namespace EDa.Models {
    public class EdaContext : DbContext {

        public EdaContext() : base("EdaContext") { }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        public DbSet<ProductInMenu> ProductsInMenus { get; set; }
    }
}