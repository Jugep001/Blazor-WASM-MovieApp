using Blazor_WASM_MovieApp.Exceptions;
using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;

namespace Blazor_WASM_MovieApp.Client.Pages.BaseComponents
{
    public class AddDescriptionComponentBase : ComponentBase
    {
        [CascadingParameter]
        protected BlazoredModalInstance ModalInstance { get; set; }

        [Parameter]
        public string? MovieDescription { get; set; }

        protected string? description;
        protected List<ErrorItem> ErrorList = new List<ErrorItem>();
        protected bool isErrorActive = false;

        protected override async Task OnInitializedAsync()
        {
            description = MovieDescription;
        }

        protected async Task SaveDescription()
        {
            try
            {               
                await ModalInstance.CloseAsync(ModalResult.Ok(description));
                return;

            }
            catch (BusinessException ex)
            {
                ErrorList = ex.ExceptionMessageList;
                isErrorActive = true;
            }

        }
    }
}
