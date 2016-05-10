namespace VolleyManagement.Domain.GameReportsAggregate
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using GamesAggregate;

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
        public PivotStandingsDto(List<TeamStandingsDto> teams, List<ShortGameResultDto> gameResults)
        {
            Teams = teams;
            GameResults = gameResults;
        }

        /// <summary>
        /// Gets readonly collection of tournament Teams
        /// </summary>
        public IReadOnlyCollection<TeamStandingsDto> Teams { get; private set; }

        /// <summary>
        /// Gets readonly collection of tournament game results
        /// </summary>
        public IReadOnlyCollection<ShortGameResultDto> GameResults { get; private set; }
    }
}
