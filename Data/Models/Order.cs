using System;

namespace RestaurantApp.Data.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderTime { get; set; }
        public OrderStatus Status { get; set; }
        public decimal TotalAmount { get; set; }
        public string SpecialInstructions { get; set; }

        // Foreign key for Table
        public int TableId { get; set; }
        public Table Table { get; set; }

        // Navigation property for order items
        public ICollection<OrderItem> OrderItems { get; set; }
    }

    public enum OrderStatus
    {
        Pending,
        InProgress,
        ReadyToServe,
        Completed,
        Cancelled
    }
}