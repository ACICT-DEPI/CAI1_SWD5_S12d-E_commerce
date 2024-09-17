using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GP.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required,MaxLength(100)]
        public string Name { get; set; }

        [Required, MaxLength(300)]
        public string Description { get; set; }
        public byte[] img1 { get; set; }
        public byte[] img2 { get; set; }
        public byte[] img3 { get; set; }
        public byte[] img4 { get; set; }
        public byte[] img5 { get; set; }
        public byte[] img6 { get; set; }
        public float Rate { get; set; }
        
        [ForeignKey("Category")]
        public int CatId { get; set; }
        public Category Category { get; set; }
        
        [ForeignKey("UpholstertMat")]
        public int Upholstery_mat_no { get; set; }
        public UpholsteryMat UpholsteryMat { get; set; }
        [ForeignKey("FrameMateNo")]
        public int FrameMateNo { get; set; }
        public List<OrderedItemsArchive> orderedItemsArchives { get; set; }
        public List<OrderdItem> orderdItems { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public float Depth { get; set; }



    }
}
