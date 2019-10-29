using System.Threading.Tasks;
using FluentValidation;
using LanguageExt;
using VolleyM.Domain.Contracts;

namespace VolleyM.Domain.Framework.Validation
{
    public class ValidationHandlerDecorator<TRequest, TResponse>
        : DecoratorBase<IRequestHandler<TRequest, TResponse>>, IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IValidator<TRequest> _validator;

        public ValidationHandlerDecorator(IRequestHandler<TRequest, TResponse> decoratee, IValidator<TRequest> validator)
            : base(decoratee)
        {
            _validator = validator;
        }

        public Task<Either<Error, TResponse>> Handle(TRequest request)
        {
            var validationResult = _validator.Validate(request);

            if (!validationResult.IsValid)
            {
                return Task.FromResult<Either<Error, TResponse>>(new ValidationError(validationResult));
            }

            return base.Decoratee.Handle(request);
        }
    }
}