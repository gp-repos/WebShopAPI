using WebShop.API.Models.V2.Category;

namespace WebShop.API.Models.V2.Product
{
    public class ProductV2DTO : CreateProductV2DTO
    {
        public int Id { get; set; }
        public CategoryV2DTO Category { get; set; }
    }

}
