namespace VolleyManagement.Data.Queries.Player
{
    using VolleyManagement.Data.Contracts;

    /// <summary>
    /// Provides parameters to retrieve Team roster
    /// </summary>
    public class UserPlayersCriteria : IQueryCriteria
    {
        /// <summary>
        /// Gets or sets player id to look for
        /// </summary>
        public int PlayerId { get; set; }

        /// <summary>
        /// Gets or sets user id to look for
        /// </summary>
        public int UserId { get; set; }
    }
}