using System.ComponentModel.DataAnnotations.Schema;

namespace Ordering_App.Models.Domain
{
    public class Employee
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public Guid EmployeeNumber { get; set; } = Guid.NewGuid();
        public decimal Balance { get; set; }

        public string? LastDepositMonth { get; set; }
        public decimal MonthlyDepositTotal { get; set; }

        public List<Order>? Orders { get; set; }
        [NotMapped]
        public UserParams? UserParams { get; set; }
    }
}
