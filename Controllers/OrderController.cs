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
        private readonly IEmployeeRepository employeeRepository;

        public OrderController(IOrderRepository orderRepository, IMenuItemRepository menuItemRepository,
        IOrderItemRepository orderItemRepository, IEmployeeRepository employeeRepository)
        {
            this.orderRepository = orderRepository;
            this.menuItemRepository = menuItemRepository;
            this.orderItemRepository = orderItemRepository;
            this.employeeRepository = employeeRepository;
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
        [HttpGet]
        public async Task<IActionResult> EmployeeOrders([FromQuery] UserParams userParams,
         decimal minTotalPrice, decimal maxTotalPrice, string orderDate, string orderStatus, int id
         )
        {
            if (minTotalPrice >= 0) userParams.MinTotalPrice = minTotalPrice;
            if (maxTotalPrice != 0 && maxTotalPrice != userParams.MaxTotalPrice) userParams.MaxTotalPrice = maxTotalPrice;
            userParams.PageSize = 1;
            if (!orderDate.IsNullOrEmpty()) userParams.OrderDate = orderDate;
            if (!orderStatus.IsNullOrEmpty()) userParams.OrderStatus = orderStatus;
            var orders = await orderRepository.GetByEmployeeId(userParams, id);
            ViewBag.TotalCount = orders.TotalCount;
            ViewBag.TotalPages = orders.TotalPages;
            ViewBag.CurrentPage = userParams.PageNumber;
            ViewBag.MinTotalPrice = userParams.MinTotalPrice;
            ViewBag.MaxTotalPrice = userParams.MaxTotalPrice;
            ViewBag.OrderDate = userParams.OrderDate;
            ViewBag.OrderStatus = userParams.OrderStatus;
            ViewBag.EmployeeId = id;
            return View(orders);
        }
        public IActionResult Cart()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> ReviewCart(List<int> SelectedMenuItemIds, List<int> NumberOfSelectedItems)
        {
            if (SelectedMenuItemIds == null || !SelectedMenuItemIds.Any())
            {
                TempData["Error"] = "No items selected.";
                return RedirectToAction("Restaurants", "Restaurant");
            }

            var menuItems = await menuItemRepository.GetSelectedItems(SelectedMenuItemIds);

            
            if (NumberOfSelectedItems == null || NumberOfSelectedItems.Count != menuItems.Count)
            {
                TempData["Error"] = "Mismatch between items and quantities.";
                return RedirectToAction("Restaurants", "Restaurant");
            }

            var cartItems = menuItems.Select((menuItem, index) => new CartItemViewModel
            {
                MenuItem = menuItem,
                Quantity = NumberOfSelectedItems[index]
            }).ToList();

            var cart = new CartViewModel
            {
                Items = cartItems,
                EmployeeId = 3
            };

            return View("Cart", cart);
        }


        [HttpPost]
        public async Task<IActionResult> PlaceOrder(List<int> SelectedMenuItemIds, decimal totalAmount, int employeeId
        ,  List<int> NumberOfSelectedItems)
        {
            var menuItems = await menuItemRepository.GetSelectedItems(SelectedMenuItemIds);
            var success = "";
            if (SelectedMenuItemIds == null || !SelectedMenuItemIds.Any())
            {
                TempData["Error"] = "No items selected.";
                return RedirectToAction("Cart");
            }
            var employee = await employeeRepository.GetByIdAsync(employeeId);
            if (totalAmount > employee!.Balance)
            {
                TempData["Error"] = $"Your balance R {employee.Balance} is not sufficient";
                return View("Cart", new CartViewModel {
                    Items = menuItems.Select((item, i) => new CartItemViewModel {
                        MenuItem = item,
                        Quantity = NumberOfSelectedItems[i]
                    }).ToList(),
                    EmployeeId = employeeId
                });

            }
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
                for (int i = 0; i < menuItems.Count; i++)
                {
                    var orderItem = new OrderItem
                    {
                        MenuItemId = menuItems[i].Id,
                        UnitPriceAtTimeOfOrder = menuItems[i].Price,
                        OrderId = newOrder.OrderId,
                        Quantity = NumberOfSelectedItems[i]
                    };

                    await orderItemRepository.CreateAsync(orderItem);
                }
                success = "Order placed successfully!";
                employee.Balance -= totalAmount;
                await employeeRepository.UpdateDalanceAsync(employee);
                await employeeRepository.SaveAllAsync();
            }

            TempData["Success"] = success;
            return RedirectToAction("Restaurants", "Restaurant");
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var order = await orderRepository.GetByIdAsync(id);
            if (order == null)
            {
                TempData["Error"] = "Order not found";
                return RedirectToAction("Orders");
            }
            return View(order);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Order order)
        {
            string errorMessage = "", successMessage = "";
            if (order == null)
            {
                errorMessage = "Order cannot be found";
            }
            else
            {
                await orderRepository.UpdateAsync(order);
                if (await orderRepository.SaveAllAsync())
                {
                    successMessage = "Order has been edited successfully";
                }
            }
            TempData["Error"] = errorMessage;
            TempData["Success"] = successMessage;
            return View();
        }
    }
}
