using System.Collections.Generic;
using System.Threading.Tasks;
using RestaurantApp.Data.Models;

namespace RestaurantApp.Data.Services
{
    public interface IMenuService
    {
        Task<IEnumerable<MenuItem>> GetAllMenuItemsAsync();
        Task<IEnumerable<MenuItem>> GetMenuItemsByCategoryAsync(string category);
        Task<MenuItem> GetMenuItemByIdAsync(int id);
        Task<MenuItem> AddMenuItemAsync(MenuItem menuItem);
        Task<MenuItem> UpdateMenuItemAsync(MenuItem menuItem);
        Task<bool> DeleteMenuItemAsync(int id);
        Task<bool> UpdateMenuItemAvailabilityAsync(int id, bool isAvailable);
        Task<IEnumerable<string>> GetAllCategoriesAsync();
    }
}