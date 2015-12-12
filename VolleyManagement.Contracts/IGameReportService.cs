namespace VolleyManagement.Contracts
{
    using System.Collections.Generic;
    using VolleyManagement.Domain.GameReportsAggregate;

    /// <summary>
    /// Defines a contract for GameReportService.
    /// </summary>
    public interface IGameReportService
    {
        /// <summary>
        /// Gets standings of the tournament specified by identifier.
        /// </summary>
        /// <param name="tournamentId">Identifier of the tournament.</param>
        /// <returns>Standings of the tournament with specified identifier.</returns>
        List<StandingsEntry> GetStandings(int tournamentId);
    }
}
