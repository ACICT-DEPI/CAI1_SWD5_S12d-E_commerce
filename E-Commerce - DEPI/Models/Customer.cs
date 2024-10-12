using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace E_Commerce___DEPI.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [MaxLength(55), Required]
        public string Fname { get; set; }
        
        [Required,MaxLength(55)] 
        public string Lname { get; set; }

        [Display(Name = "Full Name")]
        public string FullName
        {
            get
            {
                return Lname + " " + Fname;
            }
        }

        [Required,MaxLength(55)]
        public string Email { get; set; }

        [MaxLength(20)]
        public string Phone { get; set; }

        [MaxLength(20)]
        public string Password { get; set; }

        [ForeignKey("AddressId")]
        public int AddressId { get; set; }

        public virtual Address? Address { get; set; }
        //public virtual List<Order>? Order { get; set; }
        // Updated to use ICollection
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

        public virtual ICollection<CartItem>? CartItems { get; set; } = new List<CartItem>();

        //public virtual ICollection<OrdersArchive>? OrdersArchive { get; set; } = new List<OrdersArchive>();

    }
}
