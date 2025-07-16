using System;
using Ordering_App.Models.Domain;

namespace Ordering_App.Models;

public class CartItemViewModel
{
    public MenuItem MenuItem { get; set; }
    public int Quantity { get; set; }
}