using Blazor_WASM_MovieApp.Client.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;

namespace Blazor_WASM_MovieApp.Client.Pages.BaseComponents
{
    public class DeleteUserBase : ComponentBase
    {

        [Parameter]
        public string Id { get; set; }

        [Inject]
        protected WASM_IAuthenticationService _authenticationService { get; set; }

        [Inject]
        protected NavigationManager _navigationManager { get; set; }

        public IdentityUser User { get; set; }

        protected override async Task OnInitializedAsync()
        {
            User = await _authenticationService.GetUser(Id);
        }

        protected async Task DeleteUserById()
        {

            await _authenticationService.DeleteUser(Id);
            _navigationManager.NavigateTo("/ManageAccounts");


        }

    }
}
