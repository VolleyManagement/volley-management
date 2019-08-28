namespace VolleyManagement.Data.Contracts
{
    /// <summary>
    /// Used to retrieve entities by specified query
    /// </summary>
    /// <typeparam name="TResult">Type of result value</typeparam>
    /// <typeparam name="TCriterion">Type of query parameters</typeparam>
    public interface IQuery<out TResult, in TCriterion>
        where TCriterion : IQueryCriteria
    {
        /// <summary>
        /// Executes given query using criteria
        /// </summary>
        /// <param name="criteria">Query parameters</param>
        /// <returns>Query result</returns>
        TResult Execute(TCriterion criteria);
    }
}