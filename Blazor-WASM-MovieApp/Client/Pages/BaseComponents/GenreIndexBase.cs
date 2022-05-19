using Blazor_WASM_MovieApp.Client.Services;
using Blazor_WASM_MovieApp.Exceptions;
using Blazor_WASM_MovieApp.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Blazor_WASM_MovieApp.Client.Pages.BaseComponents
{
    public class GenreIndexBase : ComponentBase
    {
        [CascadingParameter(Name = "ErrorComponent")]
        protected IErrorComponent ErrorComponent { get; set; }

        [Inject]
        protected NavigationManager _navigationManager { get; set; }

        [Inject]
        protected WASM_IGenreService _genreService { get; set; }

        protected SearchGenre searchGenre = new SearchGenre();
        protected List<Genre> genres;
        protected List<ErrorItem> ErrorList = new List<ErrorItem>();
        protected bool shouldRender = true;
        protected MudTable<Genre> table;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                
                genres = await _genreService.GetGenres();
                shouldRender = true;

            }
            catch (BusinessException ex)
            {


                ErrorList = ex.ExceptionMessageList;
                ErrorComponent.ShowError(ErrorList);

            }





        }

        protected override bool ShouldRender()
        {
            return shouldRender;
        }

        protected override void OnAfterRender(bool firstRender)
        {

            if (!firstRender)
            {
                ErrorComponent.HideError();
            }
            shouldRender = false;

        }

        protected async Task HandleSubmit()
        {
            try
            {

                genres = await _genreService.SearchGenres(searchGenre.SearchString);
                ErrorComponent.HideError();
                _navigationManager.NavigateTo("/genreIndex");
                shouldRender = true;

            }
            catch (BusinessException ex)
            {

                ErrorList = ex.ExceptionMessageList;
                ErrorComponent.ShowError(ErrorList);

            }

        }
    }
}
