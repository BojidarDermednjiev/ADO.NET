using FastFood.Services.Data;
using Microsoft.AspNetCore.Authorization;

namespace FastFood.Web.Controllers
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.AspNetCore.Mvc;

    using Data;
    using Models;
    using ViewModels.Positions;

    public class PositionsController : Controller
    {
        private readonly IPositionService _positionService;

        public PositionsController(IPositionService positionService)
        {
            this._positionService = positionService;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePositionInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Error", "Home");
            }

            await this._positionService.CreateAsync(model);

            return RedirectToAction("All", "Positions");
        }

       // [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> All()
        {
            IEnumerable<PositionsAllViewModel> positions = await this._positionService.GetAllAsync();

            return View(positions.ToList());
        }
    }
}
