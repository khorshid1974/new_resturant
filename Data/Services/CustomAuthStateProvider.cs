using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace RestaurantApp.Data.Services
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ProtectedSessionStorage _sessionStorage;
        private readonly IUserService _userService;

        public CustomAuthStateProvider(ProtectedSessionStorage sessionStorage, IUserService userService)
        {
            _sessionStorage = sessionStorage;
            _userService = userService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var userSessionStorageResult = await _sessionStorage.GetAsync<string>("UserSession");
                var userRoleStorageResult = await _sessionStorage.GetAsync<string>("UserRole");
                
                if (!userSessionStorageResult.Success || string.IsNullOrEmpty(userSessionStorageResult.Value))
                {
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                }

                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, userSessionStorageResult.Value),
                    new Claim(ClaimTypes.Role, userRoleStorageResult.Success ? userRoleStorageResult.Value : "User")
                };

                var identity = new ClaimsIdentity(claims, "apiauth_type");
                return new AuthenticationState(new ClaimsPrincipal(identity));
            }
            catch
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
        }

        public async Task MarkUserAsAuthenticated(string username, string role)
        {
            await _sessionStorage.SetAsync("UserSession", username);
            await _sessionStorage.SetAsync("UserRole", role);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role)
            };

            var identity = new ClaimsIdentity(claims, "apiauth_type");
            var user = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        public async Task MarkUserAsLoggedOut()
        {
            await _sessionStorage.DeleteAsync("UserSession");
            await _sessionStorage.DeleteAsync("UserRole");

            var identity = new ClaimsIdentity();
            var user = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }
    }
} 