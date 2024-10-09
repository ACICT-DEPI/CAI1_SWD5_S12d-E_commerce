using System.ComponentModel.DataAnnotations;


namespace E_Commerce___DEPI.Models
{
    public class Feedback
    {
        public int Id { get; set; }
        public int? CustomerID { get; set; }
        public virtual Customer? Customer { get; set; }

        public int? ProductId { get; set; }
        public virtual Product? Product { get; set; }

        [MaxLength(200)]
        public string Content { get; set; }

        public int Rate { get; set; }
    }
}
