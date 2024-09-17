using System.ComponentModel.DataAnnotations.Schema;

namespace GP.Models
{
    public class OrdersArchive
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [ForeignKey("Customer_id")]
        public int Customer_id { get; set; }
        public Customer Customer { get; set; }
      
        [ForeignKey("Shipping_Address_Id")]

        public int Shipping_Address_Id { get; set; }
        public Address Address { get; set; }
    }
}
