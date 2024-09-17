using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GP.Models
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

        public Address Address { get; set; }
        public List<Order> Order { get; set; }

        public List<CartItem> CartItems { get; set; }

        public List<OrdersArchive> OrdersArchive { get; set; }

     }
}
