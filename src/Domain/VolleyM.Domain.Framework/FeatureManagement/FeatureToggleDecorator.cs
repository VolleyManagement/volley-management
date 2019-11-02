using System;
using System.Reflection;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Esquio.Abstractions;
using LanguageExt;
using Serilog;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Contracts.Crosscutting;
using VolleyM.Domain.Framework.Authorization;
using VolleyM.Domain.Framework.HandlerMetadata;

namespace VolleyM.Domain.Framework.FeatureManagement
{
    public class FeatureToggleDecorator<TRequest, TResponse>
        : DecoratorBase<IRequestHandler<TRequest, TResponse>>, IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IFeatureService _featureService;
        private readonly PermissionAttributeMappingStore _permissionAttributeMap;
        private readonly HandlerMetadataService _handlerMetadataService;

        public FeatureToggleDecorator(IRequestHandler<TRequest, TResponse> decoratee, IFeatureService featureService, PermissionAttributeMappingStore permissionAttributeMap, HandlerMetadataService handlerMetadataService)
            : base(decoratee)
        {
            _featureService = featureService;
            _permissionAttributeMap = permissionAttributeMap;
            _handlerMetadataService = handlerMetadataService;
        }

        public async Task<Either<Error, TResponse>> Handle(TRequest request)
        {
            var isEnabled = await GetFeatureInfo(RootInstance)
                .MapAsync(async fi => await _featureService.IsEnabledAsync(fi.Action, fi.Context));

            return await isEnabled.MatchAsync<Either<Error, TResponse>>(
                Left: e => e,
                RightAsync: async enabled => enabled
                        ? await Decoratee.Handle(request)
                        : Error.FeatureDisabled());
        }


        private Either<Error, (string Context, string Action)> GetFeatureInfo(
            IRequestHandler<TRequest, TResponse> handler)
        {
            return _handlerMetadataService.GetHandlerMetadata(handler)
                .Map(m => (m.Context, m.Action));
        }

        private (string Context, string Action)? GetFeatureInfo1(IRequestHandler<TRequest, TResponse> handler)
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