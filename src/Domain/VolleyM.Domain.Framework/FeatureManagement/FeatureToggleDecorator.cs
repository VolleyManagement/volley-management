using System;
using System.Reflection;
using System.Linq;
using System.Threading.Tasks;
using Esquio.Abstractions;
using LanguageExt;
using Serilog;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Contracts.Crosscutting;
using VolleyM.Domain.Framework.Authorization;

namespace VolleyM.Domain.Framework.FeatureManagement
{
    public class FeatureToggleDecorator<TRequest, TResponse>
        : DecoratorBase<IRequestHandler<TRequest, TResponse>>, IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IFeatureService _featureService;
        private readonly PermissionAttributeMappingStore _permissionAttributeMap;

        public FeatureToggleDecorator(IRequestHandler<TRequest, TResponse> decoratee, IFeatureService featureService, PermissionAttributeMappingStore permissionAttributeMap)
            : base(decoratee)
        {
            _featureService = featureService;
            _permissionAttributeMap = permissionAttributeMap;
        }

        public async Task<Either<Error, TResponse>> Handle(TRequest request)
        {
            var featureInfo = GetFeatureInfo(RootInstance);

            var featureState = await _featureService.IsEnabledAsync(featureInfo?.Action, featureInfo?.Context);

            if (featureState)
            {
                return await Decoratee.Handle(request);
            }

            return Error.FeatureDisabled();
        }

        private (string Context, string Action)? GetFeatureInfo(IRequestHandler<TRequest, TResponse> handler)
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

            return (attribute.Context, attribute.Action);
        }
        private static Type GetRequestType(IRequestHandler<TRequest, TResponse> handler)
        {
            var handlerInterface = handler.GetType().GetInterfaces()
                .SingleOrDefault(i => i.Name == (typeof(IRequestHandler<,>).Name));

            if (handlerInterface == null)
            {
                return null;
            }

            return handlerInterface.GenericTypeArguments.FirstOrDefault();
        }
    }
}