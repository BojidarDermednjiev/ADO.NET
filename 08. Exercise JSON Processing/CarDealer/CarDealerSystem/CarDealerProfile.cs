namespace CarDealerSystem
{
    using AutoMapper;

    using Models;
    using DTOs.Import;
    using DTOs.Export;

    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            // Car
            this.CreateMap<ImportCarDto, Car>();
            // Customer
            this.CreateMap<ImportCustomerDto, Customer>();
            this.CreateMap<Customer, ExportOrderedCustomersDto>()
                .ForMember(d => d.CustomerName, opt => opt.MapFrom(s => s.Name))
                .ForMember(d => d.BirthDate, opt => opt.MapFrom(s => "dd/MM/yyyy"))
                .ForMember(d => d.IsYoungDriver, opt => opt.MapFrom(s => s.IsYoungDriver));
            // Part
            this.CreateMap<ImportPartDto, Part>();
            // Sale
            this.CreateMap<ImportSaleDto, Sale>();
            // Supplier
            this.CreateMap<ImportSupplierDto, Supplier>();
        }
    }
}
