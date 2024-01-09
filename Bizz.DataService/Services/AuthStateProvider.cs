using Bizz.Entities.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Bizz.DataService.Services
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private AuthenticationState _authenticationState;
       
        public void NotifyAuthenticationStateChanged(AuthenticationState authenticationState)
        {
            _authenticationState = authenticationState;
            NotifyAuthenticationStateChanged(Task.FromResult(authenticationState));
        }

        public void MarkUserAsAuthenticated(string userName)
        {
            // Create claims for the authenticated user
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, userName),
                // Add other claims as needed, e.g., roles, email, etc.
            };

            // Create a ClaimsIdentity
            var claimsIdentity = new ClaimsIdentity(claims, "custom");

            // Create an AuthenticationState with the authenticated ClaimsPrincipal
            var authenticationState = new AuthenticationState(new ClaimsPrincipal(claimsIdentity));

            // Notify that the authentication state has changed
            NotifyAuthenticationStateChanged(Task.FromResult(authenticationState));
        }

        public void MarkUserAsLoggedOut()
        {
            // Set the authentication state to represent a logged-out user
            var anonymousIdentity = new ClaimsIdentity();
            var anonymousPrincipal = new ClaimsPrincipal(anonymousIdentity);
            var anonymousAuthenticationState = new AuthenticationState(anonymousPrincipal);

            // Notify that the authentication state has changed
            NotifyAuthenticationStateChanged(Task.FromResult(anonymousAuthenticationState));
        }



        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            return Task.FromResult(_authenticationState);
        }
    }
}
