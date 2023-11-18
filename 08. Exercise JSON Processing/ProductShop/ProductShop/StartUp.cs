using Newtonsoft.Json.Serialization;

namespace ProductShop
{
    using AutoMapper;
    using Newtonsoft.Json;
    using Microsoft.EntityFrameworkCore;
    using AutoMapper.QueryableExtensions;

    using Data;
    using Models;
    using DTOs.Import;
    using DTOs.Export;
    public class StartUp
    {
        public static void Main()
        {
            ProductShopContext context = new ProductShopContext();
            //string inputJson = File.ReadAllText(@"../../../Datasets/categories-products.json");
            string output = GetUsersWithProducts(context);
            Console.WriteLine(output);
        }

        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();
            var userDtos = JsonConvert.DeserializeObject<ImportUserDto[]>(inputJson);

            //User[] users = mapper.Map<User[]>(userDtos);

            ICollection<User> validUser = new HashSet<User>();
            foreach (var userDto in userDtos)
            {
                User user = mapper.Map<User>(userDto);
                validUser.Add(user);
            }
            context.Users.AddRange(validUser);
            context.SaveChanges();
            return $"Successfully imported {validUser.Count}";
        }
        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();
            var productDtos = JsonConvert.DeserializeObject<ImportProductsDto[]>(inputJson);
            var products = mapper.Map<Product[]>(productDtos);
            context.Products.AddRange(products);
            context.SaveChanges();
            return $"Successfully imported {products.Length}";
        }
        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();
            var categoryDtos = JsonConvert.DeserializeObject<ImportCategoriesDto[]>(inputJson);
            ICollection<Category> validCategories = new HashSet<Category>();
            foreach (var categoryDto in categoryDtos)
            {
                if (string.IsNullOrEmpty(categoryDto.Name))
                    continue;

                Category category = mapper.Map<Category>(categoryDto);
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
            //var products = context.Products
            //    .Where(p => p.Price >= 500 && p.Price <= 1000)
            //    .OrderBy(p => p.Price)
            //    .Select(p => new
            //    {
            //        name = p.Name,
            //        price = p.Price,
            //        seller = p.Seller.FirstName + " " + p.Seller.LastName
            //    })
            //    .AsNoTracking()
            //    .ToArray();
            IMapper mapper = CreateMapper();
            var productsDtos = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .AsNoTracking()
                .ProjectTo<ExportProductInRangeDto>(mapper.ConfigurationProvider)
                .ToArray();

            return JsonConvert.SerializeObject(productsDtos, Formatting.Indented);
        }
        public static string GetSoldProducts(ProductShopContext context)
        {
            //IMapper mapper = CreateMapper();
            IContractResolver contractResolver = ConfigCamelCaseNamingStrategy();
            var userWithSoldProducts = context.Users
                .Where(u => u.ProductsSold.Any(p => p.Buyer != null))
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Select(u => new
                {
                    u.FirstName,
                    u.LastName,
                    SoldProducts = u.ProductsSold
                        .Where(p => p.Buyer != null)
                        .Select(p => new
                        {
                            p.Name,
                            p.Price,
                            BuyerFirstName = p.Buyer!.FirstName,
                            BuyerLastName = p.Buyer.LastName

                        })
                        .ToArray()
                })
                .AsNoTracking()
                .ToArray();

            return JsonConvert.SerializeObject(userWithSoldProducts, Formatting.Indented, new JsonSerializerSettings()
            {
                ContractResolver = contractResolver
            });
        }
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            IContractResolver contractResolver = ConfigCamelCaseNamingStrategy();

            var category = context.Categories
                .OrderByDescending(p => p.CategoriesProducts.Count)
                .Select(c => new
                {
                    Category = c.Name,
                    ProductCount = c.CategoriesProducts.Count,
                    AvaragePrice = (c.CategoriesProducts.Any() ? c.CategoriesProducts.Average(cp => cp.Product.Price) : 0).ToString("F2"),
                    TotalRevenue = (c.CategoriesProducts.Any()? c.CategoriesProducts.Sum(cp => cp.Product.Price) : 0).ToString("F2")
                })
                .AsNoTracking()
                .ToArray();
            return JsonConvert.SerializeObject(category, Formatting.Indented, new JsonSerializerSettings()
            {
                ContractResolver = contractResolver
            });
        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            IContractResolver contractResolver = ConfigCamelCaseNamingStrategy();


            var users = context.Users
                .Where(u => u.ProductsSold.Any(p => p.Buyer != null))
                .Select(u => new
                {
                    u.FirstName,
                    u.LastName,
                    u.Age,
                    SoldProducts = new
                    {
                        Count = u.ProductsSold.Count(p => p.Buyer != null),
                        Products = u.ProductsSold.Where(p => p.Buyer != null)
                            .Select(p => new
                            {
                                p.Name,
                                p.Price
                            })
                            .ToArray()
                    }
                })
                .OrderByDescending(u => u.SoldProducts.Count)
                .AsNoTracking()
                .ToArray();
            var userWrapperDto = new
            {
                UserCount = users.Length,
                Users = users
            };
            return JsonConvert.SerializeObject(userWrapperDto, Formatting.Indented, new JsonSerializerSettings()
            {
                ContractResolver = contractResolver,
                NullValueHandling = NullValueHandling.Ignore
            });
        }

        private static IMapper CreateMapper()
            => new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            }));
        private static IContractResolver ConfigCamelCaseNamingStrategy()
            => new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy(false, true)
            };
    }
}