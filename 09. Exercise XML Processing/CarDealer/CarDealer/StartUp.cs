namespace CarDealer
{
    using Data;
    using AutoMapper;

    using Models;
    using Utilities;
    using DTOs.Import;

    public class StartUp
    {
        public static void Main()
        {
            CarDealerContext context = new CarDealerContext();
            string XmlReader = File.ReadAllText(@"../../../Datasets/sales.xml");
            string output = ImportSales(context, XmlReader);
            Console.WriteLine(output);

            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();

        }

        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            XmlHelper xmlHelper = new XmlHelper();
            ImportSupplierDto[] importSupplierDtos = xmlHelper.Deserialize<ImportSupplierDto[]>(inputXml, "Suppliers");
            IMapper mapper = InitializeAutoMapper();
            ICollection<Supplier> suppliers = new HashSet<Supplier>();
            foreach (var importSupplierDto in importSupplierDtos)
            {
                if (string.IsNullOrEmpty(importSupplierDto.Name)) continue;
                Supplier supplier = mapper.Map<Supplier>(importSupplierDto);
                suppliers.Add(supplier);
            }
            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();
            return $"Successfully imported {suppliers.Count}";
        }
        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            IMapper mapper = InitializeAutoMapper();
            XmlHelper xmlHelper = new XmlHelper();
            ImportPartDto[] partDtos = xmlHelper.Deserialize<ImportPartDto[]>(inputXml, "Parts");
            ICollection<Part> parts = new HashSet<Part>();
            foreach (var importPartDto in partDtos)
            {
                if (string.IsNullOrEmpty(importPartDto.Name) ||
                   !importPartDto.SupplierId.HasValue ||
                   !context.Suppliers.Any(s => s.Id == importPartDto.SupplierId)) continue;
                Part part = mapper.Map<Part>(importPartDto);
                parts.Add(part);
            }
            context.Parts.AddRange(parts);
            context.SaveChanges();
            return $"Successfully imported {parts.Count}";
        }
        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            IMapper mapper = InitializeAutoMapper();
            XmlHelper xmlHelper = new XmlHelper();
            ImportCarDto[] carDto = xmlHelper.Deserialize<ImportCarDto[]>(inputXml, "Cars");
            ICollection<Car> cars = new HashSet<Car>();
            foreach (var importCarDto in carDto)
            {
                if (string.IsNullOrEmpty(importCarDto.Make) || string.IsNullOrEmpty(importCarDto.Model)) continue;
                Car car = mapper.Map<Car>(importCarDto);
                foreach (var partCarDto in importCarDto.Parts.DistinctBy(p => p.PartId))
                {
                    if(!context.Parts.Any(p => p.Id == partCarDto.PartId)) continue;
                    PartCar partCar = new PartCar()
                    {
                        PartId = partCarDto.PartId
                    };
                    car.PartsCars.Add(partCar);
                }
                cars.Add(car);
            }

            context.Cars.AddRange(cars);
            context.SaveChanges();
            return $"Successfully imported {cars.Count}"; ;
        }
        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            IMapper mapper = InitializeAutoMapper();
            XmlHelper xmlHelper = new XmlHelper();
            ImportCustomerDto[] customerDto = xmlHelper.Deserialize<ImportCustomerDto[]>(inputXml, "Customers");
            ICollection<Customer> customers = new HashSet<Customer>();
            foreach (var importCustomerDto in customerDto)
            {
                if(string.IsNullOrEmpty(importCustomerDto.Name) || string.IsNullOrEmpty(importCustomerDto.BirthDate)) continue;
                Customer customer = mapper.Map<Customer>(importCustomerDto);
                customers.Add(customer);
            }
            context.Customers.AddRange(customers);
            context.SaveChanges();
            return $"Successfully imported {customers.Count}";
        }
        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            IMapper mapper = InitializeAutoMapper();
            XmlHelper xmlHelper = new XmlHelper();
            ImportSaleDto[] saleDtos = xmlHelper.Deserialize<ImportSaleDto[]>(inputXml, "Sales");
            ICollection<Sale> sales = new HashSet<Sale>();
            foreach (var importSaleDto in saleDtos)
            {
                if(!importSaleDto.CarId.HasValue || !context.Cars.Any(c => c.Id == importSaleDto.CarId)) continue;
                Sale sale = mapper.Map<Sale>(importSaleDto);
                sales.Add(sale);
            }
            context.Sales.AddRange(sales);
            context.SaveChanges();
            return $"Successfully imported {sales.Count}";
        }

        //    public static string GetCarsWithDistance(CarDealerContext context)
        //    {
        //    }

        //    public static string GetCarsFromMakeBmw(CarDealerContext context)
        //    {
        //    }

        //    public static string GetLocalSuppliers(CarDealerContext context)
        //    {
        //    }
        //    public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        //    {
        //    }

        //    public static string GetTotalSalesByCustomer(CarDealerContext context)
        //    {
        //    }

        //    public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        //    {
        //    }

        private static IMapper InitializeAutoMapper()
            => new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            }));
    }
}