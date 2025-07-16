using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Ordering_App.Interfaces;
using Ordering_App.Models;
using Ordering_App.Models.Domain;
using Ordering_App.Models.DTOs;
using Ordering_App.Repositories;

namespace Ordering_App.Controllers
{
    public class RestaurantController : Controller
    {
        private readonly IRestaurantRepository restaurantRepository;
        private readonly IMapper mapper;

        public RestaurantController(IRestaurantRepository restaurantRepository, IMapper mapper)
        {
            this.restaurantRepository = restaurantRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> Restaurants([FromQuery] UserParams userParams)
        {
            userParams.PageSize = 1;
            var restaurants = await restaurantRepository.GetAllAsync(userParams);
            ViewBag.TotalPages = restaurants.TotalPages;
            ViewBag.CurrentPage = userParams.PageNumber;
            return View(restaurants);
        }
        public IActionResult AddRestaurant()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddRestaurant(AddRestaurantDTO addRestaurantDTO)
        {
            string errorMessage ="",successMessage = "";
            if (await restaurantRepository.RestaurantExistsAsync(addRestaurantDTO))
            {
                errorMessage = $"{addRestaurantDTO.Name.ToUpper()} at {addRestaurantDTO.LocationDescription} allready exists.";
            }
            else
            {
                await restaurantRepository.CreateAsync(addRestaurantDTO);
                if (await restaurantRepository.SaveAllAsync())
                {
                    successMessage = $"{addRestaurantDTO.Name.ToUpper()} has been successful created.";
                }
            }
            TempData["Error"] = errorMessage;
            TempData["Success"] = successMessage;
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> RestaurantsDetails(int id, [FromQuery] UserParams userParams
        ,string menuItemName,decimal minPrice, decimal maxPrice, string menuItemDescription)
        {
            // var items = await restaurantRepository.GetByIdAsync(id);
            // return View(items);
            
            var restaurants = await restaurantRepository.GetPagedByIdAsync(userParams, id);
            var restaurant = restaurants.FirstOrDefault();

            var filteredItems = restaurant.MenuItems.AsQueryable();

            if (!string.IsNullOrEmpty(userParams.MenuItemName))
                filteredItems = filteredItems.Where(m => m.Name.ToLower().Contains(userParams.MenuItemName.ToLower()));

            if (!string.IsNullOrEmpty(userParams.MenuItemDescription))
                filteredItems = filteredItems.Where(m => m.Description.ToLower().Contains(userParams.MenuItemDescription.ToLower()));

            if (userParams.MinPrice > 0)
                filteredItems = filteredItems.Where(m => m.Price >= userParams.MinPrice);

            if (userParams.MaxPrice < decimal.MaxValue)
                filteredItems = filteredItems.Where(m => m.Price <= userParams.MaxPrice);

            // Overwrite items in the restaurant
            restaurant.MenuItems = filteredItems.ToList();

            ViewBag.TotalCount = restaurants.TotalCount;
            ViewBag.TotalPages = restaurants.TotalPages;
            ViewBag.CurrentPage = userParams.PageNumber;
            ViewBag.MinPrice = userParams.MinPrice;
            ViewBag.MaxPrice = userParams.MaxPrice;
            ViewBag.Description = userParams.MenuItemDescription;
            ViewBag.ItemName = userParams.MenuItemName;
            
            return View(restaurant);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var restaurant = await restaurantRepository.GetByIdAsync(id);
            if (restaurant == null)
            {
                TempData["Error"] = "Restaurant not found";
                return RedirectToAction("Restaurants");
            }
            return View(restaurant);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Restaurant restaurant)
        {
            string errorMessage = "", successMessage = "";
            if (restaurant == null)
            {
                errorMessage = "Restaurant cannot be found";
            }
            else
            {
                await restaurantRepository.UpdateAsync(restaurant);
                if (await restaurantRepository.SaveAllAsync())
                {
                    successMessage = "Restaurant has been edited successfully";
                }
            }
            TempData["Error"] = errorMessage;
            TempData["Success"] = successMessage;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            string errorMessage = "", successMessage = "";
            var restaurant = await restaurantRepository.GetByIdAsync(id);
            if (restaurant == null)
            {
                errorMessage = "Restaurant cannot be found";
            }
            else
            {
                await restaurantRepository.DeleteAsync(restaurant);
                if (await restaurantRepository.SaveAllAsync())
                {
                    successMessage = "Restaurant has been successfully deleted";
                }
                else
                {
                    errorMessage = "Something happened";
                }
            }
            TempData["Error"] = errorMessage;
            TempData["Success"] = successMessage;
            return RedirectToAction("Restaurants");
        }

    }
}
