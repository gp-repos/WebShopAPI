using System.ComponentModel.DataAnnotations;

namespace WebShop.API.Models.V2.Product
{
    public class CreateProductV2DTO
    {
        [Required]
        [StringLength(maximumLength: 150, ErrorMessage = "Product Name Is Too Long")]
        public string Name { get; set; }

        [Required]
        [StringLength(maximumLength: 250, ErrorMessage = "Excerpt Is Too Long")]
        public string Excerpt { get; set; }
        
        [Required]
        public string Description { get; set; }
        
        public string ImageFile { get; set; }

        [Required]
        [Range(1, 100000)]
        public decimal Price { get; set; }

        public int CategoryId { get; set; }

    }
}