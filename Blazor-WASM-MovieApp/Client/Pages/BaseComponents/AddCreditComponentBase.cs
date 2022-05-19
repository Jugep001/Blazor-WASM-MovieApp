using Blazor_WASM_MovieApp.Client.Services;
using Blazor_WASM_MovieApp.Exceptions;
using Blazor_WASM_MovieApp.Models;
using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;

namespace Blazor_WASM_MovieApp.Client.Pages.BaseComponents
{
    public class AddCreditComponentBase : ComponentBase
    {
        [CascadingParameter]
        protected BlazoredModalInstance ModalInstance { get; set; }

        [Parameter]
        public ICollection<Credit> CreditList { get; set; }

        [Inject]
        public WASM_ICreditService _creditService { get; set; }

        [Inject]
        public WASM_IPersonService _personService { get; set; }

        protected List<ErrorItem> ErrorList = new List<ErrorItem>();
        protected List<Function> functions = new List<Function>();
        protected List<Person> people = new List<Person>();

        protected bool isRoleRequired = false;
        protected Credit credit = new Credit { FunctionId = 1};
        protected bool isErrorActive = false;
        protected SearchPeople searchPeople = new SearchPeople();
        protected string Name = string.Empty;
        protected bool shouldRender = true;
        protected int functionId = 0;
        protected string role = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            functions = await _creditService.GetFunctions();
            people = await _personService.GetPeople();
            shouldRender = true;
        }

        protected override bool ShouldRender()
        {
            return shouldRender;
        }

        protected override void OnAfterRender(bool firstRender)
        {

            shouldRender = true;

        }


        protected async Task CreateCredit()
        {
            try
            {
                ErrorList.Clear();
                Person? person = await _personService.GetPersonByName(Name);
                credit.PersonId = person.Id;
                Function? function = await _creditService.GetFunction(credit.FunctionId);
                if (function.IsRoleRequired == false)
                {
                    credit.Role = null;
                }
                await _creditService.CreditExist(CreditList, credit.PersonId, credit.FunctionId, credit.Role, null);
                shouldRender = true;
                ModalInstance.CloseAsync(ModalResult.Ok(credit));
                return;

            }
            catch (BusinessException ex)
            {
                ErrorList = ex.ExceptionMessageList;
                isErrorActive = true;
                shouldRender = true;
            }

        }

        protected async void SetFunction(int id)
        {
            credit.FunctionId = id;
            Function? function = await _creditService.GetFunction(credit.FunctionId);
            

            if (function.IsRoleRequired)
            {
                isRoleRequired = true;
                shouldRender = true;
                StateHasChanged();
                return;
            }

            isRoleRequired = false;
            shouldRender = true;
        }
    }
}
