﻿using System.Threading.Tasks;
using LanguageExt;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Framework.HandlerMetadata;
using VolleyM.Domain.IdentityAndAccess.RolesAggregate;

namespace VolleyM.Domain.Framework.Authorization
{
	public class AuthorizationHandlerDecoratorOld<TRequest, TResponse>
		: DecoratorBase<IRequestHandlerOld<TRequest, TResponse>>, IRequestHandlerOld<TRequest, TResponse>
		where TRequest : IRequest<TResponse>
	{
		private readonly IAuthorizationService _authZService;
		private readonly HandlerMetadataService _handlerMetadataService;

		public AuthorizationHandlerDecoratorOld(
			IRequestHandlerOld<TRequest, TResponse> handler,
			IAuthorizationService authZService,
			HandlerMetadataService handlerMetadataService)
			: base(handler)
		{
			_authZService = authZService;
			_handlerMetadataService = handlerMetadataService;
		}

		public async Task<Either<Error, TResponse>> Handle(TRequest request)
		{
			var permissionEnabled = await ExtractPermissionToCheck(RootInstance)
				.MapAsync(async perm => await _authZService.CheckAccess(perm));

			return await permissionEnabled.MatchAsync<Either<Error, TResponse>>(
				Left: e => e,
				RightAsync: async allowed => allowed
					? await Decoratee.Handle(request)
					: Error.NotAuthorized("No permission"));
		}

		private Either<Error, Permission> ExtractPermissionToCheck(
			IRequestHandlerOld<TRequest, TResponse> handler)
		{
			return _handlerMetadataService.GetHandlerMetadataOld(handler)
				.Map(m => new Permission(m.Context, m.Action));
		}
	}
}