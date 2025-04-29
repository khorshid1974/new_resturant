using Microsoft.EntityFrameworkCore;
using RestaurantApp.Data.Models;

namespace RestaurantApp.Data.Services
{
    public class MenuService : IMenuService
    {
        private readonly AppDbContext _context;

        public MenuService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MenuItem>> GetAllMenuItemsAsync()
        {
            return await _context.MenuItems.ToListAsync();
        }

        public async Task<IEnumerable<MenuItem>> GetMenuItemsByCategoryAsync(string category)
        {
            return await _context.MenuItems
                .Where(m => m.Category == category)
                .ToListAsync();
        }

        public async Task<MenuItem> GetMenuItemByIdAsync(int id)
        {
            return await _context.MenuItems.FindAsync(id);
        }

        public async Task<MenuItem> AddMenuItemAsync(MenuItem menuItem)
        {
            _context.MenuItems.Add(menuItem);
            await _context.SaveChangesAsync();
            return menuItem;
        }

        public async Task<MenuItem> UpdateMenuItemAsync(MenuItem menuItem)
        {
            var existingItem = await _context.MenuItems.FindAsync(menuItem.Id);
            if (existingItem == null) return null;

            _context.Entry(existingItem).CurrentValues.SetValues(menuItem);
            await _context.SaveChangesAsync();
            return existingItem;
        }

        public async Task<bool> DeleteMenuItemAsync(int id)
        {
            var menuItem = await _context.MenuItems.FindAsync(id);
            if (menuItem == null) return false;

            _context.MenuItems.Remove(menuItem);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateMenuItemAvailabilityAsync(int id, bool isAvailable)
        {
            var menuItem = await _context.MenuItems.FindAsync(id);
            if (menuItem == null) return false;

            menuItem.IsAvailable = isAvailable;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<string>> GetAllCategoriesAsync()
        {
            return await _context.MenuItems
                .Select(m => m.Category)
                .Distinct()
                .ToListAsync();
        }
    }
}