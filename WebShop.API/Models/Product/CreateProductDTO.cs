using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShop.API.Models.Product
{
    public class CreateProductDTO
    {
        [Required]
        [StringLength(maximumLength: 150, ErrorMessage = "Product Name Is Too Long")]
        public string Name { get; set; }

        [Required]
        [StringLength(maximumLength: 250, ErrorMessage = "Excerpt Is Too Long")]
        public string Excerpt { get; set; }
        
        [Required]
        public string Description { get; set; }

        [Required]
        [Range(1, 100000)]
        public decimal Price { get; set; }

        public int CategoryId { get; set; }
    }
}