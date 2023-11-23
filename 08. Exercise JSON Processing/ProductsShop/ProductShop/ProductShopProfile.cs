using ProductShop.DTOs.Export;

namespace ProductShop
{
    using AutoMapper;

    using Models;
    using DTOs.Import;
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            // User
            this.CreateMap<ImportUserDto, User>();
            this.CreateMap<User, ExportSoldProductDto>()
                .ForMember(d => d.SellerFirstName, opt => opt.MapFrom(s => s.FirstName))
                .ForMember(d => d.SellerLastName, opt => opt.MapFrom(s => s.LastName))
                .ForMember(d => d.SoldProducts, opt => opt.MapFrom(s => s.ProductsSold));
            this.CreateMap<User, ExportUserDto>()
                .ForMember(d => d.LastName, opt => opt.MapFrom(s => s.LastName))
                .ForMember(d => d.Age, opt => opt.MapFrom(s => s.Age))
                .ForMember(d => d.SoldProducts, opt => opt.MapFrom(s => s.ProductsSold));
            this.CreateMap<User, ExportUserProductDto>()
                .ForMember(d => d.Count, opt => opt.MapFrom(s => s.ProductsSold.Count))
                .ForMember(d => d.Products, opt => opt.MapFrom(s => s.ProductsSold));
            this.CreateMap<User, ExportObjectUserDto>();
            // Product
            this.CreateMap<ImportProductsDto, Product>();
            this.CreateMap<Product, ExportProductInRangeDto>()
                .ForMember(d => d.ProductName, opt => opt.MapFrom(d => d.Name))
                .ForMember(s => s.ProductPrice, opt => opt.MapFrom(d => d.Price))
                .ForMember(s => s.SellerName, opt => opt.MapFrom(s => $"{s.Seller.FirstName} {s.Seller.LastName}"));
            this.CreateMap<Product, ExportProductDto>()
                .ForMember(d => d.BuyerFirstName, opt => opt.MapFrom(s => s.Buyer.FirstName))
                .ForMember(d => d.BuyerLastName, opt => opt.MapFrom(s => s.Buyer!.LastName))
                .ForMember(d => d.ProductPrice, opt => opt.MapFrom(s => s.Price))
                .ForMember(d => d.ProductName, opt => opt.MapFrom(s => s.Name));
            this.CreateMap<Product, ExportProductInfoDto>()
                .ForMember(d => d.ProductName, opt => opt.MapFrom(s => s.Name))
                .ForMember(d => d.ProductPrice, opt => opt.MapFrom(s => s.Price));
            // Category
            this.CreateMap<ImportCategoriesDto, Category>();
            this.CreateMap<Category, ExportCategoriesProductsDto>()
                .ForMember(d => d.CategoryName, opt => opt.MapFrom(s => s.Name))
                .ForMember(d => d.ProductCount, opt => opt.MapFrom(s => s.CategoriesProducts.Count))
                .ForMember(d => d.AveragePrice,
                    opt => opt.MapFrom(s => Math.Round((double)s.CategoriesProducts.Average(cp => cp.Product.Price), 2)))
                .ForMember(d => d.TotalRevenue, opt => opt.MapFrom(s => s.CategoriesProducts.Sum(cp => cp.Product.Price)));
            // ProductCategory
            this.CreateMap<ImportCategoryProductsDto, CategoryProduct>();

        }
    }
}
