using AutoMapper;
using UserManagement.Core.Domain.Entities;
using WebShop.API.Models.Category;
using WebShop.API.Models.Product;
using WebShop.API.Models.User;
using WebShop.API.Models.V2.Category;
using WebShop.API.Models.V2.Product;
using WebShop.Core.Domain.Entities;

namespace WebShop.API.Configurations
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<Category, CreateCategoryDTO>().ReverseMap();
            CreateMap<Category, UpdateCategoryDTO>().ReverseMap();
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Product, CreateProductDTO>().ReverseMap();
            CreateMap<Product, UpdateProductDTO>().ReverseMap();

            CreateMap<AppUser, UserDTO>().ReverseMap();

            CreateMap<Category, CategoryV2DTO>().ReverseMap();
            CreateMap<Category, CreateCategoryV2DTO>().ReverseMap();
            CreateMap<Category, UpdateCategoryV2DTO>().ReverseMap();
            CreateMap<Product, ProductV2DTO>().ReverseMap();
            CreateMap<Product, CreateProductV2DTO>().ReverseMap();
            CreateMap<Product, UpdateProductV2DTO>().ReverseMap();
        }
    }
}
