using Blazor_WASM_MovieApp.Client.Services;
using Blazor_WASM_MovieApp.Exceptions;
using Blazor_WASM_MovieApp.Models;
using Microsoft.AspNetCore.Components;

namespace Blazor_WASM_MovieApp.Client.Pages.BaseComponents
{
    public class CreateGenreBase : ComponentBase
    {
        [Inject]
        protected WASM_IGenreService _genreService { get; set; }

        [Inject]
        protected NavigationManager _navigationManager { get; set; }

        protected List<ErrorItem> ErrorList = new List<ErrorItem>();
        protected Genre genre = new Genre();
        protected bool isErrorActive = false;


        protected async Task AddGenre()
        {
            try
            {
                await _genreService.AddGenre(genre);
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


            _navigationManager.NavigateTo("/genreIndex");

        }
    }
}
