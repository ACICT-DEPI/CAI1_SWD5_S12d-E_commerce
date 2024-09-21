using System.ComponentModel.DataAnnotations;


namespace E_Commerce___DEPI.Models
{
    public class FrameMat
    {
        [Key]
        public int No { get; set; }

        [MaxLength(20)]
        public string Name { get; set; }

        public virtual List<Product>? Products { get; set; }
    }
}
