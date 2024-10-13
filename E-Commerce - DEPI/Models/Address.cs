using E_Commerce___DEPI.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class Address
{
    public int Id { get; set; }
    [Required]
    public int? Num { get; set; }

    [MaxLength(200)]
    [Required]
    public string? Street { get; set; }

    [MaxLength(10)]
    [Required]
    public string? ZipCode { get; set; }

    // Foreign key for ShippmentCity
    public int? ShippmentCitiesId { get; set; }

    // Navigation property
    [ForeignKey("ShippmentCitiesId")]
    public virtual ShippmentCity? ShippmentCities { get; set; }

    public virtual List<Order>? Orders { get; set; }
}
