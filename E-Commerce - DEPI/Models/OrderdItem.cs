using System.ComponentModel.DataAnnotations.Schema;


namespace E_Commerce___DEPI.Models
{
    public class OrderdItem
    {
        public int Id { get; set; }
        public int Amount { get; set; }

        public int? Order_id { get; set; }
        public virtual Order? Order { get; set; }
        
        [ForeignKey("ProductId")]
        public int? ProductId { get; set; }
        public virtual Product? Product { get; set; }

    }
}
