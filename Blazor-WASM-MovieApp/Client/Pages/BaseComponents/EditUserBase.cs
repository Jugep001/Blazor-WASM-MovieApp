using Blazor_WASM_MovieApp.Client.Services.Interfaces;
using Blazor_WASM_MovieApp.Exceptions;
using Blazor_WASM_MovieApp.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;

namespace Blazor_WASM_MovieApp.Client.Pages.BaseComponents
{
    public class EditUserBase : ComponentBase
    {

        [Parameter]
        public string Id { get; set; }

        [Inject]
        protected NavigationManager _navigationManager { get; set; }

        [Inject]
        protected WASM_IAuthenticationService _authenticationService { get; set; }

        protected AuthInput authInput = new AuthInput();
        protected List<ErrorItem> ErrorList = new List<ErrorItem>();
        public IdentityUser User { get; set; }

        protected override async Task OnInitializedAsync()
        {



            User = await _authenticationService.GetUser(Id);


        }

        protected async Task UpdateUser()
        {

            authInput.User = User;
            await _authenticationService.UpdateUser(authInput);
            _navigationManager.NavigateTo("/ManageAccounts");



        }

        protected void Cancel()
        {


            _navigationManager.NavigateTo("/ManageAccounts");


        }
    }
}
