using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace E_Commerce___DEPI.Models
{
    public class Customer : User
    {
        //public virtual List<Order>? Order { get; set; }
        // Updated to use ICollection
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

        public virtual ICollection<CartItem>? CartItems { get; set; } = new List<CartItem>();

        //public virtual ICollection<OrdersArchive>? OrdersArchive { get; set; } = new List<OrdersArchive>();
    }
}
