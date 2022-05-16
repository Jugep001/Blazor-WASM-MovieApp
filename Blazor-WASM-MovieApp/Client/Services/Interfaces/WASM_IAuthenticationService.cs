using Blazor_WASM_MovieApp.Models;
using Microsoft.AspNetCore.Identity;

namespace Blazor_WASM_MovieApp.Client.Services.Interfaces
{
    public interface WASM_IAuthenticationService
    {
        public Task Register(AuthInput authInput);
        public Task<AuthInput> Login(AuthInput authInput);
        public Task Logout();
        public Task<List<IdentityUser>> GetUsers();
        public Task<IdentityUser> GetUser(string id);
        public Task UpdateUser(AuthInput authInput);
        public Task DeleteUser(string id);
    }
}
