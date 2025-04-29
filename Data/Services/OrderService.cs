using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RestaurantApp.Data.Models;
using Microsoft.AspNetCore.SignalR;
using RestaurantApp.Hubs;

namespace RestaurantApp.Data.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<OrderService> _logger;
        private readonly IHubContext<OrderHub> _hubContext;
        private readonly ITableService _tableService;

        public OrderService(AppDbContext context, ILogger<OrderService> logger, IHubContext<OrderHub> hubContext, ITableService tableService)
        {
            _context = context;
            _logger = logger;
            _hubContext = hubContext;
            _tableService = tableService;
        }

        public async Task<Order> CreateOrderAsync(int tableId)
        {
            var table = await _context.Tables.FindAsync(tableId);
            if (table == null)
            {
                _logger.LogError($"Table {tableId} not found when creating order");
                throw new ArgumentException("Table not found");
            }

            var order = new Order
            {
                TableId = tableId,
                OrderTime = DateTime.UtcNow,
                Status = OrderStatus.Pending,
                TotalAmount = 0,
                SpecialInstructions = string.Empty
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            
            _logger.LogInformation($"Created new order {order.Id} for table {tableId}");
            
            // Notify connected clients about the new order
            await _hubContext.Clients.All.SendAsync("OrderUpdated", order);
            
            return order;
        }

        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuItem)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task<IEnumerable<Order>> GetActiveOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuItem)
                .Where(o => o.Status == OrderStatus.Pending || o.Status == OrderStatus.InProgress)
                .ToListAsync();
        }

        public async Task<Order> AddItemToOrderAsync(int orderId, int menuItemId, int quantity, string specialInstructions)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuItem)
                .FirstOrDefaultAsync(o => o.Id == orderId);
            
            if (order == null)
            {
                _logger.LogError($"Order {orderId} not found when adding item");
                throw new ArgumentException("Order not found");
            }

            var menuItem = await _context.MenuItems.FindAsync(menuItemId);
            if (menuItem == null)
            {
                _logger.LogError($"MenuItem {menuItemId} not found when adding to order {orderId}");
                throw new ArgumentException("Menu item not found");
            }

            var orderItem = new OrderItem
            {
                OrderId = orderId,
                MenuItemId = menuItemId,
                Quantity = quantity,
                UnitPrice = menuItem.Price,
                SpecialInstructions = specialInstructions
            };

            order.OrderItems.Add(orderItem);
            
            // Calculate total including all items
            order.TotalAmount = order.OrderItems.Sum(oi => oi.Quantity * oi.UnitPrice);
            
            await _context.SaveChangesAsync();
            
            _logger.LogInformation($"Added item {menuItemId} with quantity {quantity} to order {orderId}. New total: {order.TotalAmount}");
            return order;
        }

        public async Task<Order> UpdateOrderStatusAsync(int orderId, OrderStatus status)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null) throw new ArgumentException("Order not found");

            order.Status = status;
            
            await _context.SaveChangesAsync();
            
            // Use TableService to update table status
            if (status == OrderStatus.Completed)
            {
                await _tableService.UpdateTableStatusAsync(order.TableId, false);
            }
            
            
            // Notify connected clients about the order status update
            await _hubContext.Clients.All.SendAsync("OrderUpdated", order);
            
            return order;
        }

        public async Task<bool> RemoveItemFromOrderAsync(int orderId, int orderItemId)
        {
            var orderItem = await _context.OrderItems
                .FirstOrDefaultAsync(oi => oi.OrderId == orderId && oi.Id == orderItemId);
            if (orderItem == null) return false;

            _context.OrderItems.Remove(orderItem);
            var order = await _context.Orders.FindAsync(orderId);
            if (order != null)
            {
                order.TotalAmount = await CalculateOrderTotalAsync(orderId);
            }
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<decimal> CalculateOrderTotalAsync(int orderId)
        {
            return await _context.OrderItems
                .Where(oi => oi.OrderId == orderId)
                .SumAsync(oi => oi.Quantity * oi.UnitPrice);
        }

        public async Task<IEnumerable<Order>> GetOrdersByTableAsync(int tableId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuItem)
                .Where(o => o.TableId == tableId)
                .OrderByDescending(o => o.OrderTime)
                .ToListAsync();
        }

        public async Task<Order> UpdateOrderTotalAsync(int orderId, decimal totalAmount)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                _logger.LogError($"Order {orderId} not found when updating total");
                throw new ArgumentException("Order not found");
            }

            order.TotalAmount = totalAmount;
            await _context.SaveChangesAsync();
            
            _logger.LogInformation($"Updated total for order {orderId} to {totalAmount}");
            return order;
        }
        // I think kitchen staff should see inProgress orders which means the orders been confirmed by Capten or waiter
        // so I will make new one to return
        public async Task<IEnumerable<Order>> GetPendingOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuItem)
                .Include(o => o.Table)
                .Where(o => o.Status == OrderStatus.Pending)
                .OrderBy(o => o.OrderTime)
                .ToListAsync();
        }
        public async Task<IEnumerable<Order>> GetInProgressOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuItem)
                .Include(o => o.Table)
                .Where(o => o.Status == OrderStatus.InProgress)
                .OrderBy(o => o.OrderTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetUnpaidOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuItem)
                .Include(o => o.Table)
                .Where(o => o.Status != OrderStatus.Completed)
                .OrderBy(o => o.OrderTime)
                .ToListAsync();
        }
    }
}