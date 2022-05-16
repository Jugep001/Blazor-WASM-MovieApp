using Blazor_WASM_MovieApp.Client.Services;
using Blazor_WASM_MovieApp.Exceptions;
using Blazor_WASM_MovieApp.Models;
using Microsoft.AspNetCore.Components;

namespace Blazor_WASM_MovieApp.Client.Pages.BaseComponents
{
    public class DeleteGenreBase : ComponentBase
    {
        [Parameter]
        public int Id { get; set; }

        [Inject]
        protected WASM_IGenreService _genreService { get; set; }

        [Inject]
        protected NavigationManager _navigationManager { get; set; }

        [CascadingParameter(Name = "ErrorComponent")]
        protected IErrorComponent ErrorComponent { get; set; }

        protected List<ErrorItem> ErrorList = new List<ErrorItem>();
        protected Genre genre = new Genre();
        protected bool isErrorActive = false;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                genre = await _genreService.GetGenre(Id);
            }
            catch (BusinessException ex)
            {
                ErrorList = ex.ExceptionMessageList;
                ErrorComponent.ShowError(ErrorList);
            }

        }

        protected async Task DeleteGenreById()
        {
            try
            {
                await _genreService.DeleteGenre(Id);
                ErrorComponent.HideError();
                _navigationManager.NavigateTo("/genreIndex");
            }
            catch (BusinessException ex)
            {

                ErrorList = ex.ExceptionMessageList;
                isErrorActive = true;
                StateHasChanged();

            }
        }

        protected void Cancel()
        {
            ErrorComponent.HideError();
            _navigationManager.NavigateTo("/genreIndex");
        }
    }
}
