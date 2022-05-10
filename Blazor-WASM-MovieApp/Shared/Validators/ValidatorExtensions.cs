using Blazor_WASM_MovieApp.Exceptions;
using FluentValidation;

namespace Blazor_WASM_MovieApp.Validators
{
    public static class ValidatorExtensions
    {
        public static void ValidateAndThrowBusinessException<T>(this AbstractValidator<T> validator, T objectToValidate)
        {

            List<ErrorItem> errors = new List<ErrorItem>();

            var results = validator.Validate(objectToValidate);
            if (!results.IsValid)
            {

                foreach (var failure in results.Errors)
                {

                    errors.Add(new ErrorItem(failure.PropertyName, failure.ErrorMessage));

                }
                throw new BusinessException(errors);
            }


        }
    }
}
