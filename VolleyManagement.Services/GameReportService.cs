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
        private readonly IQuery<Team, FindByIdCriteria> _getTeamByIdQuery;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="GameReportService"/> class.
        /// </summary>
        /// <param name="getTournamentGameResultsQuery">Query for getting tournament's game results.</param>
        /// <param name="getTeamByIdQuery">Query for getting team by its identifier.</param>
        public GameReportService(
            IQuery<List<GameResult>, TournamentGameResultsCriteria> getTournamentGameResultsQuery,
            IQuery<Team, FindByIdCriteria> getTeamByIdQuery)
        {
            _getTournamentGameResultsQuery = getTournamentGameResultsQuery;
            _getTeamByIdQuery = getTeamByIdQuery;
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
            var standings = new List<StandingsEntry>();

            foreach (var gameResult in gameResults)
            {
                bool addHomeTeamEntry = false;
                bool addAwayTeamEntry = false;
                StandingsEntry standingsHomeTeamEntry = standings.SingleOrDefault(se => se.TeamId == gameResult.HomeTeamId);
                StandingsEntry standingsAwayTeamEntry = standings.SingleOrDefault(se => se.TeamId == gameResult.AwayTeamId);

                if (standingsHomeTeamEntry == null)
                {
                    // standings entry for the home team does not exist, so we need to create one
                    standingsHomeTeamEntry = CreateStandingsEntryForTeam(gameResult.HomeTeamId);
                    addHomeTeamEntry = true;
                }

                if (standingsAwayTeamEntry == null)
                {
                    // standings entry for the away team does not exist, so we need to create one
                    standingsAwayTeamEntry = CreateStandingsEntryForTeam(gameResult.AwayTeamId);
                    addAwayTeamEntry = true;
                }

                CalculateGamesStatistics(standingsHomeTeamEntry, standingsAwayTeamEntry, gameResult.SetsScore);
                CalculateSetsStatistics(standingsHomeTeamEntry, standingsAwayTeamEntry, gameResult.SetsScore, gameResult.SetScores);

                if (addHomeTeamEntry)
                {
                    standings.Add(standingsHomeTeamEntry);
                }

                if (addAwayTeamEntry)
                {
                    standings.Add(standingsAwayTeamEntry);
                }
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

        private StandingsEntry CreateStandingsEntryForTeam(int teamId)
        {
            var team = _getTeamByIdQuery.Execute(new FindByIdCriteria { Id = teamId });

            return new StandingsEntry
            {
                TeamId = team.Id,
                TeamName = team.Name
            };
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
