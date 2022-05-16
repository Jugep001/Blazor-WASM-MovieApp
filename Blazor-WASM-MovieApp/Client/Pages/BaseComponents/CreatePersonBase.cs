using Blazor_WASM_MovieApp.Client.Services;
using Blazor_WASM_MovieApp.Exceptions;
using Blazor_WASM_MovieApp.Models;
using Microsoft.AspNetCore.Components;

namespace Blazor_WASM_MovieApp.Client.Pages.BaseComponents
{
    public class CreatePersonBase : ComponentBase
    {
        [Inject]
        protected WASM_IPersonService _personService { get; set; }

        [Inject]
        protected NavigationManager _navigationManager { get; set; }

        [CascadingParameter(Name = "ErrorComponent")]
        protected IErrorComponent ErrorComponent { get; set; }

        protected List<ErrorItem> ErrorList = new List<ErrorItem>();
        protected Person person = new Person();
        protected bool isErrorActive = false;


        protected async Task AddPerson()
        {
            try
            {
                await _personService.AddPerson(person);
                ErrorComponent.HideError();
                _navigationManager.NavigateTo("/personIndex");

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


            _navigationManager.NavigateTo("/personIndex");
            ErrorComponent.HideError();

        }
    }
}

