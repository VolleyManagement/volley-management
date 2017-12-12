namespace VolleyManagement.Data.Queries.Common
{
    using Contracts;

    /// <summary>
    /// Get By Id type queries parameters
    /// </summary>
    public class FindByIdCriteria : IQueryCriteria
    {
        public FindByIdCriteria()
        {
        }

        public FindByIdCriteria(int id)
        {
            Id = id;
        }

        /// <summary>
        /// Gets or sets Id of the entity to retrieve.
        /// </summary>
        public int Id { get; set; }
    }
}