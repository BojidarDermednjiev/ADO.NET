namespace FastFood.Services.Mapping
{
    using AutoMapper;

    using Models;
    using Web.ViewModels.Items;
    using Web.ViewModels.Positions;
    using Web.ViewModels.Categories;


    public class FastFoodProfile : Profile
    {
        public FastFoodProfile()
        {
            //Positions
            CreateMap<CreatePositionInputModel, Position>()
                .ForMember(x => x.Name, y => y.MapFrom(s => s.PositionName));
            
            CreateMap<Position, PositionsAllViewModel>()
                .ForMember(x => x.Name, y => y.MapFrom(s => s.Name));

            this.CreateMap<CreateCategoryInputModel, Category>()
                .ForMember(m => m.Name, y => y.MapFrom(m => m.CategoryName));

            this.CreateMap<Category, CategoryAllViewModel>();

            this.CreateMap<Category, CreateItemViewModel>()
                .ForMember(d => d.CategoryId, opt => opt.MapFrom(src => src.Id))
                .ForMember(d => d.CategoryName, opt => opt.MapFrom(s => s.Name));

            this.CreateMap<CreateItemInputModel, Item>();

            this.CreateMap<Item, ItemsAllViewModels>()
                .ForMember(d => d.Category, opt => opt.MapFrom(s => s.Category.Name));

        }
    }
}
