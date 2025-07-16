using System;
using Ordering_App.Models.Domain;

namespace Ordering_App.Models;

public class CartViewModel
{
    public List<CartItemViewModel> Items { get; set; } = new();
    public decimal TotalAmount => Items.Sum(i => i.MenuItem.Price * i.Quantity);
    public int EmployeeId { get; set; }
}
