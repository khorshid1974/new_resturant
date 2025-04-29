namespace RestaurantApp.Data.Models
{
    public class AppUser
    {
        public int Id { get; set; }
        public string Username { get; set; } = "";
        public string PasswordHash { get; set; } = "";
        public string Role { get; set; } = ""; // e.g., "Waiter", "Admin", "Kitchen", "Cashier"
    }
} 