using System.Threading.Tasks;
using Serilog;

namespace VolleyM.Domain.Contracts.Crosscutting
{
    public class LoggingQueryObjectDecorator<TParam, TResponse> : IQuery<TParam, TResponse>
    {
        private readonly IQuery<TParam, TResponse> _query;

        public LoggingQueryObjectDecorator(IQuery<TParam, TResponse> query) => _query = query;

        public Task<TResponse> Execute(TParam param)
        {
            var logger = Log.ForContext(_query.GetType());

            logger.Debug("Executing query {@QueryParam}", param);
            var result = _query.Execute(param);
            logger.Debug("Query execution complete.");

            return result;
        }
    }
}