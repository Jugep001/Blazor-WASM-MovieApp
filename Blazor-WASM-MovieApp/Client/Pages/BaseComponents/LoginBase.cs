using Blazor_WASM_MovieApp.Client.Services.Interfaces;
using Blazor_WASM_MovieApp.Models;
using Microsoft.AspNetCore.Components;

namespace Blazor_WASM_MovieApp.Client.Pages.BaseComponents
{
    public class LoginBase : ComponentBase
    {
        [Inject]
        protected WASM_IAuthenticationService _authenticationService { get; set; }

        [Inject]
        protected NavigationManager _navigationManager { get; set; }

        protected Movie movie = new Movie();
        protected AuthInput authInput = new AuthInput();
        protected string Role = "reader";
        public bool ShowAuthError { get; set; }
        public string Error { get; set; }

        
        

        public async Task LoginUser()
        {

            ShowAuthError = false;
            var result = await _authenticationService.Login(authInput);
            if (!result.IsAuthSuccessful)
            {
                Error = result.ErrorMessage;
                ShowAuthError = true;
            }
            else
            {
                _navigationManager.NavigateTo("/");
            }
        }
    }
}
