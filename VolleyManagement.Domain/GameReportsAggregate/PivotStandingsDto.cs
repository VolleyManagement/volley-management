namespace VolleyManagement.Domain.GameReportsAggregate
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Represents pivot tournament's standings.
    /// </summary>
    public class PivotStandingsDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PivotStandingsDto"/> class.
        /// </summary>
        /// <param name="teams">List of tournament teams standings</param>
        /// <param name="gameResults">List of tournament game results</param>
        public PivotStandingsDto(IList<TeamStandingsDto> teams, IList<ShortGameResultDto> gameResults)
        {
            Teams = new ReadOnlyCollection<TeamStandingsDto>(teams);
            GameResults = new ReadOnlyCollection<ShortGameResultDto>(gameResults);
        }

        /// <summary>
        /// Id of the division this standings belongs to
        /// </summary>
        public int DivisionId { get; set; }

        /// <summary>
        /// Name of the division this standings belongs to
        /// </summary>
        public string DivisionName { get; set; }

        /// <summary>
        /// Gets readonly collection of tournament Teams
        /// </summary>
        public IReadOnlyList<TeamStandingsDto> Teams { get; }

        /// <summary>
        /// Gets readonly collection of tournament game results
        /// </summary>
        public IReadOnlyList<ShortGameResultDto> GameResults { get; }
    }
}
