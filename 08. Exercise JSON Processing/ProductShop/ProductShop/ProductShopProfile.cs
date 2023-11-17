namespace ProductShop
{
    using AutoMapper;

    using Models;
    using DTOs.Import;
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            this.CreateMap<ImportUserDto, User>();
        }
    }
}
