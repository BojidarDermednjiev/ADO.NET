
namespace ProductShop
{
    using AutoMapper;

    using Models;
    using DTOs.Import;
    using DTOs.Export;

    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            this.CreateMap<ImportUserDto, User>();
            this.CreateMap<ImportProductsDto, Product>();
            this.CreateMap<Product, ExportProductInRangeDto>()
                .ForMember(d => d.ProductName, opt => opt.MapFrom(s => s.Name))
                .ForMember(d => d.ProductPrice, opt => opt.MapFrom(s => s.Price))
                .ForMember(d => d.SellerName, opt => opt.MapFrom(s => $"{s.Seller.FirstName} {s.Seller.LastName}"));
            this.CreateMap<ImportCategoriesDto, Category>();
            this.CreateMap<ImportCategoryProductsDto, CategoryProduct>();
        }
    }
}
