using Blazor_WASM_MovieApp.Client.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Blazor_WASM_MovieApp.Client.Pages.BaseComponents
{
    public class LogoutBase : ComponentBase
    {
        [Inject]
        public WASM_IAuthenticationService _authenticationService { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        protected override async Task OnInitializedAsync()
        {
            await _authenticationService.Logout();
            NavigationManager.NavigateTo("/");
        }
    }
}
