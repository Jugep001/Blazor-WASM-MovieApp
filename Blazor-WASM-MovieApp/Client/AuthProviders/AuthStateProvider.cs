using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Blazor_WASM_MovieApp.Client.AuthProviders
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var anonymous = new ClaimsIdentity();
            return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(anonymous)));
        }
    }
}
