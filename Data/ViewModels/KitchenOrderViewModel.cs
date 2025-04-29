using System;

namespace RestaurantApp.Data.ViewModels
{
    public class KitchenOrderViewModel
    {
        public int OrderId { get; set; }
        public int TableId { get; set; }
        public string TableName { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public List<KitchenOrderItemViewModel> Items { get; set; } = new();
        public string SpecialInstructions { get; set; } = string.Empty;
    }

    public class KitchenOrderItemViewModel
    {
        public string Name { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string SpecialInstructions { get; set; } = string.Empty;
    }
} 