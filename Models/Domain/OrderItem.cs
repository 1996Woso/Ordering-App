namespace Ordering_App.Models.Domain
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPriceAtTimeOfOrder { get; set; }
        //Order and MenuItem relations
        public int MenuItemId{ get; set; }
        public MenuItem? MenuItem { get; set; }

        public int OrderId { get; set; }
        public Order? Order { get; set; }

    }
}
