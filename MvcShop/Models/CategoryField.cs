using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcShop.Models;

public class CategoryField
{
    public int? Id { get; set; }
    [ForeignKey("CategoryId")]
    public int? CategoryId { get; set; }
    [ForeignKey("FieldId")]
    public int? FieldId { get; set; }

}
