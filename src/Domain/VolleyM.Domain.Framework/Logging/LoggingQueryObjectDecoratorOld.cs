using System;
using System.Threading.Tasks;
using LanguageExt;
using Serilog;
using VolleyM.Domain.Contracts;

namespace VolleyM.Domain.Framework.Logging
{
	[Obsolete]
	public class LoggingQueryObjectDecoratorOld<TParam, TResponse> : DecoratorBase<IQueryOld<TParam, TResponse>>, IQueryOld<TParam, TResponse>
		where TResponse : class
	{
		public LoggingQueryObjectDecoratorOld(IQueryOld<TParam, TResponse> query) : base(query) { }

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