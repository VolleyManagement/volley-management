using System;
using System.Threading.Tasks;
using FluentValidation;
using LanguageExt;
using VolleyM.Domain.Contracts;

namespace VolleyM.Domain.Framework.Validation
{
	[Obsolete]
	public class ValidationHandlerDecoratorOld<TRequest, TResponse>
		: DecoratorBase<IRequestHandlerOld<TRequest, TResponse>>, IRequestHandlerOld<TRequest, TResponse>
		where TRequest : IRequest<TResponse>
	{
		private readonly IValidator<TRequest> _validator;

		public ValidationHandlerDecoratorOld(IRequestHandlerOld<TRequest, TResponse> decoratee, IValidator<TRequest> validator)
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