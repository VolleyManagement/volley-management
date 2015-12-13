namespace VolleyManagement.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using VolleyManagement.Contracts;
    using VolleyManagement.Data.Contracts;
    using VolleyManagement.Data.Queries.Common;
    using VolleyManagement.Data.Queries.GameResult;
    using VolleyManagement.Domain.GameReportsAggregate;
    using VolleyManagement.Domain.GameResultsAggregate;
    using VolleyManagement.Domain.TeamsAggregate;

    /// <summary>
    /// Represents an implementation of IGameReportService contract.
    /// </summary>
    public class GameReportService : IGameReportService
    {
        #region Queries

        private readonly IQuery<List<GameResult>, TournamentGameResultsCriteria> _getTournamentGameResultsQuery;
        private readonly IQuery<List<Team>, GetAllCriteria> _getAllTeamsQuery;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="GameReportService"/> class.
        /// </summary>
        /// <param name="getTournamentGameResultsQuery">Query for getting tournament's game results.</param>
        /// <param name="getAllTeamsQuery">Query for getting all teams.</param>
        public GameReportService(
            IQuery<List<GameResult>, TournamentGameResultsCriteria> getTournamentGameResultsQuery,
            IQuery<List<Team>, GetAllCriteria> getAllTeamsQuery)
        {
            _getTournamentGameResultsQuery = getTournamentGameResultsQuery;
            _getAllTeamsQuery = getAllTeamsQuery;
        }

        #endregion

        #region Implementation

        /// <summary>
        /// Gets standings of the tournament specified by identifier.
        /// </summary>
        /// <param name="tournamentId">Identifier of the tournament.</param>
        /// <returns>Standings of the tournament with specified identifier.</returns>
        public List<StandingsEntry> GetStandings(int tournamentId)
        {
            var gameResults = _getTournamentGameResultsQuery.Execute(new TournamentGameResultsCriteria { TournamentId = tournamentId });
            var standings = CreateStandingsEntriesForAllTeams();

            foreach (var gameResult in gameResults)
            {
                StandingsEntry standingsHomeTeamEntry = standings.Single(se => se.TeamId == gameResult.HomeTeamId);
                StandingsEntry standingsAwayTeamEntry = standings.Single(se => se.TeamId == gameResult.AwayTeamId);

                CalculateGamesStatistics(standingsHomeTeamEntry, standingsAwayTeamEntry, gameResult.SetsScore);
                CalculateSetsStatistics(standingsHomeTeamEntry, standingsAwayTeamEntry, gameResult.SetsScore, gameResult.SetScores);
            }

            // order all standings entries by points, then by sets ratio and then by balls ratio in descending order
            standings = standings.OrderByDescending(ts => ts.Points)
                .ThenByDescending(ts => ts.SetsRatio)
                .ThenByDescending(ts => ts.BallsRatio)
                .ToList();

            return standings;
        }

        #endregion

        #region Private methods

        private List<StandingsEntry> CreateStandingsEntriesForAllTeams()
        {
            var teams = _getAllTeamsQuery.Execute(new GetAllCriteria());
            var entries = new List<StandingsEntry>();

            foreach (var team in teams)
            {
                entries.Add(new StandingsEntry
                {
                    TeamId = team.Id,
                    TeamName = team.Name
                });
            }

            return entries;
        }

        private void CalculateGamesStatistics(StandingsEntry homeTeamEntry, StandingsEntry awayTeamEntry, Score setsScore)
        {
            homeTeamEntry.GamesTotal++;
            awayTeamEntry.GamesTotal++;

            switch (setsScore.Home - setsScore.Away)
            {
                case 3: // sets score - 3:0
                    homeTeamEntry.Points += 3;
                    homeTeamEntry.GamesWon++;
                    homeTeamEntry.GamesWithScoreThreeNil++;
                    awayTeamEntry.GamesLost++;
                    awayTeamEntry.GamesWithScoreNilThree++;
                    break;
                case 2: // sets score - 3:1
                    homeTeamEntry.Points += 3;
                    homeTeamEntry.GamesWon++;
                    homeTeamEntry.GamesWithScoreThreeOne++;
                    awayTeamEntry.GamesLost++;
                    awayTeamEntry.GamesWithScoreOneThree++;
                    break;
                case 1: // sets score - 3:2
                    homeTeamEntry.Points += 2;
                    homeTeamEntry.GamesWon++;
                    homeTeamEntry.GamesWithScoreThreeTwo++;
                    awayTeamEntry.Points++;
                    awayTeamEntry.GamesLost++;
                    awayTeamEntry.GamesWithScoreTwoThree++;
                    break;
                case -1: // sets score - 2:3
                    homeTeamEntry.Points++;
                    homeTeamEntry.GamesLost++;
                    homeTeamEntry.GamesWithScoreTwoThree++;
                    awayTeamEntry.Points += 2;
                    awayTeamEntry.GamesWon++;
                    awayTeamEntry.GamesWithScoreThreeTwo++;
                    break;
                case -2: // sets score - 1:3
                    homeTeamEntry.GamesLost++;
                    homeTeamEntry.GamesWithScoreOneThree++;
                    awayTeamEntry.Points += 3;
                    awayTeamEntry.GamesWon++;
                    awayTeamEntry.GamesWithScoreThreeOne++;
                    break;
                case -3: // sets score - 0:3
                    homeTeamEntry.GamesLost++;
                    homeTeamEntry.GamesWithScoreNilThree++;
                    awayTeamEntry.Points += 3;
                    awayTeamEntry.GamesWon++;
                    awayTeamEntry.GamesWithScoreThreeNil++;
                    break;
            }
        }

        private void CalculateSetsStatistics(
            StandingsEntry homeTeamEntry,
            StandingsEntry awayTeamEntry,
            Score setsScore,
            List<Score> setScores)
        {
            homeTeamEntry.SetsWon += setsScore.Home;
            homeTeamEntry.SetsLost += setsScore.Away;
            homeTeamEntry.SetsRatio = homeTeamEntry.SetsLost != 0 ? (float)homeTeamEntry.SetsWon / homeTeamEntry.SetsLost : 0.0f;
            awayTeamEntry.SetsWon += setsScore.Away;
            awayTeamEntry.SetsLost += setsScore.Home;
            awayTeamEntry.SetsRatio = awayTeamEntry.SetsLost != 0 ? (float)awayTeamEntry.SetsWon / awayTeamEntry.SetsLost : 0.0f;

            var homeBallsTotal = setScores.Aggregate(0, (sum, e) => sum + e.Home);
            var awayBallsTotal = setScores.Aggregate(0, (sum, e) => sum + e.Away);

            homeTeamEntry.BallsWon += homeBallsTotal;
            homeTeamEntry.BallsLost += awayBallsTotal;
            homeTeamEntry.BallsRatio = homeTeamEntry.BallsLost != 0 ? (float)homeTeamEntry.BallsWon / homeTeamEntry.BallsLost : 0.0f;
            awayTeamEntry.BallsWon += awayBallsTotal;
            awayTeamEntry.BallsLost += homeBallsTotal;
            awayTeamEntry.BallsRatio = awayTeamEntry.BallsLost != 0 ? (float)awayTeamEntry.BallsWon / awayTeamEntry.BallsLost : 0.0f;
        }

        #endregion
    }
}
