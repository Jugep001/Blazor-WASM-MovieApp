using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components.Forms;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Blazor_WASM_MovieApp.Exceptions;
using Blazor_WASM_MovieApp.Client.Services;
using Blazor_WASM_MovieApp.Models;

namespace Blazor_WASM_MovieApp.Client.Pages.BaseComponents
{
    public class EditBase : ComponentBase
    {
        [Parameter]
        public int Id { get; set; }

        [CascadingParameter(Name = "ErrorComponent")]
        protected IErrorComponent ErrorComponent { get; set; }

        [CascadingParameter]
        public IModalService Modal { get; set; }

        [Inject]
        protected WASM_IMovieService _movieService { get; set; }

        [Inject]
        protected WASM_IGenreService _genreService { get; set; }

        [Inject]
        protected WASM_ICreditService _creditService { get; set; }

        [Inject]
        protected WASM_IPersonService _personService { get; set; }

        [Inject]
        protected NavigationManager _navigationManager { get; set; }

        [Inject]
        protected AuthenticationStateProvider _getAuthenticationState { get; set; }

        protected List<Genre> genres = new List<Genre>();
        protected List<Credit> credits = new List<Credit>();
        protected List<ErrorItem> ErrorList = new List<ErrorItem>();
        protected List<int> GenreIds { get; set; } = new List<int>();
        protected List<int> CreditIds { get; set; } = new List<int>();
        protected List<Credit> DeleteCreditList { get; set; } = new List<Credit>();

        public MarkupString HtmlString { get; set; }

        protected IBrowserFile? loadedImage = null;
        protected IBrowserFile? loadedThumbnailImage = null;
        protected string ImageName = string.Empty;
        protected bool bClearInputFile = false;
        protected bool shouldDelete = false;
        protected bool shouldRender = true;
        protected bool isErrorActive = false;
        protected int currentIndex;

        protected SearchPeople searchPeople = new SearchPeople();
        protected Movie movie = new Movie();
        protected List<Credit> creditList = new List<Credit>();


