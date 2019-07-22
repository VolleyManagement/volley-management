namespace VolleyManagement.Data.Queries.GameResult
{
    using Contracts;

    /// <summary>
    /// Represents criteria to retrieve tournament's game by its identifier and rounds.
    /// </summary>
    public class TournamentRoundsGameResultsCriteria : IQueryCriteria
    {
        /// <summary>
        /// Gets or sets the tournament's identifier.
        /// </summary>
        public int TournamentId { get; set; }

        /// <summary>
        /// Gets or sets the first round's identifier.
        /// </summary>
        public byte FirstRoundNumber { get; set; }

        /// <summary>
        /// Gets or sets the second round's identifier.
        /// </summary>
        public byte SecondRoundNumber { get; set; }
    }
}
