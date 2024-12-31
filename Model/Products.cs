using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Model
{
    public class Products : TransectionKeys
    {
        [Required][MaxLength(100)] public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; }
        public string ProductCode { get; set; }
        public bool IsActive { get; set; }
        public ICollection<ProductImage> ProductImages { get; set; }
        [NotMapped] public List<Category> CategoryList { get; set; } = new List<Category>();
    }
}
