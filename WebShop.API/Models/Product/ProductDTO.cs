using WebShop.API.Models.Category;

namespace WebShop.API.Models.Product
{
    public class ProductDTO : CreateProductDTO
    {
        public int Id { get; set; }
        public CategoryDTO Category { get; set; }
    }

}
