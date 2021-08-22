using System.Collections.Generic;
using WebShop.API.Models.V2.Product;

namespace WebShop.API.Models.V2.Category
{
    public class UpdateCategoryV2DTO : CreateCategoryV2DTO
    {
        public IList<CreateProductV2DTO> Products { get; set; }
    }
}