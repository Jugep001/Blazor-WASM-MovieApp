using Blazor_WASM_MovieApp.Client.Services;
using Blazor_WASM_MovieApp.Exceptions;
using Blazor_WASM_MovieApp.Models;
using Microsoft.AspNetCore.Components;

namespace Blazor_WASM_MovieApp.Client.Pages.BaseComponents
{
    public class PersonIndexBase : ComponentBase
    {
        [CascadingParameter(Name = "ErrorComponent")]
        protected IErrorComponent ErrorComponent { get; set; }

        [Inject]
        protected NavigationManager _navigationManager { get; set; }

        [Inject]
        protected WASM_IPersonService _personService { get; set; }

        protected SearchPeople searchPeople = new SearchPeople();
        protected List<Person> people = new List<Person>();
        protected List<ErrorItem> ErrorList = new List<ErrorItem>();
        protected bool shouldRender = true;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                people = await _personService.GetPeople();
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

                people = await _personService.SearchPeople(searchPeople.SearchString);
                ErrorComponent.HideError();
                _navigationManager.NavigateTo("/personIndex");
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
