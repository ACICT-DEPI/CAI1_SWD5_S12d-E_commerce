using System.ComponentModel.DataAnnotations;

namespace GP.Models
{
    public class UpholsteryMat
    {
        [Key]
        public int No {  get; set; }

        [MaxLength(20)]
        public string Name { get; set; }

        public List<Product> Products { get; set; }
    }
}
