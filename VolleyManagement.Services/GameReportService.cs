namespace VolleyManagement.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;
    using Data.Contracts;
    using Data.Queries.Division;
    using Data.Queries.GameResult;
    using Data.Queries.Team;
    using Data.Queries.Tournament;
    using Domain.GameReportsAggregate;
    using Domain.GamesAggregate;
    using Domain.TeamsAggregate;
    using Domain.TournamentsAggregate;

    /// <summary>
    /// Represents an implementation of IGameReportService contract.
    /// </summary>
    public class GameReportService : IGameReportService
    {
        #region Queries

        private readonly IQuery<List<GameResultDto>, TournamentGameResultsCriteria> _tournamentGameResultsQuery;
        private readonly IQuery<List<Team>, FindByTournamentIdCriteria> _tournamentTeamsQuery;
        private readonly IQuery<TournamentScheduleDto, TournamentScheduleInfoCriteria> _tournamentScheduleDtoByIdQuery;
        private readonly IQuery<List<List<Team>>, FindTeamsInDivisionsByTournamentIdCriteria> _teamsInDivisionsByTournamentIdQuery;
        private readonly IQuery<List<Division>, TournamentDivisionsCriteria> _divisionsByTournamentIdQuery;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="GameReportService"/> class.
        /// </summary>
        /// <param name="tournamentGameResultsQuery">Query for getting tournament's game results.</param>
        /// <param name="tournamentTeamsQuery">Query for getting tournament's game teams.</param>
        /// <param name="tournamentScheduleDtoByIdQuery">Get tournament data transfer object query.</param>
        /// <param name="teamsInDivisionsByTournamentIdQuery">Get teams by group id</param>
        /// <param name="divisionsByTournamentIdQuery">Get divisions by tournament id</param>
        public GameReportService(
            IQuery<List<GameResultDto>, TournamentGameResultsCriteria> tournamentGameResultsQuery,
            IQuery<List<Team>, FindByTournamentIdCriteria> tournamentTeamsQuery,
            IQuery<TournamentScheduleDto, TournamentScheduleInfoCriteria> tournamentScheduleDtoByIdQuery,
            IQuery<List<List<Team>>, FindTeamsInDivisionsByTournamentIdCriteria> teamsInDivisionsByTournamentIdQuery,
            IQuery<List<Division>, TournamentDivisionsCriteria> divisionsByTournamentIdQuery)
        {
            _tournamentGameResultsQuery = tournamentGameResultsQuery;
            _tournamentTeamsQuery = tournamentTeamsQuery;
            _tournamentScheduleDtoByIdQuery = tournamentScheduleDtoByIdQuery;
            _teamsInDivisionsByTournamentIdQuery = teamsInDivisionsByTournamentIdQuery;
            _divisionsByTournamentIdQuery = divisionsByTournamentIdQuery;
        }

        #endregion

        #region Implementation

        /// <summary>
        /// Gets standings of the tournament specified by identifier.
        /// </summary>
        /// <param name="tournamentId">Identifier of the tournament.</param>
        /// <returns>Standings of the tournament with specified identifier.</returns>
        public List<List<StandingsEntry>> GetStandings(int tournamentId)
        {
            var gameResults = _tournamentGameResultsQuery.Execute(new TournamentGameResultsCriteria { TournamentId = tournamentId });
            var teamsInTournamentByDivisions = GetTeamsInTournamentByDivisions(tournamentId);
            var standingsByDivisions = new List<List<StandingsEntry>>();

            foreach (var teams in teamsInTournamentByDivisions)
            {
                var standings = CreateEntriesForTeams(teams);

                var gameResultsForDivision = GetGamesResultsForDivision(gameResults, teams);

                foreach (var gameResult in gameResultsForDivision)
                {
                    if (gameResult.AwayTeamId != null)
                    {
                        StandingsEntry standingsHomeTeamEntry = standings.Single(se => se.TeamId == gameResult.HomeTeamId);
                        StandingsEntry standingsAwayTeamEntry = standings.Single(se => se.TeamId == gameResult.AwayTeamId);

                        CalculateGamesStatistics(standingsHomeTeamEntry, standingsAwayTeamEntry, gameResult);
                        CalculateSetsStatistics(gameResultsForDivision, standings);
                        CalculateBallsStatistics(gameResultsForDivision, standings);
                    }
                }

                var orderedStandings = standings.OrderByDescending(s => s.Points)
                    .ThenByDescending(s => s.GamesWon)
                    .ThenByDescending(s => s.SetsRatio)
                    .ThenByDescending(s => s.BallsRatio)
                    .ToList();

                standingsByDivisions.Add(orderedStandings);
            }

            return standingsByDivisions;
        }

        /// <summary>
        /// Gets pivot standings of the tournament specified by identifier.
        /// </summary>
        /// <param name="tournamentId">Identifier of the tournament.</param>
        /// <returns>Pivot standings of the tournament with specified identifier.</returns>
        public List<PivotStandingsDto> GetPivotStandings(int tournamentId)
        {
            var gameResults = _tournamentGameResultsQuery.Execute(new TournamentGameResultsCriteria { TournamentId = tournamentId });
            var teamsInTournamentByDivisions = GetTeamsInTournamentByDivisions(tournamentId);

            var pivotStandings = new List<PivotStandingsDto>();

            foreach (var teams in teamsInTournamentByDivisions)
            {
                var gameResultsForDivision = GetGamesResultsForDivision(gameResults, teams);

                var teamStandingsInDivision = CreateTeamStandings(teams, gameResultsForDivision);

                var shortGameResults = gameResultsForDivision.Where(g => g.AwayTeamId != null).
                    Select(g => new ShortGameResultDto
                    {
                        HomeTeamId = g.HomeTeamId.Value,
                        AwayTeamId = g.AwayTeamId.Value,
                        HomeSetsScore = g.Result.GameScore.Home,
                        AwaySetsScore = g.Result.GameScore.Away,
                        IsTechnicalDefeat = g.Result.GameScore.IsTechnicalDefeat
                    }).
                    ToList();

                pivotStandings.Add(new PivotStandingsDto(teamStandingsInDivision, shortGameResults));
            }

            return pivotStandings;
        }

        /// <summary>
        /// Check if the standing available in the tournament
        /// </summary>
        /// <param name="tournamentId">Identifier of the tournament.</param>
        /// <returns>True or false</returns>
        public bool IsStandingAvailable(int tournamentId)
        {
            var tournamentInfo = _tournamentScheduleDtoByIdQuery
                .Execute(new TournamentScheduleInfoCriteria { TournamentId = tournamentId });

            return tournamentInfo.Scheme != TournamentSchemeEnum.PlayOff;
        }

        #endregion

        #region Private methods

        private static List<StandingsEntry> CreateEntriesForTeams(IEnumerable<Team> tournamentTeams)
        {
            return tournamentTeams.Select(team => new StandingsEntry
                {
                    TeamId = team.Id,
                    TeamName = team.Name
                })
                .ToList();
        }

        private List<TeamStandingsDto> CreateTeamStandings(List<Team> tournamentTeams, List<GameResultDto> gameResults)
        {
            var teamsStandings = tournamentTeams.Select(
                t => new TeamStandingsDto
                {
                    TeamId = t.Id,
                    TeamName = t.Name,
                    Points = 0,
                    SetsRatio = CalculateSetsRatio(GetTeamWonSets(t.Id, gameResults), GetTeamLostSets(t.Id, gameResults)),
                    BallsRatio = CalculateBallsRatio(GetTeamWonBalls(t.Id, gameResults), GetTeamLostBalls(t.Id, gameResults))
                })
                .ToList();

            foreach (var game in gameResults)
            {
                if (game.AwayTeamId != null)
                {
                    var homeTeam = new StandingsEntry { TeamId = game.HomeTeamId.Value };
                    var awayTeam = new StandingsEntry { TeamId = game.AwayTeamId.Value };

                    CalculateGamesStatistics(homeTeam, awayTeam, game);

                    teamsStandings.Single(t => t.TeamId == homeTeam.TeamId).Points += homeTeam.Points;
                    teamsStandings.Single(t => t.TeamId == awayTeam.TeamId).Points += awayTeam.Points;
                }
            }

            return teamsStandings
                 .OrderByDescending(t => t.Points)
                 .ThenByDescending(t => t.SetsRatio)
                 .ThenByDescending(ts => ts.BallsRatio)
                 .ThenBy(t => t.TeamName)
                 .ToList();
        }

        private void CalculateGamesStatistics(StandingsEntry homeTeamEntry, StandingsEntry awayTeamEntry, GameResultDto gameResult)
        {
            if (HasTeamPlayedGames(homeTeamEntry))
            {
                SetDataFromNullToZero(homeTeamEntry);
            }

            if (HasTeamPlayedGames(awayTeamEntry))
            {
                SetDataFromNullToZero(awayTeamEntry);
            }

            if (gameResult.HasResult)
            {
                homeTeamEntry.GamesTotal++;
                awayTeamEntry.GamesTotal++;

                switch (gameResult.Result.GameScore.Home - gameResult.Result.GameScore.Away)
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

                var penalty = gameResult.Result.Penalty;

                if (penalty != null)
                {
                    if (penalty.IsHomeTeam)
                    {
                        homeTeamEntry.Points -= penalty.Amount;
                    }
                    else
                    {
                        awayTeamEntry.Points -= penalty.Amount;
                    }
                }
            }
        }

        private void CalculateSetsStatistics(List<GameResultDto> gameResults, List<StandingsEntry> standings)
        {
            foreach (var item in standings)
            {
                item.SetsWon = GetTeamWonSets(item.TeamId, gameResults);
                item.SetsLost = GetTeamLostSets(item.TeamId, gameResults);
                item.SetsRatio = CalculateSetsRatio(item.SetsWon.Value, item.SetsLost.Value);
            }
        }

        private void CalculateBallsStatistics(List<GameResultDto> gameResults, List<StandingsEntry> standings)
        {
            foreach (var item in standings)
            {
                item.BallsWon = GetTeamWonBalls(item.TeamId, gameResults);
                item.BallsLost = GetTeamLostBalls(item.TeamId, gameResults);
                item.BallsRatio = CalculateBallsRatio(item.BallsWon.Value, item.BallsLost.Value);
            }
        }

        private int GetTeamWonSets(int teamId, List<GameResultDto> games)
        {
            int result = 0;
            result += games.Where(g => g.HomeTeamId == teamId).Sum(g => (int)g.Result.GameScore.Home);
            result += games.Where(g => g.AwayTeamId == teamId).Sum(g => (int)g.Result.GameScore.Away);
            return result;
        }

        private int GetTeamLostSets(int teamId, List<GameResultDto> games)
        {
            int result = 0;
            result += games.Where(g => g.HomeTeamId == teamId).Sum(g => (int)g.Result.GameScore.Away);
            result += games.Where(g => g.AwayTeamId == teamId).Sum(g => (int)g.Result.GameScore.Home);
            return result;
        }

        private float? CalculateSetsRatio(int gamesWon, int gamesLost)
        {
            var result = (float)gamesWon / gamesLost;
            if (float.IsNaN(result))
            {
                return null;
            }

            return result;
        }

        private int GetTeamWonBalls(int teamId, List<GameResultDto> games)
        {
            var results = games.Where(g => g.HomeTeamId == teamId).ToList();
            int wonBalls = results.Where(item => !item.Result.GameScore.IsTechnicalDefeat).Sum(CalculateHomeSetBallsForNonTechnicalDefeatSets);

            results = games.Where(g => g.AwayTeamId == teamId).ToList();
            wonBalls += results.Where(item => !item.Result.GameScore.IsTechnicalDefeat).Sum(CalculateAwaySetBallsForNonTechnicalDefeatSets);
            return wonBalls;
        }

        private int GetTeamLostBalls(int teamId, List<GameResultDto> games)
        {
            var results = games.Where(g => g.HomeTeamId == teamId).ToList();
            int lostBalls = results.Where(item => !item.Result.GameScore.IsTechnicalDefeat).Sum(CalculateAwaySetBallsForNonTechnicalDefeatSets);

            results = games.Where(g => g.AwayTeamId == teamId).ToList();
            lostBalls += results.Where(item => !item.Result.GameScore.IsTechnicalDefeat).Sum(CalculateHomeSetBallsForNonTechnicalDefeatSets);
            return lostBalls;
        }

        private float? CalculateBallsRatio(int ballsWon, int ballsLost)
        {
            var result = (float)ballsWon / ballsLost;
            if (float.IsNaN(result))
            {
                return null;
            }

            return result;
        }

        private void SetDataFromNullToZero(StandingsEntry entry)
        {
            entry.GamesWon = 0;
            entry.GamesLost = 0;
            entry.GamesWithScoreThreeNil = 0;
            entry.GamesWithScoreThreeOne = 0;
            entry.GamesWithScoreThreeTwo = 0;
            entry.GamesWithScoreTwoThree = 0;
            entry.GamesWithScoreOneThree = 0;
            entry.GamesWithScoreNilThree = 0;
            entry.BallsWon = 0;
            entry.BallsLost = 0;
            entry.SetsWon = 0;
            entry.SetsLost = 0;
        }

        private bool HasTeamPlayedGames(StandingsEntry entry)
        {
            return entry.GamesTotal == 0;
        }

        private List<List<Team>> GetTeamsInTournamentByDivisions(int tournamentId)
        {
            var teamsByDivisions = _teamsInDivisionsByTournamentIdQuery.Execute(new FindTeamsInDivisionsByTournamentIdCriteria { TournamentId = tournamentId });

            return teamsByDivisions;
        }

        private List<GameResultDto> GetGamesResultsForDivision(List<GameResultDto> gameResults, List<Team> teams)
        {
            var teamsIds = teams.Select(t => t.Id).ToList();
            return gameResults.Where(gr => teamsIds.Contains(gr.AwayTeamId.GetValueOrDefault()) &&
                                            teamsIds.Contains(gr.HomeTeamId.GetValueOrDefault())).
                               ToList();
        }

        private int CalculateHomeSetBallsForNonTechnicalDefeatSets(GameResultDto item)
        {
            return item.Result.SetScores.Sum(r => r.HomeBallsForStatistics);
        }

        private int CalculateAwaySetBallsForNonTechnicalDefeatSets(GameResultDto item)
        {
            return item.Result.SetScores.Sum(r => r.AwayBallsForStatistics);
        }
        #endregion
    }
}
