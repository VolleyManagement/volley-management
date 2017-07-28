namespace VolleyManagement.Data.Queries.Tournament
{
    using Contracts;

    /// <summary>
    /// Represents criteria for finding tournament by given group criteria
    /// </summary>
    public class TournamentByGroupCriteria : IQueryCriteria
    {
        /// <summary>
        /// Gets or sets group id criteria
        /// </summary>
        public int GroupId { get; set; }
    }
}
