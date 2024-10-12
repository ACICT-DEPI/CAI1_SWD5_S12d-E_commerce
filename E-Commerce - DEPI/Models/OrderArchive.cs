using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce___DEPI.Models
{
    public class OrderArchive
    {
        public int Id { get; set; }
        public int OrderId { get; set; }

        [ForeignKey("OrderId")]
        public virtual Order? Order { get; set; }

        public DateTime ArchiveDate { get; set; } // Add Date property
    }
}
