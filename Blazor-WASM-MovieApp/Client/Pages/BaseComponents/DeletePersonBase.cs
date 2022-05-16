using Blazor_WASM_MovieApp.Client.Services;
using Blazor_WASM_MovieApp.Exceptions;
using Blazor_WASM_MovieApp.Models;
using Microsoft.AspNetCore.Components;

namespace Blazor_WASM_MovieApp.Client.Pages.BaseComponents
{
    public class DeletePersonBase : ComponentBase
    {
        [Parameter]
        public int Id { get; set; }

        [Inject]
        protected WASM_IPersonService _personService { get; set; }

        [Inject]
        protected NavigationManager _navigationManager { get; set; }

        [CascadingParameter(Name = "ErrorComponent")]
        protected IErrorComponent ErrorComponent { get; set; }

        protected List<ErrorItem> ErrorList = new List<ErrorItem>();
        protected Person person = new Person();
        protected bool isErrorActive = false;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                person = await _personService.GetPerson(Id);
            }
            catch (BusinessException ex)
            {
                ErrorList = ex.ExceptionMessageList;
                ErrorComponent.ShowError(ErrorList);
            }

        }

        protected async Task DeletePersonById()
        {
            try
            {
                await _personService.DeletePerson(Id);
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
            ErrorComponent.HideError();
            _navigationManager.NavigateTo("/personIndex");
        }
    }
}
