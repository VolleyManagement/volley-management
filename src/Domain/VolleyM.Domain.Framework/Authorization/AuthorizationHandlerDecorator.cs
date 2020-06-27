using LanguageExt;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Framework.HandlerMetadata;
using VolleyM.Domain.IdentityAndAccess.RolesAggregate;

namespace VolleyM.Domain.Framework.Authorization
{
	public class AuthorizationHandlerDecorator<TRequest, TResponse>
		: DecoratorBase<IRequestHandler<TRequest, TResponse>>, IRequestHandler<TRequest, TResponse>
		where TRequest : IRequest<TResponse>
	{
		private readonly IAuthorizationService _authZService;
		private readonly HandlerMetadataService _handlerMetadataService;

		public AuthorizationHandlerDecorator(
			IRequestHandler<TRequest, TResponse> handler,
			IAuthorizationService authZService,
			HandlerMetadataService handlerMetadataService)
			: base(handler)
		{
			_authZService = authZService;
			_handlerMetadataService = handlerMetadataService;
		}

		public EitherAsync<Error, TResponse> Handle(TRequest request)
		{
			var permissionEnabled = ExtractPermissionToCheck(RootInstance)
				.MapAsync(async perm => await _authZService.CheckAccess(perm))
				.Map(p => p.Match(
						Right: allowed => allowed
							? (Either<Error, Unit>)Unit.Default
							: Error.NotAuthorized("No permission"),
						Left: e => e
					))
				.ToAsync();

			return permissionEnabled.Bind(_ => Decoratee.Handle(request));
		}

		private Either<Error, Permission> ExtractPermissionToCheck(
			IRequestHandler<TRequest, TResponse> handler)
		{
			return _handlerMetadataService.GetHandlerMetadata(handler)
				.Map(m => new Permission(m.Context, m.Action));
		}
	}
}