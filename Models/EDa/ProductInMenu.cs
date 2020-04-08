using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace EDa.Models {
    public class ProductInMenu {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int MenuId { get; set; }
    }
}