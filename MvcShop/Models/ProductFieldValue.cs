using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace MvcShop.Models
{
    public class ProductFieldValue
    {
        public int? Id { get; set; }

        [ForeignKey("ProductId")]
        public int? ProductId { get; set; }
        [ForeignKey("FieldId")]
        public int? FieldId { get; set; }

        public required string Value { get; set; }

    }
}