using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace MvcShop.Models
{
    public class Field
    {
        public int? Id { get; set; }

        public required string Name { get; set; }

    }
}
