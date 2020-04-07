using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EDa.Models.EDa {
    public class OrderProduct {
        public int Id { get; set; }
        public int? OrderId { get; set; }
        public Order Order { get; set; }
        public int ProductId { get; set; }

    }
}