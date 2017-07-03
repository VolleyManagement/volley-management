namespace VolleyManagement.Data.Queries.Player
{
    using Contracts;

    /// <summary>
    /// Provides parameters to retrieve Team roster
    /// </summary>
    public class UserToPlayerCriteria : IQueryCriteria
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