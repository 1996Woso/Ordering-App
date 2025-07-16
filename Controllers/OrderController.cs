using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Ordering_App.Interfaces;
using Ordering_App.Models;
using Ordering_App.Models.Domain;

namespace Ordering_App.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderRepository orderRepository;
        private readonly IMenuItemRepository menuItemRepository;
        private readonly IOrderItemRepository orderItemRepository;

        public OrderController(IOrderRepository orderRepository, IMenuItemRepository menuItemRepository, IOrderItemRepository orderItemRepository)
        {
            this.orderRepository = orderRepository;
            this.menuItemRepository = menuItemRepository;
            this.orderItemRepository = orderItemRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Orders([FromQuery] UserParams userParams,
         decimal minTotalPrice, decimal maxTotalPrice, string orderDate, string orderStatus
         )
        {
            if (minTotalPrice >= 0) userParams.MinTotalPrice = minTotalPrice;
            if (maxTotalPrice != 0 && maxTotalPrice != userParams.MaxTotalPrice) userParams.MaxTotalPrice = maxTotalPrice;
            userParams.PageSize = 1;
            if (!orderDate.IsNullOrEmpty()) userParams.OrderDate = orderDate;
            if (!orderStatus.IsNullOrEmpty()) userParams.OrderStatus = orderStatus;
            var orders = await orderRepository.GetAllAsync(userParams);
            ViewBag.TotalCount = orders.TotalCount;
            ViewBag.TotalPages = orders.TotalPages;
            ViewBag.CurrentPage = userParams.PageNumber;
            ViewBag.MinTotalPrice = userParams.MinTotalPrice;
            ViewBag.MaxTotalPrice = userParams.MaxTotalPrice;
            ViewBag.OrderDate = userParams.OrderDate;
            ViewBag.OrderStatus = userParams.OrderStatus;
            return View(orders);
        }
        public IActionResult Cart()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> ReviewCart(List<int> SelectedMenuItemIds)
        {
            string error = "";
            if (SelectedMenuItemIds == null || !SelectedMenuItemIds.Any())
            {
                error = "No items selected.";
                TempData["Error"] = error;
                return RedirectToAction("Restaurants", "Restaurant");
            }
            var menuItems = await menuItemRepository.GetSelectedItems(SelectedMenuItemIds);

            return View("Cart", menuItems);
        }

       [HttpPost]
        public async Task<IActionResult> PlaceOrder(List<int> SelectedMenuItemIds, decimal totalAmount, int employeeId)
        {
            var success = "";
            if (SelectedMenuItemIds == null || !SelectedMenuItemIds.Any())
            {
                TempData["Error"] = "No items selected.";
                return RedirectToAction("Restaurants");
            }
            var menuItems = await menuItemRepository.GetSelectedItems(SelectedMenuItemIds);
            var order = new Order
            {
                OrderDate = DateTime.Now,
                Status = "Pending",
                EmployeeId = employeeId,
                TotalAmount = totalAmount
            };
            
            var newOrder = await orderRepository.CreateAsync(order);
            if (await orderRepository.SaveAllAsync())
            {
                foreach (var menuItem in menuItems)
                {
                    var orderItem = new OrderItem
                    {
                        MenuItemId = menuItem.Id,
                        UnitPriceAtTimeOfOrder = menuItem.Price,
                        OrderId = newOrder.OrderId,
                        Quantity = 1
                    };
                    await orderItemRepository.CreateAsync(orderItem);

                }
                success = "Order placed successfully!";
            }

            TempData["Success"] = success;
            return RedirectToAction("Restaurants", "Restaurant");
        }


    }
}
