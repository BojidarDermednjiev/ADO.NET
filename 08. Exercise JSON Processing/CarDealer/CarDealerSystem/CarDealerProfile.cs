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
            this.CreateMap<Car, ExportCarToyotaDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.Make, opt => opt.MapFrom(s => s.Make))
                .ForMember(d => d.Model, opt => opt.MapFrom(s => s.Model))
                .ForMember(d => d.TraveledDistance, opt => opt.MapFrom(s => s.TraveledDistance));
            // Customer
            this.CreateMap<ImportCustomerDto, Customer>();
            this.CreateMap<Customer, ExportOrderedCustomersDto>()
                .ForMember(d => d.CustomerName, opt => opt.MapFrom(s => s.Name))
                .ForMember(d => d.BirthDate, opt => opt.MapFrom(s => s.BirthDate.ToString("dd/MM/yyyy")))
                .ForMember(d => d.IsYoungDriver, opt => opt.MapFrom(s => s.IsYoungDriver));
            // Part
            this.CreateMap<ImportPartDto, Part>();
            // Sale
            this.CreateMap<ImportSaleDto, Sale>();
            // Supplier
            this.CreateMap<ImportSupplierDto, Supplier>();
            this.CreateMap<Supplier, ExportLocalSuppliersDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name))
                .ForMember(d => d.PartsCount, opt => opt.MapFrom(s => s.Parts.Count));

        }
    }
}
