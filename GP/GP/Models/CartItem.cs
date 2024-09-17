using System.ComponentModel.DataAnnotations.Schema;

namespace GP.Models
{
    public class CartItem
    {
        public int CustomerID { get; set; }
        public List<Customer> Customer { get; set; }

        public List<Product> Product { get; set; }
        public int Quantity { get; set; }
    }
}
