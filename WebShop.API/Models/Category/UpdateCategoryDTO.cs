using System.Collections.Generic;
using WebShop.API.Models.Product;

namespace WebShop.API.Models.Category
{
    public class UpdateCategoryDTO : CreateCategoryDTO
    {
        public IList<CreateProductDTO> Products { get; set; }
    }
}