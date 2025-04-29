using Microsoft.EntityFrameworkCore;
using RestaurantApp.Data.Models;

namespace RestaurantApp.Data.Services
{
    public class TableService : ITableService
    {
        private readonly AppDbContext _context;

        public TableService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Table>> GetAllTablesAsync()
        {
            return await _context.Tables.ToListAsync();
        }

        public async Task<Table> GetTableByIdAsync(int id)
        {
            return await _context.Tables.FindAsync(id);
        }

        public async Task<Table> UpdateTableStatusAsync(int id, bool isOccupied)
        {
            var table = await _context.Tables.FindAsync(id);
            if (table == null) return null;

            table.IsOccupied = isOccupied;
            table.LastOccupiedTime = isOccupied ? DateTime.UtcNow : null;
            
            await _context.SaveChangesAsync();
            return table;
        }

        public async Task<IEnumerable<Table>> GetAvailableTablesAsync()
        {
            return await _context.Tables
                .Where(t => !t.IsOccupied)
                .ToListAsync();
        }

        public async Task<IEnumerable<Table>> GetOccupiedTablesAsync()
        {
            return await _context.Tables
                .Where(t => t.IsOccupied)
                .ToListAsync();
        }
    }
} 