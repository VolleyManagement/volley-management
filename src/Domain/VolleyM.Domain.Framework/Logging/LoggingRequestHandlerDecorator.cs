﻿using Serilog;
using System.Threading.Tasks;
using LanguageExt;
using VolleyM.Domain.Contracts;

namespace VolleyM.Domain.Framework.Logging
{
    public class LoggingRequestHandlerDecorator<TRequest, TResponse> : DecoratorBase<IRequestHandlerOld<TRequest, TResponse>>, IRequestHandlerOld<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        public LoggingRequestHandlerDecorator(IRequestHandlerOld<TRequest, TResponse> handler)
            : base(handler) { }

        public Task<Either<Error, TResponse>> Handle(TRequest request)
        {
            var logger = Log.ForContext(RootInstance.GetType());

            logger.Debug("Handling request {@Request}", request);
            var result = Decoratee.Handle(request);
            logger.Debug("Request handling complete.");

            return result;
        }
    }
}