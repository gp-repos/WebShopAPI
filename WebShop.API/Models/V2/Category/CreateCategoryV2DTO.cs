using System.ComponentModel.DataAnnotations;

namespace WebShop.API.Models.V2.Category
{
    public class CreateCategoryV2DTO
    {
        [Required]
        [StringLength(maximumLength: 50, ErrorMessage = "Category Name Is Too Long")]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public string ImageName { get; set; }

    }
}