using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Script.Serialization;

namespace EDa.Models {
    public class OrderProduct {
        public int Id { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public int Amount { get; set; }
        
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        
        public virtual Order Order{ get; set; }

        public virtual Product Product { get; set; }

    }
}