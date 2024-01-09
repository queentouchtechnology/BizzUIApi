using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Bizz.Entities.Models;

namespace Bizz.DataService.Services
{
    public class AuthenticationService : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;

        public AuthenticationService(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var storedClaimsDtoList = await _localStorage.GetItemAsync<List<ClaimsDto>>("user");

                if (storedClaimsDtoList != null && storedClaimsDtoList.Any())
                {
                    // Convert back to List<Claim> for authentication state
                    var storedClaims = storedClaimsDtoList.Select(dto => new Claim(dto.Type, dto.Value)).ToList();
                    var identity = new ClaimsIdentity(storedClaims, "custom");
                    return new AuthenticationState(new ClaimsPrincipal(identity));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting authentication state: {ex.Message}");
            }

            return new AuthenticationState(new ClaimsPrincipal());
        }
        public async Task<ClaimUser> GetCurrentUserAsync()
        {
            try
            {
                var claimsDtoList = await _localStorage.GetItemAsync<List<ClaimsDto>>("user");

                if (claimsDtoList != null && claimsDtoList.Any())
                {
                    var userName = claimsDtoList.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                    return new ClaimUser { UserName = userName };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting current user: {ex.Message}");
            }

            return null;
        }

        public async Task SignInAsync(string username)
        {
            var userClaims = new List<Claim> { new Claim(ClaimTypes.Name, username) };

            // Convert to ClaimsDto for storage
            var claimsDtoList = userClaims.Select(c => new ClaimsDto { Type = c.Type, Value = c.Value }).ToList();

            // Clear existing claims (if any)
            await _localStorage.RemoveItemAsync("user");

            // Store new claims
            await _localStorage.SetItemAsync("user", claimsDtoList);

            // Notify authentication state change
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task SignInCustomAsync(string username)
        {
            var userClaims = new List<CustomClaim> { new CustomClaim(ClaimTypes.Name, username, "CustomValue1", 123) };

            // Convert to ClaimsDto for storage
            // Convert to ClaimsDto for storage
            var claimsDtoList = userClaims.Select(c => new ClaimsDto
            {
                Type = c.Type,
                Value = c.Value,
                CustomField1 = c.CustomField1,
                CustomField2 = c.CustomField2
            }).ToList();

            // Clear existing claims (if any)
            await _localStorage.RemoveItemAsync("user");

            // Store new claims
            await _localStorage.SetItemAsync("user", claimsDtoList);

            // Notify authentication state change
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }



        public async Task SignOutAsync()
        {
            await _localStorage.RemoveItemAsync("user");
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}