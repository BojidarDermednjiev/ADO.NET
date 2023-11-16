
namespace FastFood.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using ViewModels.Categories;
    using FastFood.Services.Data;


    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            this._categoryService = categoryService;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction("Error", "Home");
            }

            await this._categoryService.CreateAsync(model);
            return this.RedirectToAction("All");
        }

        public async Task<IActionResult> All()
        {
            IEnumerable<CategoryAllViewModel> categories = await this._categoryService.GetAllAsync();
            return View(categories.ToList());
        }
    }
}
