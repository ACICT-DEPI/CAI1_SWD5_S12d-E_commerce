using System.ComponentModel.DataAnnotations.Schema;


namespace E_Commerce___DEPI.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int CustomerID { get; set; }
        public virtual List<Customer>? Customer { get; set; }

        public virtual List<Product>? Product { get; set; }
        public int Quantity { get; set; }
    }
}
