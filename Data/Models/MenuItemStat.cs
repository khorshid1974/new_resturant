namespace RestaurantApp.Data.Models;

public class MenuItemStat
{
    public int MenuItemId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public int QuantitySold { get; set; }
    public decimal Revenue { get; set; }
} 