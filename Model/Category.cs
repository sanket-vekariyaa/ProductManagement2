using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Model
{
    public class Category : TransectionKeys
    {
        [Required][MaxLength(100)] public string Name { get; set; }
        public ICollection<Products> Products { get; set; }

    }
}
