using Blazor_WASM_MovieApp.Client.Services.Interfaces;
using Blazor_WASM_MovieApp.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;

namespace Blazor_WASM_MovieApp.Client.Pages.BaseComponents
{
    public class RegisterBase : ComponentBase
    {
        [Inject]
        protected WASM_IAuthenticationService _authenticationService { get; set; }

        [Inject]
        protected NavigationManager _navigationManager { get; set; }

        protected Movie movie = new Movie();
        protected string Role = "reader";

        protected AuthInput authInput = new AuthInput();

        public async Task AddUser()
        {
            var role = Role;

            authInput.Role = role;

            await _authenticationService.Register(authInput);
            _navigationManager.NavigateTo("/ManageAccounts", true);
        }
    }
}
