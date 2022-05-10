using Blazor_WASM_MovieApp.Models;
using FluentValidation;

namespace Blazor_WASM_MovieApp.Validators
{

    public class GenreValidator : AbstractValidator<Genre>
    {

        public GenreValidator()
        {

            RuleFor(genre => genre.Name).NotEmpty().WithMessage("Der Name fehlt!");

            //RuleFor(genre => genre.Name).Must((genre, Name) => !IsDuplicate(genre.Name, genre.Id)).WithMessage("Dieses Genre gibt es bereits!");
        }

        //private bool IsDuplicate(string Title, int genreId)
        //{

        //    return _genreRepository.GenreExist(Title, genreId);

        //}
    }

}
