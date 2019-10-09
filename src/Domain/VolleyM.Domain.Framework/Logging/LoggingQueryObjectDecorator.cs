using Serilog;
using System.Threading.Tasks;
using VolleyM.Domain.Contracts;

namespace VolleyM.Domain.Framework.Logging
{
    public class LoggingQueryObjectDecorator<TParam, TResponse> : IQuery<TParam, TResponse>
        where TResponse: class
    {
        private readonly IQuery<TParam, TResponse> _query;

        public LoggingQueryObjectDecorator(IQuery<TParam, TResponse> query) => _query = query;

        public Task<Result<TResponse>> Execute(TParam param)
        {
            var logger = Log.ForContext(_query.GetType());

            logger.Debug("Executing query {@QueryParam}", param);
            var result = _query.Execute(param);
            logger.Debug("Query execution complete.");

            return result;
        }
    }
}