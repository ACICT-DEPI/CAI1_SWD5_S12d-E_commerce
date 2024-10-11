using E_Commerce___DEPI.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class Address
{
    public int Id { get; set; }
    public int? Num { get; set; }

    [MaxLength(200)]
    public string? Street { get; set; }

    [MaxLength(10)]
    public string? ZipCode { get; set; }

    // Foreign key for ShippmentCity
    public int? ShippmentCitiesId { get; set; }

    // Navigation property
    [ForeignKey("ShippmentCitiesId")]
    public virtual ShippmentCity? ShippmentCities { get; set; }

    // Foreign key for Customer
    public int? CustomerId { get; set; }

    // Navigation property
    [ForeignKey("CustomerId")]
    public virtual Customer? Customer { get; set; }

    public virtual List<Order>? Orders { get; set; }
}
