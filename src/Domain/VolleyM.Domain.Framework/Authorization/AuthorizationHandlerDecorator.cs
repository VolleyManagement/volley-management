using LanguageExt;
using System.Threading.Tasks;
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
        private readonly PermissionAttributeMappingStore _permissionAttributeMap;
        private readonly HandlerMetadataService _handlerMetadataService;

        public AuthorizationHandlerDecorator(
            IRequestHandler<TRequest, TResponse> handler,
            IAuthorizationService authZService,
            PermissionAttributeMappingStore permissionAttributeMap, HandlerMetadataService handlerMetadataService)
            : base(handler)
        {
            _authZService = authZService;
            _permissionAttributeMap = permissionAttributeMap;
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
            IRequestHandler<TRequest, TResponse> handler)
        {
            return _handlerMetadataService.GetHandlerMetadata(handler)
                .Map(m => new Permission(m.Context, m.Action));
        }
    }
}