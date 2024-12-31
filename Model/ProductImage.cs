using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Model
{
    public class ProductImage
    {
        [Key] public int Id { get; set; }
        public int ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]public Products Product { get; set; }
        public string ImageUrl { get; set; }
    }
}
