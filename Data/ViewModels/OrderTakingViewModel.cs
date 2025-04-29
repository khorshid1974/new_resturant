using RestaurantApp.Data.Models;

namespace RestaurantApp.Data.ViewModels
{
    public class OrderTakingViewModel
    {
        public int SelectedTableId { get; set; }
        public Order CurrentOrder { get; set; }
        public List<MenuItem> AvailableMenuItems { get; set; } = new();
        public Dictionary<int, int> SelectedItems { get; set; } = new(); // MenuItemId -> Quantity
        public string SpecialInstructions { get; set; }
        public decimal OrderTotal => CurrentOrder?.TotalAmount ?? 0;

        public decimal CalculateCurrentTotal()
        {
            return AvailableMenuItems
                .Where(item => SelectedItems.ContainsKey(item.Id) && SelectedItems[item.Id] > 0)
                .Sum(item => item.Price * SelectedItems[item.Id]);
        }
    }
} 