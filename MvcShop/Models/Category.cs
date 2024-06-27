using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace MvcShop.Models
{
    public class Category
    {
        public int? Id { get; set; }

        public int ParentId { get; set; }

        [Display(Name = "Category Name")]
        public required string Category_Name { get; set; }

        //public List<Product> Products { get; set; }
    }
}