        protected override async Task OnInitializedAsync()
        {


            try
            {

                movie = await _movieService.GetMovie(Id);
                genres = await _genreService.GetGenres();
                creditList = new List<Credit>(movie.Credits.OrderBy(x => x.Position));

                HtmlString = (MarkupString)movie.Description;

                if (movie.Image != null)
                {
                    string pattern = @"([a-z0-9]{8}[-][a-z0-9]{4}[-][a-z0-9]{4}[-][a-z0-9]{4}[-][a-z0-9]{12})";
                    ImageName = Regex.Replace(movie.Image.ImageName, pattern, "");
                }


                foreach (var genre in movie.Genres)
                {
                    GenreIds.Add(genre.Id);
                }
                foreach (var credit in creditList)
                {
                    CreditIds.Add(credit.Id.Value);
                }
                shouldRender = true;

            }
            catch (BusinessException ex)
            {

                ErrorList = ex.ExceptionMessageList;
                isErrorActive = true;
                shouldRender = true;
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

        protected async Task UpdateMovie()
        {
            try
            {
                foreach (Credit credit in creditList)
                {
                    credit.Person = null;
                    credit.Function = null;
                    _creditService.AddCredit(credit);
                }

                if (StripHTML(movie.Description) == "")
                {
                    movie.Description = StripHTML(movie.Description);
                }
                //var authstate = await _getAuthenticationState.GetAuthenticationStateAsync();
                var response = await _movieService.UpdateMovie(movie, loadedImage, loadedThumbnailImage, GenreIds, DeleteCreditList, shouldDelete, "admin");
                
                ErrorComponent.HideError();
                _navigationManager.NavigateTo("");

            }
            catch (BusinessException ex)
            {

                ErrorList = ex.ExceptionMessageList;
                ErrorComponent.ShowError(ErrorList);

            }

        }

        protected void SetIndex(Credit credit)
        {
            currentIndex = creditList.FindIndex(x => x.Position == credit.Position);
            Refresh();
        }

        protected void Drop(Credit credit)
        {
            var tempPos = credit.Position;
            var dropIndex = creditList.FindIndex(x => x.Position == credit.Position);
            var currentCredit = creditList[currentIndex];
            creditList.RemoveAt(currentIndex);
            creditList.Insert(dropIndex, currentCredit);

            int i = 0;
            foreach (var creditEntry in creditList)
            {
                creditEntry.Position = i;
                i++;
            }
            Refresh();


        }

        protected async void LoadImage(InputFileChangeEventArgs e)
        {
            loadedImage = await e.File.RequestImageFileAsync(e.File.ContentType, 300, 300);
            Task.WaitAll();
            loadedThumbnailImage = await e.File.RequestImageFileAsync(e.File.ContentType, 150, 150);
            ImageName = loadedImage.Name;
            Refresh();
        }

        protected async Task DeleteImage()
        {
            loadedImage = null;
            loadedThumbnailImage = null;
            ImageName = string.Empty;
            shouldDelete = true;
            bClearInputFile = true;
            Refresh();
            await Task.Delay(1);
            bClearInputFile = false;
            Refresh();
        }

        protected void CheckboxClicked(int genreId, object checkedValue)
        {
            if ((bool)checkedValue)
            {
                if (!GenreIds.Contains(genreId))
                {
                    GenreIds.Add(genreId);
                }
            }
            else
            {
                if (GenreIds.Contains(genreId))
                {
                    GenreIds.Remove(genreId);
                }
            }
            Refresh();
        }

        protected async void ShowAddCredit()
        {
            var parameters = new ModalParameters();
            parameters.Add("CreditList", creditList);
            IModalReference? formModal = Modal.Show<AddCreditComponent>("Add Credit", parameters);
            ModalResult? result = await formModal.Result;
            if (!result.Cancelled)
            {
                Credit credit = (Credit)result.Data;
                credit.Person = await _personService.GetPerson(credit.PersonId);
                credit.Function = await _creditService.GetFunction(credit.FunctionId);
                credit.MovieId = Id;
                if (creditList.Count == 0)
                {
                    credit.Position = 0;
                }
                if (creditList.Count > 0)
                {
                    credit.Position = creditList.MaxBy(x => x.Position).Position + 1;
                }              
                creditList.Add(credit);
                Refresh();
            }


        }

        protected async void ShowEditCredit(Credit creditEdit)
        {

            var parameters = new ModalParameters();
            parameters.Add("CreditList", creditList);
            parameters.Add("OldCredit", creditEdit);

            IModalReference? formModal = Modal.Show<EditCreditComponent>("Edit Credit", parameters);
            ModalResult? result = await formModal.Result;
            if (!result.Cancelled)
            {
                Credit credit = (Credit)result.Data;
                credit.Person = await _personService.GetPerson(credit.PersonId);
                credit.Function = await _creditService.GetFunction(credit.FunctionId);
                credit.MovieId = Id;
                creditList.Remove(creditEdit);
                DeleteCreditList.Add(creditEdit);
                creditList.Add(credit);
                Refresh();
            }


        }

        protected void DeleteCredit(Credit credit)
        {
            ErrorComponent.HideError();
            creditList.Remove(credit);
            DeleteCreditList.Add(credit);
            Refresh();
        }

        protected async void HandleCreditSubmit()
        {
            try
            {

                credits = await _creditService.GetCreditsByName(searchPeople.SearchString);
                ErrorComponent.HideError();
                Refresh();

            }
            catch (BusinessException ex)
            {


                ErrorList = ex.ExceptionMessageList;
                ErrorComponent.ShowError(ErrorList);

            }

        }

        private static string StripHTML(string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }

        public async Task RestoreMovie()
        {
            await _movieService.RestoreMovie(movie, "");
            _navigationManager.NavigateTo("");
        }

        protected void Refresh()
        {
            shouldRender = true;
            InvokeAsync(() =>
            {
                StateHasChanged();
            });
        }

        protected void Cancel()
        {


            _navigationManager.NavigateTo("");
            ErrorComponent.HideError();

        }
    }
}
