

using Blazor_WASM_MovieApp.Client.Services;
using Blazor_WASM_MovieApp.Exceptions;
using Blazor_WASM_MovieApp.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Blazor_WASM_MovieApp.Client.Pages.BaseComponents
{
    public class DetailsBase : ComponentBase
    {
        [Parameter]
        public int Id { get; set; }

        [CascadingParameter(Name = "ErrorComponent")]
        protected IErrorComponent ErrorComponent { get; set; }

        [Inject]
        protected NavigationManager _navigationManager { get; set; }

        [Inject]
        protected WASM_IMovieService _movieService { get; set; }

        [Inject]
        protected IDialogService Dialog { get; set; }

        [Inject]
        protected WASM_ICreditService _creditService { get; set; }

        public MarkupString HtmlString { get; set; }

        protected List<ErrorItem> ErrorList = new List<ErrorItem>();
        protected Movie movie = new Movie();
        protected List<Credit> credits = new List<Credit>();
        protected string imagesrc = "";
        protected string functionName = string.Empty;
        protected bool shouldRender = false;
        protected bool isDetail = true;

        protected override async Task OnInitializedAsync()
        {
            movie = await _movieService.GetMovie(Id);
            credits = await _creditService.GetCreditsFromMovie(Id);
            HtmlString = (MarkupString)movie.Description;
            shouldRender = true;
        }

        protected override bool ShouldRender()
        {
            return shouldRender;
        }

        protected override async void OnAfterRender(bool firstRender)
        {

            if (!firstRender)
            {
                ErrorComponent.HideError();
                shouldRender = false;
            }

        }

        protected async Task OpenDeleteDialog()
        {

            var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };

            var dialog = Dialog.Show<DeleteDialog>("Delete Movie", options);
            var result = await dialog.Result;

            if (!result.Cancelled)
            {
                try
                {

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
        }

        protected void Cancel()
        {
            _navigationManager.NavigateTo("");
        }

        protected void ChangeToEdit(Movie movie)
        {
            _navigationManager.NavigateTo($"edit/{movie.Id}");
        }

        protected void ChangeToDetail()
        {
            isDetail = true;
            shouldRender = true;
        }

        protected void ChangeToLog()
        {
            isDetail = false;
            shouldRender = true;
        }
    }
}
