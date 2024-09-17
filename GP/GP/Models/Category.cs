using System.ComponentModel.DataAnnotations;

namespace GP.Models
{
    public class Category
    {
        public int Id { get; set; }
        [MaxLength(50),Required]
        public string Name { get; set; }

        public List<Product> Products { get; set; }
    }
}
