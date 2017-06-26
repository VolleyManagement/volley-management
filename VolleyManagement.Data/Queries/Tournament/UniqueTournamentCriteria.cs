namespace VolleyManagement.Data.Queries.Tournament
{
    using Contracts;

    /// <summary>
    /// Search by name criterion
    /// </summary>
    public class UniqueTournamentCriteria : IQueryCriteria
    {
        /// <summary>
        /// Gets or sets Name to search
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Id of the entity for Update operation
        /// </summary>
        public int? EntityId { get; set; }
    }
}