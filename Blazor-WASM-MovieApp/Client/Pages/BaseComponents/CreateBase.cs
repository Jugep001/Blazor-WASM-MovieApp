﻿using Blazor_WASM_MovieApp.Client.Services;
using Blazor_WASM_MovieApp.Exceptions;
using Blazor_WASM_MovieApp.Models;
using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System.Text.RegularExpressions;

namespace Blazor_WASM_MovieApp.Client.Pages.BaseComponents
{
    public class CreateBase : ComponentBase
    {
        [CascadingParameter(Name = "ErrorComponent")]
        protected IErrorComponent ErrorComponent { get; set; }

        [CascadingParameter]
        public IModalService Modal { get; set; }

        [Inject]
        protected NavigationManager _navigationManager { get; set; }

        [Inject]
        protected AuthenticationStateProvider _getAuthenticationState { get; set; }

        [Inject]
        protected WASM_IGenreService _genreService { get; set; }

        [Inject]
        protected WASM_IMovieService _movieService { get; set; }

        [Inject]
        protected WASM_ICreditService _creditService { get; set; }

        [Inject]
        protected WASM_IPersonService _personService { get; set; }

        protected List<int> GenreIds { get; set; } = new List<int>();
        protected List<int> CreditIds { get; set; } = new List<int>();
        protected List<int> PeopleIds { get; set; } = new List<int>();

        protected List<Genre> genres = new List<Genre>();
        protected List<Credit> credits = new List<Credit>();
        protected List<Person> people = new List<Person>();
        protected List<ErrorItem> ErrorList = new List<ErrorItem>();

        protected IBrowserFile? loadedImage = null;
        protected IBrowserFile? loadedThumbnailImage = null;
        protected InputFileChangeEventArgs inputFileChangeEventArgs = null;
        protected bool bClearInputFile = false;
        protected bool shouldRender = true;
        protected bool isErrorActive = false;
        protected int CreditId;
        protected int _FunctionId;
        protected int currentIndex;
        protected MudDropContainer<DropItem> _container;

        protected SearchPeople searchPeople = new SearchPeople();
        protected Movie movie = new Movie { Rating = "G", Credits = new List<Credit>() };
        protected List<Credit> creditList = new List<Credit>();

        protected List<DropItem> _items = new List<DropItem>();


        protected class DropItem
        {
            public int? Id { get; set; }

            public int? MovieId { get; set; }
            public virtual Movie? Movie { get; set; }

            public int PersonId { get; set; }
            public virtual Person? Person { get; set; }

            public int FunctionId { get; set; }
            public virtual Function? Function { get; set; }

            public string? Role { get; set; }

            public int Order { get; set; }
            public string? Identifier { get; set; } = "1";

            public bool IsDragOver { get; set; }

            public string Name { get; set; }
        }



        protected override async Task OnInitializedAsync()
        {

            genres = await _genreService.GetGenres();
            shouldRender = true;

        }
        protected override bool ShouldRender()
        {
            return shouldRender;
        }

        protected override void OnAfterRender(bool firstRender)
        {

            shouldRender = false;

        }

