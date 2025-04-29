using System.Collections.Generic;using System.Threading.Tasks;
using RestaurantApp.Data.Models;

namespace RestaurantApp.Data.Services
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(int tableId);
        Task<Order> GetOrderByIdAsync(int orderId);
        Task<IEnumerable<Order>> GetActiveOrdersAsync();
        Task<IEnumerable<Order>> GetPendingOrdersAsync();
        Task<IEnumerable<Order>> GetInProgressOrdersAsync();
        Task<IEnumerable<Order>> GetUnpaidOrdersAsync();
        Task<Order> AddItemToOrderAsync(int orderId, int menuItemId, int quantity, string specialInstructions);
        Task<Order> UpdateOrderStatusAsync(int orderId, OrderStatus status);
        Task<bool> RemoveItemFromOrderAsync(int orderId, int orderItemId);
        Task<decimal> CalculateOrderTotalAsync(int orderId);
        Task<IEnumerable<Order>> GetOrdersByTableAsync(int tableId);
        Task<Order> UpdateOrderTotalAsync(int orderId, decimal totalAmount);
    }
}