using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Ordering_App.Models.DTOs;

public class UpdateEmployeeDTO
{
    public Guid EmployeeNumber { get; set; } = Guid.NewGuid();
    [DisplayName("Amount")]
      [Range(0.01, 5000, ErrorMessage = "Price must more than R 0.00 and less than R5000.00")]
    public decimal Balance { get; set; }

    public string? LastDepositMonth { get; set; }
}
