namespace VolleyManagement.Data.Queries.TournamentRequest
{
    using VolleyManagement.Data.Contracts;

    /// <summary>
    /// Represents criteria for finding tournament data transfer object
    /// </summary>
    public class FindByTeamTournamentCriteria : IQueryCriteria
    {
        /// <summary>
        /// Gets or sets tournament id criteria
        /// </summary>
        public int TournamentId { get; set; }

        /// <summary>
        /// Gets or sets team id criteria
        /// </summary>
        public int TeamId { get; set; }
    }
}