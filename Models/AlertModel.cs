using System;

namespace Ordering_App.Models;

public class AlertModel
{
    public string? Message { get; set; }
    public bool IsDivDisplayed { get; set; } = true;
    public string? RedirectUrl { get; set; }
}
