namespace ProductShop
{
    using AutoMapper;
    using Newtonsoft.Json;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json.Serialization;
    using AutoMapper.QueryableExtensions;


    using Data;
    using Models;
    using DTOs.Import;
    using DTOs.Export;

    public class StartUp
    {
        static void Main(string[] args)
        {
            ProductShopContext context = new ProductShopContext();
            // string inputJSON = File.ReadAllText(@"../../../Datasets/categories-products.json");
            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();
            string output = GetSoldProducts(context);
            Console.WriteLine(output);
        }

        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            ImportUserDto[] userDtos = JsonConvert.DeserializeObject<ImportUserDto[]>(inputJson);
            IMapper mapper = CreateMapper();
            ICollection<User> validUsers = new HashSet<User>();
            foreach (var importUserDto in userDtos)
            {
                User user = mapper.Map<User>(importUserDto);
                validUsers.Add(user);
            }
            context.Users.AddRange(validUsers);
            context.SaveChanges();
            return $"Successfully imported {validUsers.Count}";
        }

        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            ImportProductsDto[] productDtos = JsonConvert.DeserializeObject<ImportProductsDto[]>(inputJson);
            IMapper mapper = CreateMapper();
            ICollection<Product> validProducts = new HashSet<Product>();
            foreach (var importProductsDto in productDtos)
            {
                Product product = mapper.Map<Product>(importProductsDto);
                validProducts.Add(product);
            }

            context.Products.AddRange(validProducts);
            context.SaveChanges();
            return $"Successfully imported {validProducts.Count}";

            //IMapper mapper = CreateMapper();
            //ImportProductsDto[] productDtos = JsonConvert.DeserializeObject<ImportProductsDto[]>(inputJson);
            //var products = mapper.Map<Product[]>(productDtos);
            //context.Products.AddRange(products);
            //context.SaveChanges();
            //return $"Successfully imported {products.Length}";
        }

        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            ImportCategoriesDto[] categoriesDto = JsonConvert.DeserializeObject<ImportCategoriesDto[]>(inputJson);
            IMapper mapper = CreateMapper();
            ICollection<Category> validCategories = new HashSet<Category>();
            foreach (var importCategoriesDto in categoriesDto)
            {
                if (string.IsNullOrEmpty(importCategoriesDto.Name)) continue;
                Category category = mapper.Map<Category>(importCategoriesDto);
                validCategories.Add(category);

            }

            context.Categories.AddRange(validCategories);
            context.SaveChanges();
            return $"Successfully imported {validCategories.Count}";
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();
            ImportCategoryProductsDto[] categoryProducts = JsonConvert.DeserializeObject<ImportCategoryProductsDto[]>(inputJson);
            ICollection<CategoryProduct> validEntities = new HashSet<CategoryProduct>();
            foreach (var cpDto in categoryProducts)
            {
                if (!context.Categories.Any(c => c.Id == cpDto.CategoryId) ||
                    !context.Products.Any(p => p.Id == cpDto.ProductId))
                    continue;

                CategoryProduct category = mapper.Map<CategoryProduct>(cpDto);
                validEntities.Add(category);
            }
            context.CategoriesProducts.AddRange(validEntities);
            context.SaveChanges();
            return $"Successfully imported {validEntities.Count}";
        }
        public static string GetProductsInRange(ProductShopContext context)
        {
            IMapper mapper = CreateMapper();
            ExportProductInRangeDto[] productDtos = context.Products.Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price).AsNoTracking().ProjectTo<ExportProductInRangeDto>(mapper.ConfigurationProvider).ToArray();
            return JsonConvert.SerializeObject(productDtos, Formatting.Indented);
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            IMapper mapper = CreateMapper();
            ExportSoldProductDto[] soldProductDtos = context.Users
                .Where(u => u.ProductsSold.Any(p => p.Buyer != null))
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .AsNoTracking()
                .ProjectTo<ExportSoldProductDto>(mapper.ConfigurationProvider)
                .ToArray();
            return JsonConvert.SerializeObject(soldProductDtos, Formatting.Indented);
        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            IMapper mapper = CreateMapper();
            ExportCategoriesProductsDto[] categoriesProductsDtos = context.Categories
                .OrderByDescending(cp => cp.CategoriesProducts.Count)
                .AsNoTracking()
                .ProjectTo<ExportCategoriesProductsDto>(mapper.ConfigurationProvider)
                .ToArray();
            return JsonConvert.SerializeObject(categoriesProductsDtos, Formatting.Indented);
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            IMapper mapper = CreateMapper();
            //IContractResolver contractResolver = ConfigCamelCaseNamingStrategy();

            ExportUserDto[] usersDtos = context.Users
                .Where(u => u.ProductsSold.Any(p => p.Buyer != null))
                .OrderByDescending(u => u.ProductsSold.Count)
                .AsNoTracking()
                .ProjectTo<ExportUserDto>(mapper.ConfigurationProvider)
                .ToArray();

            var user = new
            {
                UserCount = usersDtos.Length,
                User = usersDtos
            };

            return JsonConvert.SerializeObject(usersDtos, Formatting.Indented, new JsonSerializerSettings()
            {
                //ContractResolver = contractResolver,
                NullValueHandling = NullValueHandling.Ignore
            });
        }

        private static IMapper CreateMapper()
            => new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            }));

        //private static IContractResolver ConfigCamelCaseNamingStrategy()
        //    => new DefaultContractResolver()
        //    {
        //        NamingStrategy = new CamelCaseNamingStrategy(false, true)
        //    };
    }
}
