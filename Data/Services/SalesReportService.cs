using Microsoft.EntityFrameworkCore;
using RestaurantApp.Data.Models;

namespace RestaurantApp.Data.Services;

public class SalesReportService : ISalesReportService
{
    private readonly AppDbContext _context;

    public SalesReportService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> GetTodayOrderCountAsync()
    {
        var today = DateTime.Today;
        return await _context.Orders
            .Where(o => o.OrderTime.Date == today)
            .CountAsync();
    }

    public async Task<decimal> GetTodayRevenueAsync()
    {
        var today = DateTime.Today;
        return await _context.Orders
            .Where(o => o.OrderTime.Date == today)
            .SumAsync(o => o.TotalAmount);
    }

    public async Task<List<TableOrderStat>> GetOrdersGroupedByTableAsync()
    {
        var today = DateTime.Today;
        return await _context.Orders
            .Where(o => o.OrderTime.Date == today)
            .GroupBy(o => new { o.TableId, o.Table.Name })
            .Select(g => new TableOrderStat
            {
                TableId = g.Key.TableId,
                TableName = g.Key.Name,
                OrderCount = g.Count(),
                Revenue = g.Sum(o => o.TotalAmount)
            })
            .OrderByDescending(s => s.Revenue)
            .ToListAsync();
    }

    public async Task<List<MenuItemStat>> GetMostPopularItemsAsync()
    {
        var today = DateTime.Today;
        return await _context.OrderItems
            .Where(oi => oi.Order.OrderTime.Date == today)
            .GroupBy(oi => new { oi.MenuItemId, oi.MenuItem.Name })
            .Select(g => new MenuItemStat
            {
                MenuItemId = g.Key.MenuItemId,
                ItemName = g.Key.Name,
                QuantitySold = g.Sum(oi => oi.Quantity),
                Revenue = g.Sum(oi => oi.Quantity * oi.UnitPrice)
            })
            .OrderByDescending(s => s.QuantitySold)
            .Take(10)
            .ToListAsync();
    }
} 