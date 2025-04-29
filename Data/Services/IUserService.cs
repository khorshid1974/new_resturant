using System.Threading.Tasks;
using RestaurantApp.Data.Models;

namespace RestaurantApp.Data.Services
{
    public interface IUserService
    {
        Task<AppUser?> GetUserByUsernameAsync(string username);
        Task<AppUser?> ValidateUserAsync(string username, string password);
        Task<bool> CreateUserAsync(AppUser user);
    }
} 