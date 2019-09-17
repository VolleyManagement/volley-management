using System.Threading.Tasks;

namespace VolleyM.Domain.Contracts
{
    /// <summary>
    /// Defines a handler for a request
    /// </summary>
    /// <typeparam name="TRequest">The type of request being handled</typeparam>
    /// <typeparam name="TResponse">The type of response from the handler</typeparam>
    public interface IRequestHandler<in TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse: class
    {
        /// <summary>
        /// Handles a request
        /// </summary>
        /// <param name="request">The request</param>
        /// <returns>Response from the request</returns>
        Task<Result<TResponse>> Handle(TRequest request);
    }
}