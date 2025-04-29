using Microsoft.AspNetCore.SignalR;
using RestaurantApp.Data.Models;

namespace RestaurantApp.Hubs
{
    /// <summary>
    /// SignalR hub for broadcasting order updates to connected clients
    /// </summary>
    public class OrderHub : Hub
    {
        // The actual broadcasting will be done through IHubContext<OrderHub> in the OrderService
        // This class primarily defines the hub endpoint that clients can connect to
        
        // You can add specific hub methods here if needed, for example:
        public async Task UpdateOrder(Order order)
        {
            await Clients.All.SendAsync("OrderUpdated", order);
        }
    }
}