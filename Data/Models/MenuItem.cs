using System;

namespace RestaurantApp.Data.Models
{
    public class MenuItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public bool IsAvailable { get; set; }
        public string ImageUrl { get; set; }

        // Navigation property for order items
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}