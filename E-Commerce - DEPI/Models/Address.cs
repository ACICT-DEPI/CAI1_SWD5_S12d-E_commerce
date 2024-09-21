using System.ComponentModel.DataAnnotations;

namespace E_Commerce___DEPI.Models
{
    public class Address
    {
        public int Id { get; set; }
        public int? Num { get; set; }

        public virtual Customer? Customer { get; set; }

        [MaxLength(200)]
        public string? Street { get; set; }

        public virtual City? City { get; set; }

        [MaxLength(10)]
        public string? ZipCode { get; set; }

        public virtual List<Order>? Orders { get; set; }

    }
}
