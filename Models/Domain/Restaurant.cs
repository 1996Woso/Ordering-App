using System.ComponentModel;

namespace Ordering_App.Models.Domain
{
    public class Restaurant
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        [DisplayName("Location Description")]
        public required string LocationDescription { get; set; }
        [DisplayName("Contact Number")]
        public required string ContactNumber { get; set; }
        //
        public List<MenuItem>? MenuItems { get; set; }
    }
}
