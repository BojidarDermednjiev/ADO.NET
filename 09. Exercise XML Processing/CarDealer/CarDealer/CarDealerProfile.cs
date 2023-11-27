using System.Globalization;

namespace CarDealer
{
    using AutoMapper;

    using Models;
    using DTOs.Import;

    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            // Car
            this.CreateMap<ImportCarDto, Car>()
                .ForSourceMember(s => s.Parts, opt => opt.DoNotValidate());
            //.ForMember(d => d.PartsCars, opt => opt.MapFrom(s => s.Parts.Select(p => new PartCar() {PartId = p.PartId})));
            // Customer
            this.CreateMap<ImportCustomerDto, Customer>()
                .ForMember(d => d.BirthDate, opt => opt.MapFrom(s => DateTime.Parse(s.BirthDate, CultureInfo.InvariantCulture)));
            // Part
            this.CreateMap<ImportPartDto, Part>()
                .ForMember(d => d.SupplierId, opt => opt.MapFrom(s => s.SupplierId!.Value));
            // PartCar

            //this.CreateMap<ImportCarPartDto, PartCar>();

            // Sale
            this.CreateMap<ImportSaleDto, Sale>()
                .ForMember(d => d.CarId, opt => opt.MapFrom(s => s.CarId.Value))
                .ForMember(d => d.Discount, opt => opt.MapFrom(s => s.Discount));
            // Supplier
            this.CreateMap<ImportSupplierDto, Supplier>();
        }
    }
}
