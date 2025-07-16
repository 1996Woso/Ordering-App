using System.ComponentModel.DataAnnotations;

namespace Ordering_App.Models.Domain
{
    public class MenuItem
    {
        public int Id { get; set; }
        public int RestaurantId { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        [Range(0.01, 1000, ErrorMessage = "Price must more than R 0.00")]
        public decimal Price { get; set; }
        //
        public Restaurant? Restaurant { get; set; }
        public List<OrderItem>? OrderItems { get; set; }

    }
}
