﻿using System;
using System.Collections.Concurrent;
using System.Linq;
using FluentValidation;
using LanguageExt;
using VolleyM.Domain.Contracts;

namespace VolleyM.Domain.Framework.HandlerMetadata
{
    /// <summary>
    /// Provides runtime information related to handler structure
    /// </summary>
    public class HandlerMetadataService
    {
        private readonly ConcurrentDictionary<Type, HandlerInfo> _handlerMetadataCache
            = new ConcurrentDictionary<Type, HandlerInfo>();

        public Either<Error, HandlerInfo> GetHandlerMetadata<TRequest, TResponse>(IRequestHandlerOld<TRequest, TResponse> handler)
            where TRequest : IRequest<TResponse>
        {
            return from requestType in GetRequestType(handler)
                   from metadata in GetOrCreateMetadata(handler, requestType)
                   select metadata;
        }

        public bool HasValidator(Type handlerType)
        {
            return handlerType.DeclaringType? //Type which hosts all handler related classes
                .GetNestedTypes() //Find classes that implement IValidator<>
                .Any(t => t.GetInterfaces()
                    .Any(i => i.Name == typeof(IValidator<>).Name)) ?? false;
        }

        private static Either<Error, Type> GetRequestType<TRequest, TResponse>(IRequestHandlerOld<TRequest, TResponse> handler)
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

        private Either<Error, HandlerInfo> GetOrCreateMetadata(object handler, Type requestType)
        {
            Either<Error, HandlerInfo> result;

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

        private Either<Error, HandlerInfo> GetHandlerMetadata(object handler, Type requestType)
        {
            var declaringType = handler.GetType().DeclaringType;

            if (declaringType == null)
                return Error.DesignViolation("Handler should be nested in a class to group handler related classes together");


            return GetContextFromNS(declaringType.Namespace)
                .Map(context => new HandlerInfo(context, declaringType.Name));
        }

        private Either<Error, string> GetContextFromNS(string declaringTypeNamespace)
        {
            var parts = declaringTypeNamespace.Split('.');

            if (parts.Length < 3)
            {
                return Error.DesignViolation("Handler should be defined inside VolleyM.Domain.Context namespace");
            }

            // usually handlers will have NS: VolleyM.Domain.<Context>.<Handler>
            // any other schemes are not supported yet
            return parts[2];
        }

        private static bool IsIRequestHandler<TRequest, TResponse>(Type interfaceType) where TRequest : IRequest<TResponse>
        {
            var name = typeof(IRequestHandlerOld<,>).Name;
            return interfaceType.Name == name;
        }

        public void OverrideHandlerMetadata<T>(HandlerInfo handlerInfo)
        {
            OverrideHandlerMetadata(typeof(T), handlerInfo);
        }

        private void OverrideHandlerMetadata(Type requestType, HandlerInfo handlerInfo)
        {
            _handlerMetadataCache.AddOrUpdate(requestType, handlerInfo, (_, __) => handlerInfo);
        }
    }
}