namespace VolleyManagement.Data.Queries.Tournament
{
    using VolleyManagement.Data.Contracts;

    /// <summary>
    /// Represents criteria for finding tournament data transfer object 
    /// </summary>
    public class TournamentScheduleDtoCriteria : IQueryCriteria 
    {
        /// <summary>
        /// Gets or sets tournament id criteria 
        /// </summary>
        public int TournamentId { get; set; }
    }
}
