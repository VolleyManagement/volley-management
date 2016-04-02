namespace VolleyManagement.Data.Queries.GameResult
{
    using VolleyManagement.Data.Contracts; 

    /// <summary>
    /// Represents criteria for retrieving list of lists of games sorted by rounds number 
    /// </summary>
    public class AllTournamentRoundsCriteria : IQueryCriteria
    {
        /// <summary>
        /// Gets or sets the tournament's identifier 
        /// </summary>
        public int TournamentID { get; set; }

        /// <summary>
        /// Gets or sets the round number in the tournament 
        /// </summary>
        public int RoundNumber { get; set; }
    }
}
