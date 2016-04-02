namespace VolleyManagement.Data.Queries.GameResult
{
    using VolleyManagement.Data.Contracts; 

    /// <summary>
    /// Represents criteria for retrieving all games of the round 
    /// </summary>
    public class AllTournamentGamesInRound : IQueryCriteria 
    {
        /// <summary>
        /// Get or sets game round criteria for finding games by round number 
        /// </summary>
        public int RoundNumber { get; set; }
    }
}
