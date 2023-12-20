using LanguageExt;

namespace VolleyM.Domain.Contracts
{
	/// <summary>
	/// Represents Query object to be executed against persistence layer
	/// </summary>
	/// <typeparam name="TParam">Query parameters</typeparam>
	/// <typeparam name="TResult">Query result</typeparam>
	public interface IQuery<in TParam, TResult>
		where TResult : class
	{
		EitherAsync<Error, TResult> Execute(TParam param);
	}
}