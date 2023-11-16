namespace FastFood.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using ViewModels.Items;
    using FastFood.Services.Data;



    public class ItemsController : Controller
    {
        private readonly IItemService itemService;

        public ItemsController(IItemService itemService)
        {
            this.itemService = itemService;
        }

        public async Task<IActionResult> Create()
        {
            IEnumerable<CreateItemViewModel> availableCategories = await this.itemService.GetAllAvailableCategoriesAsync();
            return View(availableCategories);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateItemInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return RedirectToAction("Error", "Home");
            }

            await this.itemService.CreateAsync(model);
            return this.RedirectToAction("All");
        }

        public async Task<IActionResult> All()
        {
            IEnumerable<ItemsAllViewModels> items = 
                await this.itemService.GetAllAsync();
            return View(items.ToList());
        }
    }
}
