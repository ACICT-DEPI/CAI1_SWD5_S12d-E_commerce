using System.ComponentModel.DataAnnotations;

namespace GP.Models
{
    public class Address
    {
        public int Id { get; set; }
        public int? Num { get; set; }

        public Customer Customer { get; set; }

        [MaxLength(200)]
        public string Street { get; set; }

        public City City { get; set; }

        [MaxLength(10)]
        public string ZipCode { get; set; }

        public List<Order> Orders { get; set; }

    }
}
