using AutoMapper.QueryableExtensions;
using CarDealerSystem.DTOs.Export;
using CarDealerSystem.DTOs.Import;
using CarDealerSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CarDealerSystem
{
    using AutoMapper;
    using Data;
    using Newtonsoft.Json;

    public class StartUp
    {
        static void Main()
        {
            CarDealerContext context = new CarDealerContext();
            //string inputJSON = File.ReadAllText(@"../../../Datasets/sales.json");
            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();
            string output = GetOrderedCustomers(context);
            Console.WriteLine(output);
        }
        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();
            ImportSupplierDto[] supplierDtos = JsonConvert.DeserializeObject<ImportSupplierDto[]>(inputJson);
            ICollection<Supplier> suppliers = new HashSet<Supplier>();
            foreach (var supplierDto in supplierDtos)
            {
                if (string.IsNullOrEmpty(supplierDto.Name)) continue;
                Supplier supplier = mapper.Map<Supplier>(supplierDto);
                suppliers.Add(supplier);
            }
            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();
            return $"Successfully imported {suppliers.Count}.";
        }
        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();
            ImportPartDto[] partDtos = JsonConvert.DeserializeObject<ImportPartDto[]>(inputJson);
            ICollection<Part> parts = new HashSet<Part>();
            foreach (var importPartDto in partDtos)
            {
                if (string.IsNullOrEmpty(importPartDto.Name)) continue;
                if (!context.Suppliers.Any(s => s.Id == importPartDto.SupplierId)) continue;
                Part part = mapper.Map<Part>(importPartDto);
                parts.Add(part);
            }

            context.Parts.AddRange(parts);
            context.SaveChanges();
            return $"Successfully imported {parts.Count}.";
        }
        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();

            ImportCarDto[] carDtos = JsonConvert.DeserializeObject<ImportCarDto[]>(inputJson);
            ICollection<Car> cars = new HashSet<Car>();
            foreach (var carDto in carDtos)
            {
                if (string.IsNullOrEmpty(carDto.Make) || string.IsNullOrEmpty(carDto.Model)) continue;
                Car car = mapper.Map<Car>(carDto);
                foreach (var partId in carDto.PartsId.Distinct())
                {
                    context.PartsCars.Add(new PartCar
                    {
                        PartId = partId,
                        Car = car
                    });
                }
                cars.Add(car);
            }
            context.Cars.AddRange(cars);
            context.SaveChanges();
            return $"Successfully imported {cars.Count}.";
        }
        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();
            ImportCustomerDto[] customerDtos = JsonConvert.DeserializeObject<ImportCustomerDto[]>(inputJson);
            ICollection<Customer> customers = new HashSet<Customer>();
            foreach (var importCustomerDto in customerDtos)
            {
                if (string.IsNullOrEmpty(importCustomerDto.Name)) continue; ;
                Customer customer = mapper.Map<Customer>(importCustomerDto);
                customers.Add(customer);
            }
            context.Customers.AddRange(customers);
            context.SaveChanges();
            return $"Successfully imported {customers.Count}.";

        }
        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();
            ImportSaleDto[] saleDtos = JsonConvert.DeserializeObject<ImportSaleDto[]>(inputJson);
            ICollection<Sale> sales = new HashSet<Sale>();
            foreach (var saleDto in saleDtos)
            {
                Sale sale = mapper.Map<Sale>(saleDto);
                sales.Add(sale);
            }
            context.Sales.AddRange(sales);
            context.SaveChanges();
            return $"Successfully imported {sales.Count}.";
        }

        public static string GetOrderedCustomers(CarDealerContext context)
        {
            IMapper mapper = CreateMapper();
            ExportOrderedCustomersDto[] customersDtos = context.Customers
                .OrderBy(c => c.BirthDate)
                .ThenByDescending(c => c.BirthDate)
                .AsNoTracking()
                .ProjectTo<ExportOrderedCustomersDto>(mapper.ConfigurationProvider)
                .ToArray();

            return JsonConvert.SerializeObject(customersDtos, Formatting.Indented);
        }

        //public static string GetCarsFromMakeToyota(CarDealerContext context)
        //{
        //}

        //public static string GetLocalSuppliers(CarDealerContext context)
        //{
        //}

        //public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        //{
        //}

        //public static string GetTotalSalesByCustomer(CarDealerContext context)
        //{
        //}

        //public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        //{
        //}
        private static IMapper CreateMapper()
            => new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            }));
    }
}
