using System.ComponentModel.DataAnnotations.Schema;

namespace GP.Models
{
    public class Order
    {
        public int Id { get; set; }
        public OrderState Status { get; set; }
        public DateTime Date { get; set; }

        [ForeignKey("CustomerId")]
        public int? CustomerId { get; set; }
        public Customer Customer { get; set; }

        [ForeignKey("ShippingAdressId")]
        public int? ShippingAdressId { get; set; }
        public Address Address { get; set; }

        public List<OrderedItemsArchive> OrderedItemsArchives { get; set; }
        public List<OrderdItem> OrderdItems { get; set; }
    }
}
