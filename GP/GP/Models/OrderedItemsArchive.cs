using System.ComponentModel.DataAnnotations.Schema;

namespace GP.Models
{
    public class OrderedItemsArchive
    {
        public int Id { get; set; }

        [ForeignKey("OrderId")]
        public int OrderId { get; set; }
        public Order Order { get; set; }
        [ForeignKey("ProductId")]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Amount { get; set; }

    }
}
