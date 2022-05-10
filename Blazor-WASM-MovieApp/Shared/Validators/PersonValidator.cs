using Blazor_WASM_MovieApp.Models;
using FluentValidation;

namespace Blazor_WASM_MovieApp.Validators
{
    public class PersonValidator : AbstractValidator<Person>
    {


            public PersonValidator()
            {

                RuleFor(person => person.Name).NotEmpty().WithMessage("Der Name fehlt!");
                RuleFor(person => person.Name).MinimumLength(3).WithMessage("Der Name ist zu kurz!");
                RuleFor(person => person.Vorname).NotEmpty().WithMessage("Der Vorname fehlt!");
                RuleFor(person => person.Vorname).MinimumLength(3).WithMessage("Der Name ist zu kurz!");
                //RuleFor(person => person.Vorname).Must((person, Vorname) => !IsDuplicate(person.Vorname, person.Name, person.Id)).WithMessage("Die exakt selbe Person gibt es bereits!");

            }
            //private bool IsDuplicate(string vorname, string name, int id)
            //{

            //    return _personRepository.PersonExist(vorname, name, id);

            //}

        
    }
}
