using Blazor_WASM_MovieApp.Exceptions;
using Blazor_WASM_MovieApp.Models;
using Blazor_WASM_MovieApp.Repositories;
using Blazor_WASM_MovieApp.Shared.Models;
using Blazor_WASM_MovieApp.Validators;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

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

        public void UpdateUser(AuthInput authInput)
        {
            _authenticationRepository.UpdateUser(authInput);
        }

        public void DeleteUser(string id)
        {
            _authenticationRepository.DeleteUser(id);
        }

        public async Task<AuthInput> Login(AuthInput authInput)
        {
            return await _authenticationRepository.Login(authInput);
        }

        public async Task Register(AuthInput authInput)
        {
            await _authenticationRepository.Register(authInput);
        }



    }
}
