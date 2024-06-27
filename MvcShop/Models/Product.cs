using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcShop.Models;

public class Product
{
    public int? Id { get; set; }
    [ForeignKey("CategoryId")]
    public int? CategoryId { get; set; }
    [Display(Name = "Product Name")]
    public string? Product_Name { get; set; }
    public string? Color { get; set; }
    public string? Description { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }
    public string? Image { get; set; }

}

