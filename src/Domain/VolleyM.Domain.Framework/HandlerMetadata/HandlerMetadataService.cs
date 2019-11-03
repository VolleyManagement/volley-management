using System;
using System.Collections.Concurrent;
using System.Linq;
using LanguageExt;
using VolleyM.Domain.Contracts;

namespace VolleyM.Domain.Framework.HandlerMetadata
{
    /// <summary>
    /// Provides runtime information related to handler structure
    /// </summary>
    public class HandlerMetadataService
    {
        private readonly ConcurrentDictionary<Type, HandlerMetadata> _handlerMetadataCache
            = new ConcurrentDictionary<Type, HandlerMetadata>();

        public Either<Error, HandlerMetadata> GetHandlerMetadata<TRequest, TResponse>(IRequestHandler<TRequest, TResponse> handler)
            where TRequest : IRequest<TResponse>
        {
            return from requestType in GetRequestType(handler)
                   from metadata in GetOrCreateMetadata(handler, requestType)
                   select metadata;
        }

        private static Either<Error, Type> GetRequestType<TRequest, TResponse>(IRequestHandler<TRequest, TResponse> handler)
            where TRequest : IRequest<TResponse>
        {
            var handlerInterfaces = handler.GetType().GetInterfaces()
                .Where(IsIRequestHandler<TRequest, TResponse>)
                .ToArray();

            if (handlerInterfaces.Length > 1)
            {
                return Error.DesignViolation("Handler is allowed to implement only one IRequestHandler");
            }

            return handlerInterfaces[0].GenericTypeArguments.First();
        }

        private Either<Error, HandlerMetadata> GetOrCreateMetadata(object handler, Type requestType)
        {
            Either<Error, HandlerMetadata> result;

            if (!_handlerMetadataCache.TryGetValue(requestType, out var metadata))
            {
                result = GetHandlerMetadata(handler, requestType);

                result.Match(
                    Right: m => _handlerMetadataCache
                        .AddOrUpdate(requestType, m, (_, __) => m),
                    Left: _ => { });
            }
            else
            {
                result = metadata;
            }

            return result;
        }

        private Either<Error, HandlerMetadata> GetHandlerMetadata(object handler, Type requestType)
        {
            var declaringType = handler.GetType().DeclaringType;

            if (declaringType == null)
                return Error.DesignViolation("Handler should be nested in a class to group handler related classes together");

            return new HandlerMetadata
            {
                Action = declaringType.Name,
                Context = GetContextFromNS(declaringType.Namespace)
            };
        }

        private string GetContextFromNS(string declaringTypeNamespace)
        {
            // usually handlers will have NS: VolleyM.Domain.<Context>.<Handler>
            // any other schemes are not supported yet
            return declaringTypeNamespace.Split('.')[2];
        }

        private static bool IsIRequestHandler<TRequest, TResponse>(Type interfaceType) where TRequest : IRequest<TResponse>
        {
            var name = typeof(IRequestHandler<,>).Name;
            return interfaceType.Name == name;
        }

        public void OverrideHandlerMetadata<T>(HandlerMetadata handlerMetadata)
        {
            _handlerMetadataCache.AddOrUpdate(typeof(T), handlerMetadata, (_, __) => handlerMetadata);
        }
    }
}