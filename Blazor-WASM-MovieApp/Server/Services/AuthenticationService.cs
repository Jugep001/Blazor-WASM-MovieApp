using Blazor_WASM_MovieApp.Exceptions;
using Blazor_WASM_MovieApp.Models;
using Blazor_WASM_MovieApp.Repositories;
using Blazor_WASM_MovieApp.Validators;
using Microsoft.AspNetCore.Identity;

namespace Blazor_WASM_MovieApp.Services
{
    public class AuthenticationService
    {

        private List<ErrorItem> errors = new List<ErrorItem>();
        private readonly AuthenticationRepository _authenticationRepository;
        private readonly AuthInputValidator _authInputValidator;

        public AuthenticationService(AuthenticationRepository authenticationRepository, AuthInputValidator authInputValidator)
        {
            _authenticationRepository = authenticationRepository;
            _authInputValidator = authInputValidator;

        }

        public List<IdentityUser> GetUsers()
        {
            return _authenticationRepository.GetUsers();
        }

        public IdentityUser GetUser(string id)
        {
            return _authenticationRepository.GetUser(id);
        }

        public void AddUser(IdentityUser user, AuthInput authInput, string role)
        {
            errors = new List<ErrorItem>();
            _authInputValidator.ValidateAndThrowBusinessException(authInput);
            _authenticationRepository.AddUser(user, role);
        }

        public void UpdateUser(IdentityUser user, AuthInput authInput)
        {
            _authenticationRepository.UpdateUser(user, authInput);
        }

        public void DeleteUser(IdentityUser user)
        {
            _authenticationRepository.DeleteUser(user);
        }

    }
}
