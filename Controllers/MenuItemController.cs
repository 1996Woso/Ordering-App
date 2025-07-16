using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Ordering_App.Interfaces;
using Ordering_App.Models;
using Ordering_App.Models.DTOs;
using Ordering_App.Repositories;

namespace Ordering_App.Controllers
{
    public class MenuItemController : Controller
    {
        private readonly IMenuItemRepository menuItemRepository;

        public MenuItemController(IMenuItemRepository menuItemRepository)
        {
            this.menuItemRepository = menuItemRepository;
        }
        [HttpGet]
        public async Task<IActionResult> MenuItems([FromQuery] UserParams userParams
            , decimal minPrice, decimal maxPrice, string restaurantName, string memuItemName)
        {
            if (minPrice >= 0) userParams.MinPrice = minPrice;
            if (maxPrice != 0 && maxPrice != userParams.MaxPrice) userParams.MaxPrice = maxPrice;
            userParams.PageSize = 1;
            if (!restaurantName.IsNullOrEmpty())
            {
                userParams.RestaurantName = restaurantName;
            }
            var items = await menuItemRepository.GetAllAsync(userParams);
            ViewBag.TotalCount = items.TotalCount;
            ViewBag.TotalPages = items.TotalPages;
            ViewBag.CurrentPage = userParams.PageNumber;
            ViewBag.MinPrice = userParams.MinPrice;
            ViewBag.MaxPrice = userParams.MaxPrice;
            ViewBag.RestaurantName = userParams.RestaurantName;
            ViewBag.MemuItemName = memuItemName;
            return View(items);
        }
        public IActionResult AddMenuItem(int id)
        {
            TempData["RestaurantId"] = id;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddMenuItem(AddMenuItemDTO addMenuItemDTO, int id)
        {
            var menuITemDM = await menuItemRepository.CreateAsync(addMenuItemDTO);
            menuITemDM.RestaurantId = id;
            string errorMessage ="",successMessage = "";
            if (await menuItemRepository.ItemExistsAsync(menuITemDM))
            {
                errorMessage = $"{menuITemDM.Name.ToUpper()} at {menuITemDM.Restaurant!.Name!} allready exists.";
            }
            
            if (await menuItemRepository.SaveAllAsync())
            {
                successMessage = $"{addMenuItemDTO.Name.ToUpper()} has been successful created.";
            }
            TempData["Error"] = errorMessage;
            TempData["Success"] = successMessage;
            return View();
        }
        
    }
}
