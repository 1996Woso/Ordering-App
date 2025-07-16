namespace Ordering_App.Models
{
    public class UserParams
    {
        private const int MaxPageSize = 10;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 5;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
        
        public decimal MinBalance { get; set; } = 0;
        public decimal MaxBalance { get; set; } = 1000000;
        public decimal MinTotalPrice { get; set; } = 0;
        public decimal MaxTotalPrice { get; set; } = 1000000;
        public decimal MinPrice { get; set; } = 0;
        public decimal MaxPrice { get; set; } = 1000000;
        public string? LastDepositMonth { get; set; } 
        public string? RestaurantName { get; set; }
        public string? MenuItemName { get; set; }
        public string? MenuItemDescription { get; set; }
        public int EmployeeId { get; set; }
        public string OrderStatus { get; set; } = "Pending";
        public string? OrderDate { get; set; }
        public string OrderBy { get; set; } = "name";
    }
}
