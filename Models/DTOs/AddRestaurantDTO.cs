using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Ordering_App.Models.DTOs;

public class AddRestaurantDTO
{
    public required string Name { get; set; }
    [DisplayName("Location Description")]
    public required string LocationDescription { get; set; }
    [DisplayName("Contact Number")]
    public required string ContactNumber { get; set; }
}
