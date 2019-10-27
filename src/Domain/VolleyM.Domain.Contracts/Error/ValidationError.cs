using FluentValidation.Results;

namespace VolleyM.Domain.Contracts
{
    public class ValidationError : Error
    {
        protected ValidationError(ValidationResult result) 
            : base(ErrorType.ValidationFailed, "Validation failed")
        {
            Result = result;
        }

        public ValidationResult Result { get; }
    }
}