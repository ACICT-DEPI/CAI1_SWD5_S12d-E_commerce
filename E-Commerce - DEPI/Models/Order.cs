using System.ComponentModel.DataAnnotations.Schema;


namespace E_Commerce___DEPI.Models
{
    public class Order
    {
        public int Id { get; set; }
        public virtual OrderState? Status { get; set; }
        public DateTime Date { get; set; }
        public int? CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customer? Customer { get; set; }
        public int? AddressId { get; set; }

        [ForeignKey("AddressId")]
        public virtual Address? Address { get; set; }
        public virtual List<OrderedItemsArchive>? OrderedItemsArchives { get; set; }
        public virtual List<OrderdItem>? OrderdItems { get; set; }
    }
}
