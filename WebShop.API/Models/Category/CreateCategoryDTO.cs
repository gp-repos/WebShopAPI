using System.ComponentModel.DataAnnotations;

namespace WebShop.API.Models.Category
{
    public class CreateCategoryDTO
    {
        [Required]
        [StringLength(maximumLength: 50, ErrorMessage = "Category Name Is Too Long")]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public string ImageName { get; set; }
    }
}