using System.Collections.Generic;
using WebShop.API.Models.Product;

namespace WebShop.API.Models.Category
{
    public class CategoryDTO : CreateCategoryDTO
    {
        public int Id { get; set; }
        public IList<ProductDTO> Products { get; set; }
    }

}
