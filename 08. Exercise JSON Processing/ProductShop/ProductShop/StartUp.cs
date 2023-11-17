namespace ProductShop
{
    using AutoMapper;
    using Newtonsoft.Json;

    using Data;
    using Models;
    using DTOs.Import;

    public class StartUp
    {
        public static void Main()
        {
            ProductShopProfile context = new ProductShopProfile();
            string output = File.ReadAllText(@"../../../");
        }

        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            IMapper mapper = new Mapper(new MapperConfiguration(cfg =>
             {
                 cfg.AddProfile<ProductShopProfile>();
             }));
            var userDtos = JsonConvert.DeserializeObject<ImportUserDto[]>(inputJson);
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
    }
}