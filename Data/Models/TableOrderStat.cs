namespace RestaurantApp.Data.Models;

public class TableOrderStat
{
    public int TableId { get; set; }
    public string TableName { get; set; } = string.Empty;
    public int OrderCount { get; set; }
    public decimal Revenue { get; set; }
} 