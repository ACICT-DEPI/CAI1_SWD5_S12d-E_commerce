using System.ComponentModel.DataAnnotations.Schema;

namespace GP.Models
{
    public class OrderdItem
    {
        public int Id { get; set; }
        public int Amount { get; set; }

        public int Order_id { get; set; }
        public Order Order { get; set; }
        
        [ForeignKey("ProductId")]
        public int ProductId { get; set; }
        public Product Product { get; set; }

    }
}
