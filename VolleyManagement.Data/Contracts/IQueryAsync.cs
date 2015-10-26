namespace VolleyManagement.Data.Contracts
{
    using System.Threading.Tasks;

    /// <summary>
    /// Used to retrieve entities by specified query in an asynchronous way
    /// </summary>
    /// <typeparam name="TResult">Type of result value</typeparam>
    /// <typeparam name="TCriterion">Type of query parameters</typeparam>
    public interface IQueryAsync<TResult, in TCriterion> where TCriterion : IQueryCriteria
    {
        /// <summary>
        /// Executes given query using criteria
        /// </summary>
        /// <param name="criteria">Query parameters</param>
        /// <returns>Query result</returns>
        Task<TResult> ExecuteAsync(TCriterion criteria);
    }
}