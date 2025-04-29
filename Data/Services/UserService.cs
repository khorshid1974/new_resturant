using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RestaurantApp.Data.Models;

namespace RestaurantApp.Data.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UserService> _logger;

        public UserService(AppDbContext context, ILogger<UserService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<AppUser?> GetUserByUsernameAsync(string username)
        {
            try
            {
                return await _context.AppUsers
                    .FirstOrDefaultAsync(u => u.Username == username);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user by username");
                return null;
            }
        }

        public async Task<AppUser?> ValidateUserAsync(string username, string password)
        {
            try
            {
                var user = await GetUserByUsernameAsync(username);
                if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                {
                    return user;
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating user");
                return null;
            }
        }

        public async Task<bool> CreateUserAsync(AppUser user)
        {
            try
            {
                // Check if username already exists
                if (await _context.AppUsers.AnyAsync(u => u.Username == user.Username))
                {
                    return false;
                }

                // Hash the password
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);

                _context.AppUsers.Add(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user");
                return false;
            }
        }
    }
} 