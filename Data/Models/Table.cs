using System;

namespace RestaurantApp.Data.Models
{
    public class Table
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public bool IsOccupied { get; set; }
        public DateTime? LastOccupiedTime { get; set; }

        // Navigation property for orders at this table
        public ICollection<Order> Orders { get; set; }
    }
}