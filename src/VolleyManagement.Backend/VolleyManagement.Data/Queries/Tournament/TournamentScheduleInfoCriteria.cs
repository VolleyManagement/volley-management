namespace VolleyManagement.Data.Queries.Tournament
{
    using Contracts;

    /// <summary>
    /// Represents criteria for finding tournament data transfer object
    /// </summary>
    public class TournamentScheduleInfoCriteria : IQueryCriteria
    {
        /// <summary>
        /// Gets or sets tournament id criteria
        /// </summary>
        public int TournamentId { get; set; }
    }
}