        protected async Task AddMovie()
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
                movie.Credits = creditList;
                foreach (Credit credit in movie.Credits)
                {
                    credit.Function = null;
                }
                var authstate = await _getAuthenticationState.GetAuthenticationStateAsync();
                var response = await _movieService.AddMovie(movie, loadedImage, loadedThumbnailImage, GenreIds, authstate.User.Identity.Name);
                await Task.Delay(1);
                _navigationManager.NavigateTo("");

            }
            catch (BusinessException ex)
            {

                ErrorList = ex.ExceptionMessageList;
                isErrorActive = true;
                shouldRender = true;
            }


        }

        protected void AddToItems(Credit credit)
        {
            _items.Add(new DropItem
            {
                Id = credit.Id,
                MovieId = credit.MovieId,
                PersonId = credit.PersonId,
                FunctionId = credit.FunctionId,
                Role = credit.Role,
                Identifier = credit.Identifier,
                Function = credit.Function,
                Person = credit.Person,
                Movie = credit.Movie,
                Order = credit.Order,

            });
            RefreshContainer();
        }


        protected void Drop(MudItemDropInfo<DropItem> credit)
        {
            var item = _items.Find(x => x.Order == credit.Item.Order);
            _items.Remove(item);
            _items.Insert(credit.IndexInZone, credit.Item);



            int i = 0;
            creditList.Clear();
            foreach (var itemEntry in _items)
            {
                itemEntry.Order = i;
                i++;
                creditList.Add(new Credit
                {
                    Id = itemEntry.Id,
                    MovieId = itemEntry.MovieId,
                    Movie = itemEntry.Movie,
                    PersonId = itemEntry.PersonId,
                    Person = itemEntry.Person,
                    FunctionId = itemEntry.FunctionId,
                    Function = itemEntry.Function,
                    Role = itemEntry.Role,
                    Order = itemEntry.Order,
                    Identifier = itemEntry.Identifier,
                });
            }


            RefreshContainer();



        }

        private void RefreshContainer()
        {
            StateHasChanged();
            _container.Refresh();
        }


        protected async void LoadImage(InputFileChangeEventArgs e)
        {
            loadedImage = await e.File.RequestImageFileAsync(e.File.ContentType, 300, 300);
            Task.WaitAll();
            loadedThumbnailImage = await e.File.RequestImageFileAsync(e.File.ContentType, 150, 150);
            Refresh();
        }

        protected async Task DeleteImage()
        {
            loadedImage = null;
            loadedThumbnailImage = null;
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

        protected async void AddDescription()
        {
            var parameters = new ModalParameters();
            parameters.Add("MovieDescription", movie.Description);
            IModalReference? formModal = Modal.Show<AddDescriptionComponent>("Add Description", parameters);
            ModalResult? result = await formModal.Result;
            if (!result.Cancelled)
            {
                movie.Description = (string)result.Data;
            }
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
                if (creditList.Count == 0)
                {
                    credit.Order = 0;
                }
                if (creditList.Count > 0)
                {
                    credit.Order = creditList.Last().Order + 1;
                }
                AddToItems(credit);
                creditList.Add(credit);
                Refresh();
                
            }





        }

        protected async void ShowEditCredit(DropItem DropItemEdit)
        {
            Credit? creditEdit = new Credit
            {
                Id = DropItemEdit.Id,
                MovieId = DropItemEdit.MovieId,
                Movie = DropItemEdit.Movie,
                PersonId = DropItemEdit.PersonId,
                Person = DropItemEdit.Person,
                FunctionId = DropItemEdit.FunctionId,
                Function = DropItemEdit.Function,
                Role = DropItemEdit.Role,
                Order = DropItemEdit.Order,
                Identifier = DropItemEdit.Identifier,

            };

            var parameters = new ModalParameters();
            parameters.Add("CreditList", creditList);
            parameters.Add("OldCredit", creditEdit);

            IModalReference? formModal = Modal.Show<EditCreditComponent>("Edit Credit", parameters);
            ModalResult? result = await formModal.Result;
            if (!result.Cancelled)
            {
                Credit credit = (Credit)result.Data;
                if (credit.ShouldDelete == true)
                {
                    creditList.Remove(creditEdit);
                    _items.Remove(DropItemEdit);
                    RefreshContainer();
                    return;
                }
                credit.Person = await _personService.GetPerson(credit.PersonId);
                credit.Function = await _creditService.GetFunction(credit.FunctionId);
                credit.Order = creditEdit.Order;
                creditList.Remove(creditEdit);
                _items.Remove(DropItemEdit);
                creditList.Add(credit);
                AddToItems(credit);
                RefreshContainer();
            }



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

