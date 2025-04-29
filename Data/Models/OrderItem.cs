using System;

namespace RestaurantApp.Data.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string SpecialInstructions { get; set; }

        // Foreign keys and navigation properties
        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int MenuItemId { get; set; }
        public MenuItem MenuItem { get; set; }

        // Calculated property
        public decimal Subtotal => Quantity * UnitPrice;
    }
}