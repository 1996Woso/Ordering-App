using System.ComponentModel.DataAnnotations;

namespace Ordering_App.Models.Domain
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public required string Status { get; set; }

        public Employee? Employee { get; set; }
        public List<OrderItem>? OrderItems { get; set; }

    }
}
