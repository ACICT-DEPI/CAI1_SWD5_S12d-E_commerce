using System.ComponentModel.DataAnnotations;

namespace GP.Models
{
    public class Feedback
    {
        public int CustomerID { get; set; }
        public Customer Customer { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        [MaxLength(200)]
        public string Content { get; set; }

        public int Rate { get; set; }
    }
}
