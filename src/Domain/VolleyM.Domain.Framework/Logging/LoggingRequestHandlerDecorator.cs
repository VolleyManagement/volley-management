using Serilog;
using System.Threading.Tasks;
using VolleyM.Domain.Contracts;

namespace VolleyM.Domain.Framework.Logging
{
    public class LoggingRequestHandlerDecorator<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse: class
    {
        private readonly IRequestHandler<TRequest, TResponse> _handler;

        public LoggingRequestHandlerDecorator(IRequestHandler<TRequest, TResponse> handler) => _handler = handler;

        public Task<Result<TResponse>> Handle(TRequest request)
        {
            var logger = Log.ForContext(_handler.GetType());

            logger.Debug("Handling request {@Request}", request);
            var result = _handler.Handle(request);
            logger.Debug("Request handling complete.");

            return result;
        }
    }
}