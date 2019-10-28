using FluentValidation.Results;

namespace VolleyM.Domain.Contracts
{
    public class ValidationError : Error
    {
        public ValidationError(ValidationResult result) 
            : base(ErrorType.ValidationFailed, "Validation failed")
        {
            Result = result;
        }

        public ValidationResult Result { get; }
    }
}