using RestaurantApp.Data.Models;

namespace RestaurantApp.Data.Services;

public interface ISalesReportService
{
    Task<int> GetTodayOrderCountAsync();
    Task<decimal> GetTodayRevenueAsync();
    Task<List<TableOrderStat>> GetOrdersGroupedByTableAsync();
    Task<List<MenuItemStat>> GetMostPopularItemsAsync();
} 