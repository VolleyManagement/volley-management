using Serilog;
using System.Threading.Tasks;
using LanguageExt;
using VolleyM.Domain.Contracts;

namespace VolleyM.Domain.Framework.Logging
{
    public class LoggingQueryObjectDecorator<TParam, TResponse> : DecoratorBase<IQuery<TParam, TResponse>>, IQuery<TParam, TResponse>
        where TResponse : class
    {
        public LoggingQueryObjectDecorator(IQuery<TParam, TResponse> query) : base(query) { }

        public Task<Either<Error, TResponse>> Execute(TParam param)
        {
            var logger = Log.ForContext(RootInstance.GetType());

            logger.Debug("Executing query {@QueryParam}", param);
            var result = Decoratee.Execute(param);
            logger.Debug("Query execution complete.");

            return result;
        }
    }
}