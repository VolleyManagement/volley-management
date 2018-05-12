namespace VolleyManagement.Data.Queries.Common
{
    using Contracts;

    /// <summary>
    /// Query criterion to make request without criteria
    /// </summary>
    public class CriteriaIsNull : IQueryCriteria
    {
        /// <summary>
        /// Gets or sets Player ID to search for
        /// </summary>
        public int? IsNull
        {
            get
            {
                return null;
            }
        }
    }
}
