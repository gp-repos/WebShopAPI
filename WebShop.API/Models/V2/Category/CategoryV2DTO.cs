using System.Collections.Generic;
using WebShop.API.Models.V2.Product;

namespace WebShop.API.Models.V2.Category
{
    public class CategoryV2DTO : CreateCategoryV2DTO
    {
        public int Id { get; set; }
        public IList<ProductV2DTO> Products { get; set; }
    }

}
