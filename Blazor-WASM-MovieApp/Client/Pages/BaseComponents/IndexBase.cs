using Blazor_WASM_MovieApp.Client.AuthProviders;
using Blazor_WASM_MovieApp.Client.Services;
using Blazor_WASM_MovieApp.Exceptions;
using Blazor_WASM_MovieApp.Models;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace Blazor_WASM_MovieApp.Client.Pages.BaseComponents
{
    public class IndexBase : ComponentBase
    {
        [CascadingParameter(Name = "ErrorComponent")]
        protected IErrorComponent ErrorComponent { get; set; }

        [CascadingParameter]
        protected string? MessageComponent { get; set; }

        [Parameter]
        public string? Name { get; set; }

        [Inject]
        public HttpClient Http { get; set; }

        [Inject]
        protected NavigationManager _navigationManager { get; set; }

        [Inject]
        protected WASM_IMovieService _movieService { get; set; }

        protected Search search = new Search();
        protected List<Movie> movies = new List<Movie>();
        protected List<Movie> page = new List<Movie>();
        protected List<ErrorItem> ErrorList = new List<ErrorItem>();
        protected List<string> SearchStringList = new List<string>();

        protected string firstTenWords = "";
        protected string? searchString = null;
        protected string prefix = "... ";
        protected string suffix = "...";

        protected List<MarkupString>? highlightedDescription = null;

        protected bool shouldRender = true;

        protected int pageSize = 10;
        protected int pageIndex = 0;
        protected int pageCounter = 0;


        protected override async Task OnInitializedAsync()
        {

            try
            {
                movies = await _movieService.GetMovies(true);
                pageCounter = movies.Count() / pageSize;
                HandleChangePage(1);
                if (Name != null)
                {

                    search.SearchString = Name;
                    Name = null;
                    HandleSubmit();

                }


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

        protected async void HandleSubmit()
        {
            try
            {
                highlightedDescription = null;
                if (search.SearchString == null || search.SearchString.Length == 0)
                {
                    HandleChangePage(1);
                    return;
                }
                page = await _movieService.SearchMovies(search.SearchString, true);

                shouldRender = true;


                ErrorComponent.HideError();
                _navigationManager.NavigateTo("");
            }
            catch (BusinessException ex)
            {


                ErrorList = ex.ExceptionMessageList;
                ErrorComponent.ShowError(ErrorList);

            }

        }

        protected async Task GetFirstTenWords(string description)
        {
            string? call = await _movieService.GetFirstTenWords(description);
            firstTenWords = call;
            shouldRender = true;
            return;
        }

        protected bool GetHighlightedDescription(Movie movie, string searchString)
        {
            HighlightDescription(movie, searchString);
            return true;
        }

        protected async void HighlightDescription(Movie movie, string searchString)
        {
            highlightedDescription = _movieService.GetHighlightedDescription(movie.Description, searchString);
            
        }

        protected void HandleChangePage(int pageIndex)
        {
            page = movies.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            shouldRender = true;
        }

        protected override void OnAfterRender(bool firstRender)
        {

            base.OnAfterRender(firstRender);
            shouldRender = false;
        }
    }
}
