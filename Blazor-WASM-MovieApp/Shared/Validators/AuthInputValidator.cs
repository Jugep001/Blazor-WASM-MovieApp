using Blazor_WASM_MovieApp.Models;
using FluentValidation;

namespace Blazor_WASM_MovieApp.Validators
{
    public class AuthInputValidator : AbstractValidator<AuthInput>
    {
        

        public AuthInputValidator()
        {
            RuleFor(x => x.Username).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
            RuleFor(x => x.ConfirmPassword).NotEmpty();
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.PhoneNumber).NotEmpty();
            
        }

        //private bool IsDuplicate(string Title, int genreId)
        //{

        //    return _genreRepository.GenreExist(Title, genreId);

        //}

    }
}
