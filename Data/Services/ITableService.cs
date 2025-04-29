using System.Collections.Generic;
using System.Threading.Tasks;
using RestaurantApp.Data.Models;

namespace RestaurantApp.Data.Services
{
    public interface ITableService
    {
        Task<IEnumerable<Table>> GetAllTablesAsync();
        Task<Table> GetTableByIdAsync(int id);
        Task<Table> UpdateTableStatusAsync(int id, bool isOccupied);
        Task<IEnumerable<Table>> GetAvailableTablesAsync();
        Task<IEnumerable<Table>> GetOccupiedTablesAsync();
    }
} 