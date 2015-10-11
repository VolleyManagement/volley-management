namespace VolleyManagement.Data.Contracts
{
    /// <summary>
    /// Used to retrieve entities by specified query
    /// </summary>
    /// <typeparam name="TResult">Type of result value</typeparam>
    /// <typeparam name="TCriterion">Type of query parameters</typeparam>
    public interface IQuery<out TResult, in TCriterion> where TCriterion : IQueryCriterion
    {
        /// <summary>
        /// Executes given query using criterion
        /// </summary>
        /// <param name="criterion">Query parameters</param>
        /// <returns>Query result</returns>
        TResult Execute(TCriterion criterion);
    }
}