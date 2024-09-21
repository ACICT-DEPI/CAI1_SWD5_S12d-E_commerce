using System.ComponentModel.DataAnnotations;


namespace E_Commerce___DEPI.Models
{
    public class Category
    {
        public int Id { get; set; }
        [MaxLength(50),Required]
        public string Name { get; set; }

        public virtual List<Product>? Products { get; set; }
    }
}
