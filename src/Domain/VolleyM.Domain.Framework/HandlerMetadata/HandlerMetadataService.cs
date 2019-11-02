using LanguageExt;
using VolleyM.Domain.Contracts;

namespace VolleyM.Domain.Framework.HandlerMetadata
{
    /// <summary>
    /// Provides runtime information related to handler structure
    /// </summary>
    public class HandlerMetadataService
    {
        public Either<Error, HandlerMetadata> GetHandlerMetadata<TRequest, TResponse>(IRequestHandler<TRequest, TResponse> handler) 
            where TRequest : IRequest<TResponse>
        {
            return Error.DesignViolation("Handler is allowed to implement only one IRequestHandler");
        }
    }
}