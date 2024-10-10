using System.ComponentModel.DataAnnotations.Schema;


namespace E_Commerce___DEPI.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public virtual Customer? Customer { get; set; }

        public virtual Product? Product { get; set; }
        public int Quantity { get; set; }
    }
}
