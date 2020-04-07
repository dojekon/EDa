using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EDa.Models {
    public class Menu {
        public int Id { get; set; }
        public int Name { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}