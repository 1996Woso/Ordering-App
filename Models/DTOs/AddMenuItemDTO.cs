using System;
using System.ComponentModel.DataAnnotations;

namespace Ordering_App.Models.DTOs;

public class AddMenuItemDTO
{
     public required string Name { get; set; }
    public required string Description { get; set; }
    [Range(0.01, 10000, ErrorMessage = "Price must more than R 0.00 and less than R10,000.00")]
    public decimal Price { get; set; }
}
