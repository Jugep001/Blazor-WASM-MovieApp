using Blazor_WASM_MovieApp.Exceptions;
using Blazor_WASM_MovieApp.Models;
using Blazor_WASM_MovieApp.Repositories;

namespace Blazor_WASM_MovieApp.Services
{
    public class CreditService
    {
        private List<ErrorItem> errors = new List<ErrorItem>();
        private readonly CreditRepository _creditRepository;

        public CreditService(CreditRepository creditRepository)
        {

            _creditRepository = creditRepository;

        }

        public void AddCredit(Credit credit)
        {
            if(credit.PersonId == null)
            {
                errors.Add(new ErrorItem("PersonId", "Keine Person übergeben!"));
                throw new BusinessException(errors);
            }
            if (credit.FunctionId == null)
            {
                errors.Add(new ErrorItem("FunctionId", "Keine Funktion übergeben!"));
                throw new BusinessException(errors);
            }
            if (credit.Role == string.Empty)
            {
                errors.Add(new ErrorItem("Role", "Bitte Rolle eingeben!"));
                throw new BusinessException(errors);
            }

            _creditRepository.AddCredit(credit);
        }

        public void AddCredit_Create(Credit credit)
        {
            if (credit.PersonId == null)
            {
                errors.Add(new ErrorItem("PersonId", "Keine Person übergeben!"));
                throw new BusinessException(errors);
            }
            if (credit.FunctionId == null)
            {
                errors.Add(new ErrorItem("FunctionId", "Keine Funktion übergeben!"));
                throw new BusinessException(errors);
            }
            if (credit.Role == string.Empty)
            {
                errors.Add(new ErrorItem("Role", "Bitte Rolle eingeben!"));
                throw new BusinessException(errors);
            }

            _creditRepository.AddCredit_Create(credit);
        }

        public void UpdateCredit(Credit credit)
        {
            errors = new List<ErrorItem>();

            if (credit.PersonId == null)
            {
                errors.Add(new ErrorItem("PersonId", "Keine Person übergeben!"));
                throw new BusinessException(errors);
            }
            if (credit.FunctionId == null)
            {
                errors.Add(new ErrorItem("FunctionId", "Keine Funktion übergeben!"));
                throw new BusinessException(errors);
            }

            if (!_creditRepository.CreditExist(credit.Id.Value))
            {
                errors.Add(new ErrorItem("CreditId", "Diesen Crediteintrag gibt es nicht!"));
                throw new BusinessException(errors);
            }

            _creditRepository.UpdateCredit(credit);
        }

        public void DeleteCredit(int creditId)
        {
            errors = new List<ErrorItem>();

            if (!_creditRepository.CreditExist(creditId))
            {
                errors.Add(new ErrorItem("CreditId", "Diesen Crediteintrag gibt es nicht!"));
                throw new BusinessException(errors);
            }

            _creditRepository.DeleteCredit(creditId);
        }

        public List<Credit> GetCredits()
        {
            return _creditRepository.GetCredits();
        }

        public Credit GetCredit(int creditId)
        {
            errors = new List<ErrorItem>();

            if (!_creditRepository.CreditExist(creditId))
            {
                errors.Add(new ErrorItem("CreditId", "Diesen Crediteintrag gibt es nicht!"));
                throw new BusinessException(errors);
            }

            return _creditRepository.GetCredit(creditId);
        }

        public List<Function> GetFunctions()
        {
            return _creditRepository.GetFunctions();
        }

        public Function GetFunction(int functionId)
        {
            errors = new List<ErrorItem>();

            if (functionId == 0)
            {
                errors.Add(new ErrorItem("FunctionId", "Diese Funktion gibt es nicht!"));
                throw new BusinessException(errors);
            }

            return _creditRepository.GetFunction(functionId);
        }

        public List<Credit> GetCreditsByName(string searchString)
        {
            return _creditRepository.GetCreditsByName(searchString);
        }

        public List<ErrorItem> CreditExist(ICollection<Credit> credits, int personId, int functionId, string role, Credit? oldCredit)
        {
            errors = new List<ErrorItem>();

            if (personId == 0)
            {
                errors.Add(new ErrorItem("PersonId", "Keine Person ausgewählt!"));
                return errors;
            }

            if(functionId == 0)
            {
                errors.Add(new ErrorItem("FunctionId", "Keine Funktion ausgewählt!"));
                return errors;
            }

            if (role == null && _creditRepository.GetFunction(functionId).IsRoleRequired)
            {
                errors.Add(new ErrorItem("Role", "Bitte Rolle eingeben!"));
                return errors;
            }

            if (_creditRepository.CreditExist(credits, personId, functionId, role, oldCredit) == true)
            {
                errors.Add(new ErrorItem("Credit", "Doppelter Crediteintrag!"));
                return errors;
            }
            return errors;
        }

        public List<Credit> GetCreditsFromMovie(int movieId)
        {
            return _creditRepository.GetCreditsFromMovie(movieId);
        }


    }
}
