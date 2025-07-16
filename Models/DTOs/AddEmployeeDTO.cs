using System;
using System.ComponentModel.DataAnnotations;

namespace Ordering_App.Models.DTOs;

public class AddEmployeeDTO
{
        
        [Required(ErrorMessage = "Name is required.")]
        public required string Name { get; set; }
   

}
