using EDa.Models.EDa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EDa.Models {
    public class Order {
        public int Id { get; set; }
        public string ClientName{ get; set; }
        public string ClientAdress { get; set; }
        public DateTime Date { get; set; }
        public ICollection<OrderProduct> OrderProducts { get; set; }

    }
}