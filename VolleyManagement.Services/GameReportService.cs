namespace VolleyManagement.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using VolleyManagement.Contracts;
    using VolleyManagement.Data.Contracts;
    using VolleyManagement.Data.Queries.GameResult;
    using VolleyManagement.Domain.GameReportsAggregate;
    using VolleyManagement.Domain.GameResultsAggregate;

    /// <summary>
    /// Represents an implementation of IGameReportService contract.
    /// </summary>
    public class GameReportService : IGameReportService
    {
        #region Queries

        private readonly IQuery<List<GameResult>, TournamentGameResultsCriteria> _tournamentGameResultsQuery;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="GameReportService"/> class.
        /// </summary>
        /// <param name="tournamentGameResultsQuery">Query for getting tournament's game results.</param>
        public GameReportService(IQuery<List<GameResult>, TournamentGameResultsCriteria> tournamentGameResultsQuery)
        {
            _tournamentGameResultsQuery = tournamentGameResultsQuery;
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
            var gameResults = _tournamentGameResultsQuery.Execute(new TournamentGameResultsCriteria { TournamentId = tournamentId });
            var standings = CreateEntriesForTeams(gameResults);

            foreach (var gameResult in gameResults)
            {
                StandingsEntry standingsHomeTeamEntry = standings.Single(se => se.TeamId == gameResult.HomeTeamId);
                StandingsEntry standingsAwayTeamEntry = standings.Single(se => se.TeamId == gameResult.AwayTeamId);

                CalculateGamesStatistics(standingsHomeTeamEntry, standingsAwayTeamEntry, gameResult.SetsScore);
                CalculateSetsStatistics(standingsHomeTeamEntry, standingsAwayTeamEntry, gameResult.SetsScore, gameResult.SetScores);
            }

            // order all standings entries by points, then by sets ratio and then by balls ratio in descending order
            return standings.OrderByDescending(ts => ts.Points)
                .ThenByDescending(ts => ts.SetsRatio)
                .ThenByDescending(ts => ts.BallsRatio)
                .ToList();
        }

        #endregion

        #region Private methods

        private List<StandingsEntry> CreateEntriesForTeams(IEnumerable<GameResult> gameResults)
        {
            var entries = new List<StandingsEntry>();
            var teams = gameResults.Select(gr => new { Id = gr.HomeTeamId, Name = gr.HomeTeamName })
                .Union(gameResults.Select(gr => new { Id = gr.AwayTeamId, Name = gr.AwayTeamName }));

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
            homeTeamEntry.SetsRatio = (float)homeTeamEntry.SetsWon / homeTeamEntry.SetsLost;
            awayTeamEntry.SetsWon += setsScore.Away;
            awayTeamEntry.SetsLost += setsScore.Home;
            awayTeamEntry.SetsRatio = (float)awayTeamEntry.SetsWon / awayTeamEntry.SetsLost;

            var homeBallsTotal = setScores.Aggregate(0, (sum, e) => sum + e.Home);
            var awayBallsTotal = setScores.Aggregate(0, (sum, e) => sum + e.Away);

            homeTeamEntry.BallsWon += homeBallsTotal;
            homeTeamEntry.BallsLost += awayBallsTotal;
            homeTeamEntry.BallsRatio = (float)homeTeamEntry.BallsWon / homeTeamEntry.BallsLost;
            awayTeamEntry.BallsWon += awayBallsTotal;
            awayTeamEntry.BallsLost += homeBallsTotal;
            awayTeamEntry.BallsRatio = (float)awayTeamEntry.BallsWon / awayTeamEntry.BallsLost;
        }

        #endregion
    }
}
