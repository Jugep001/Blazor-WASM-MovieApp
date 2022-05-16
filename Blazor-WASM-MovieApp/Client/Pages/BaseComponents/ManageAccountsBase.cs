using Blazor_WASM_MovieApp.Client.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;

namespace Blazor_WASM_MovieApp.Client.Pages.BaseComponents
{
    public class ManageAccountsBase : ComponentBase
    {
        [Inject]
        protected WASM_IAuthenticationService _authenticationService { get; set; }

        [Inject]
        protected NavigationManager _navigationManager { get; set; }

        public List<IdentityUser> Users { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Users = await _authenticationService.GetUsers();
        }
    }
}
