using Blazor_WASM_MovieApp.Client.Services;
using Blazor_WASM_MovieApp.Exceptions;
using Blazor_WASM_MovieApp.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Blazor_WASM_MovieApp.Client.Pages.BaseComponents
{
    public class DeleteBase : ComponentBase
    {
        [Parameter]
        public int Id { get; set; }

        [CascadingParameter(Name = "ErrorComponent")]
        protected IErrorComponent ErrorComponent { get; set; }

        [Inject]
        protected WASM_IMovieService _movieService { get; set; }

        [Inject]
        protected WASM_ICreditService _creditService { get; set; }

        [Inject]
        protected NavigationManager _navigationManager { get; set; }

        [Inject]
        protected AuthenticationStateProvider _getAuthenticationState { get; set; }

        protected List<ErrorItem> ErrorList = new List<ErrorItem>();
        protected Movie? movie;
        protected bool shouldRender = false;

        protected override async Task OnInitializedAsync()
        {

            try
            {

                movie = await _movieService.GetMovie(Id);
                shouldRender = true;

            }
            catch (BusinessException ex)
            {

                ErrorList = ex.ExceptionMessageList;
                ErrorComponent.ShowError(ErrorList);

            }


        }

        protected override void OnAfterRender(bool firstRender)
        {

            if (!firstRender)
            {
                ErrorComponent.HideError();
            }
            shouldRender = false;
        }

        protected async void DeleteMovie()
        {
            try
            {

                var authstate = await _getAuthenticationState.GetAuthenticationStateAsync();
                await _movieService.DeleteMovie(Id, "admin");
                shouldRender = true;
                _navigationManager.NavigateTo("");
            }
            catch (BusinessException ex)
            {

                ErrorList = ex.ExceptionMessageList;
                ErrorComponent.ShowError(ErrorList);
                shouldRender = true;

            }


        }

        protected void Cancel()
        {
            _navigationManager.NavigateTo("");
        }
    }
}
