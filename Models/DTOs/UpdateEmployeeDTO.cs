using System;
using System.ComponentModel;

namespace Ordering_App.Models.DTOs;

public class UpdateEmployeeDTO
{
    public Guid EmployeeNumber { get; set; } = Guid.NewGuid();
    [DisplayName("Amount")]
    public decimal Balance { get; set; }

    public string? LastDepositMonth { get; set; }
}
