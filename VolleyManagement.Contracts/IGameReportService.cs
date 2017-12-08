namespace VolleyManagement.Contracts
{
    using System.Collections.Generic;
    using Domain.GameReportsAggregate;

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
        TournamentStandings<StandingsDto> GetStandings(int tournamentId);

        /// <summary>
        /// Gets pivot standings of the tournament specified by identifier.
        /// </summary>
        /// <param name="tournamentId">Identifier of the tournament.</param>
        /// <returns>Pivot standings of the tournament with specified identifier.</returns>
        TournamentStandings<PivotStandingsDto> GetPivotStandings(int tournamentId);

        /// <summary>
        /// Check if the standing available in the tournament
        /// </summary>
        /// <param name="tournamentId">Identifier of the tournament.</param>
        /// <returns>True or false</returns>
        bool IsStandingAvailable(int tournamentId);
    }
}
