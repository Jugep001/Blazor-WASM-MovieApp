using Blazor_WASM_MovieApp.Models;
using FluentValidation;

namespace Blazor_WASM_MovieApp.Validators
{
    public class MovieValidator : AbstractValidator<Movie>
    {

        public MovieValidator()
        {

            RuleFor(movie => movie.Title)
                .NotEmpty().WithMessage("Der Titel fehlt!");

            RuleFor(movie => movie.ReleaseDate)
                .GreaterThan(new DateTime(1900, 1, 1)).WithMessage("Das Veröffentlichungsdatum ist zu klein!")
                .LessThan(DateTime.Now.AddYears(1)).WithMessage("Das Veröffentlichungsdatum darf maximal ein Jahr in der Zukunft liegen!")
                .NotNull().WithMessage("Das Veröffentlichungsdatum fehlt!");


            //RuleFor(movie => movie.Title).Must((movie, Title) => !IsDuplicate(movie.Title, movie.ReleaseDate ?? new DateTime(), movie.Id)).WithMessage("Den Film gibt es bereits!");



        }

        //private bool IsDuplicate(string title, DateTime releaseDate, int id)
        //{

        //    return _movieRepository.MovieExist(title, releaseDate, id);

        //}
    }
}
