using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace E_Commerce___DEPI.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required,MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public double Price {  get; set; }
        [Required, MaxLength(300)]
        public string Description { get; set; }
        public string img1 { get; set; }
        public string? img2 { get; set; }
        public string? img3 { get; set; }
        public string? img4 { get; set; }
        public string? img5 { get; set; }
        public string? img6 { get; set; }
        public float Rate { get; set; }
        
        [ForeignKey("Category")]
        public int? CatId { get; set; }
        public virtual Category? Category { get; set; }
        
        [ForeignKey("UpholstertMat")]
        public int? Upholstery_mat_no { get; set; }
        public virtual UpholsteryMat? UpholsteryMat { get; set; }
        [ForeignKey("FrameMateNo")]
        public int? FrameMateNo { get; set; }
        public virtual List<OrderedItemsArchive>? orderedItemsArchives { get; set; }
        public virtual List<OrderdItem>? orderdItems { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public float Depth { get; set; }



    }
}
