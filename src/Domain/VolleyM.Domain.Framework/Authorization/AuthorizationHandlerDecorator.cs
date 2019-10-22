using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Serilog;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Contracts.Crosscutting;
using VolleyM.Domain.IdentityAndAccess.RolesAggregate;

namespace VolleyM.Domain.Framework.Authorization
{
    public class AuthorizationHandlerDecorator<TRequest, TResponse>
        : DecoratorBase<IRequestHandler<TRequest, TResponse>>, IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : class
    {
        private readonly IAuthorizationService _authZService;
        private readonly PermissionAttributeMappingStore _permissionAttributeMap;

        public AuthorizationHandlerDecorator(
            IRequestHandler<TRequest, TResponse> handler,
            IAuthorizationService authZService,
            PermissionAttributeMappingStore permissionAttributeMap)
            : base(handler)
        {
            _authZService = authZService;
            _permissionAttributeMap = permissionAttributeMap;
        }

        public async Task<Result<TResponse>> Handle(TRequest request)
        {
            var permissionToCheck = ExtractPermissionToCheck(RootInstance);

            if (permissionToCheck == null)
            {
                return Error.NotAuthorized("Handler does not have single permission attribute");
            }

            if (!(await _authZService.CheckAccess(permissionToCheck)))
            {
                return Error.NotAuthorized("No permission");
            }

            return await Decoratee.Handle(request);
        }

        private Permission ExtractPermissionToCheck(IRequestHandler<TRequest, TResponse> handler)
        {
            var type = GetRequestType(handler);

            if (type == null)
            {
                Log.Warning("Failed to obtain type of request for {HandlerType}", handler.GetType());
                return null;
            }
            
            PermissionAttribute GetPermissionAttribute(Type t)
            {
                var declaringType = handler.GetType().DeclaringType;

                if (declaringType == null) return null;

                var result = declaringType.GetCustomAttributes<PermissionAttribute>().SingleOrDefault();

                return result;
            }

            var attribute = _permissionAttributeMap.GetOrAdd(type, GetPermissionAttribute);

            if (attribute == null)
            {
                Log.Warning("Failed to obtain permission attribute for {HandlerType}", handler.GetType());
                return null;
            }

            return new Permission(attribute.Context, attribute.Action);
        }

        private static Type GetRequestType(IRequestHandler<TRequest, TResponse> handler)
        {
            var handlerInterface=handler.GetType().GetInterfaces()
                .SingleOrDefault(i=>i.Name==(typeof(IRequestHandler<,>).Name));

            if (handlerInterface == null)
            {
                return null;
            }

            return handlerInterface.GenericTypeArguments.FirstOrDefault();
        }
    }
}