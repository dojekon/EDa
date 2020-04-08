using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EDa.Models {
    public class Product {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Cost { get; set; }

        public int? CategoryId { get; set; }
        public virtual Category Category { get; set; }

    }
}