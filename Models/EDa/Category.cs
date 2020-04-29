using System.Collections.Generic;

namespace EDa.Models {
    public class Category {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public Category() {
            Products = new List<Product>();
        }
    }
}