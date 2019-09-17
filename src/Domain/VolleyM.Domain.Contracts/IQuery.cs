using System.Threading.Tasks;

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
        Task<Result<TResult>> Execute(TParam param);
    }
}
