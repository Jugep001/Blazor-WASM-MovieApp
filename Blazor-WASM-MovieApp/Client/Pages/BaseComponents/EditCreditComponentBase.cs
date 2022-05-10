using Blazor_WASM_MovieApp.Client.Services;
using Blazor_WASM_MovieApp.Exceptions;
using Blazor_WASM_MovieApp.Models;
using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;

namespace Blazor_WASM_MovieApp.Client.Pages.BaseComponents
{
    public class EditCreditComponentBase : ComponentBase
    {
        [CascadingParameter]
        protected BlazoredModalInstance ModalInstance { get; set; }

        [Parameter]
        public ICollection<Credit> CreditList { get; set; }

        [Parameter]
        public Credit OldCredit { get; set; }

        [Inject]
        protected WASM_ICreditService _creditService { get; set; }

        [Inject]
        protected WASM_IPersonService _personService { get; set; }

        protected List<ErrorItem> ErrorList = new List<ErrorItem>();
        protected List<Function> functions = new List<Function>();
        protected List<Person> people = new List<Person>();

        protected Credit credit = new Credit();
        protected Function functionEdit = new Function();
        protected Person personEdit = new Person();

        protected bool isRoleRequired = false;
        protected bool isErrorActive = false;
        protected SearchPeople searchPeople = new SearchPeople();
        protected string Name = string.Empty;
        protected bool shouldRender = true;

        protected override async Task OnInitializedAsync()
        {
            credit.FunctionId = OldCredit.FunctionId;
            credit.PersonId = OldCredit.PersonId;
            credit.Role = OldCredit.Role;

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


        protected async void CreateCredit()
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
                _creditService.CreditExist(CreditList, credit.PersonId, credit.FunctionId, credit.Role, OldCredit);
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

        protected async Task GetRoleRequired()
        {
            Function? function = await _creditService.GetFunction(credit.FunctionId);

            if (function.IsRoleRequired)
            {
                isRoleRequired = true;
                shouldRender = true;
            }

            isRoleRequired = false;
            shouldRender = true;
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
