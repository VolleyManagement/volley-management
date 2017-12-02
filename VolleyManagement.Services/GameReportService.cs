namespace VolleyManagement.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;
    using Data.Contracts;
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
        private readonly IQuery<List<TeamTournamentDto>, FindByTournamentIdCriteria> _tournamentTeamsQuery;
        private readonly IQuery<TournamentScheduleDto, TournamentScheduleInfoCriteria> _tournamentScheduleDtoByIdQuery;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="GameReportService"/> class.
        /// </summary>
        /// <param name="tournamentGameResultsQuery">Query for getting tournament's game results.</param>
        /// <param name="tournamentTeamsQuery">Query for getting tournament's game teams.</param>
        /// <param name="tournamentScheduleDtoByIdQuery">Get tournament data transfer object query.</param>
        public GameReportService(
            IQuery<List<GameResultDto>, TournamentGameResultsCriteria> tournamentGameResultsQuery,
            IQuery<List<TeamTournamentDto>, FindByTournamentIdCriteria> tournamentTeamsQuery,
            IQuery<TournamentScheduleDto, TournamentScheduleInfoCriteria> tournamentScheduleDtoByIdQuery)
        {
            _tournamentGameResultsQuery = tournamentGameResultsQuery;
            _tournamentTeamsQuery = tournamentTeamsQuery;
            _tournamentScheduleDtoByIdQuery = tournamentScheduleDtoByIdQuery;
        }

        #endregion

        #region Implementation

        /// <summary>
        /// Gets standings of the tournament specified by identifier.
        /// </summary>
        /// <param name="tournamentId">Identifier of the tournament.</param>
        /// <returns>Standings of the tournament with specified identifier.</returns>
        public TournamentStandings<StandingsDto> GetStandings(int tournamentId)
        {
            var result = new TournamentStandings<StandingsDto>();

            var gameResults = _tournamentGameResultsQuery.Execute(new TournamentGameResultsCriteria { TournamentId = tournamentId });
            var teamsInTournamentByDivisions = GetTeamsInTournamentByDivisions(tournamentId);

            foreach (var teams in teamsInTournamentByDivisions)
            {
                var standings = CreateEntriesForTeams(teams.Value);

                var gameResultsForDivision = GetGamesResultsForDivision(gameResults, teams.Value);

                foreach (var gameResult in gameResultsForDivision)
                {
                    if (gameResult.HasResult && gameResult.AwayTeamId != null)
                    {
                        StandingsEntry standingsHomeTeamEntry = standings.Single(se => se.TeamId == gameResult.HomeTeamId);
                        StandingsEntry standingsAwayTeamEntry = standings.Single(se => se.TeamId == gameResult.AwayTeamId);

                        CalculateGamesStatistics(standingsHomeTeamEntry, standingsAwayTeamEntry, gameResult);

                        CalculateSetsStatistics(gameResultsForDivision, standings);
                        CalculateBallsStatistics(gameResultsForDivision, standings);
                    }
                }

                var standingsDto = new StandingsDto();
                standingsDto.DivisionId = teams.Key;
                standingsDto.DivisionName = $"Division {teams.Key}";
                standingsDto.Standings = standings.OrderByDescending(s => s.Points)
                    .ThenByDescending(s => s.GamesWon)
                    .ThenByDescending(s => s.SetsRatio)
                    .ThenByDescending(s => s.BallsRatio)
                    .ToList();
                result.Divisions.Add(standingsDto);
            }

            return result;
        }

        /// <summary>
        /// Gets pivot standings of the tournament specified by identifier.
        /// </summary>
        /// <param name="tournamentId">Identifier of the tournament.</param>
        /// <returns>Pivot standings of the tournament with specified identifier.</returns>
        public TournamentStandings<PivotStandingsDto> GetPivotStandings(int tournamentId)
        {
            var gameResults = _tournamentGameResultsQuery.Execute(new TournamentGameResultsCriteria { TournamentId = tournamentId });
            var teamsInTournamentByDivisions = GetTeamsInTournamentByDivisions(tournamentId);

            var pivotStandings = new List<PivotStandingsDto>();

            foreach (var groupedTeams in teamsInTournamentByDivisions)
            {
                var gameResultsForDivision = GetGamesResultsForDivision(gameResults, groupedTeams.Value);

                var teamStandingsInDivision = CreateTeamStandings(groupedTeams.Value, gameResultsForDivision);

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

            return null;//pivotStandings;
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

        private static List<StandingsEntry> CreateEntriesForTeams(IEnumerable<TeamTournamentDto> tournamentTeams)
        {
            return tournamentTeams.Select(team => new StandingsEntry
            {
                TeamId = team.TeamId,
                TeamName = team.TeamName
            })
                .ToList();
        }

        private List<TeamStandingsDto> CreateTeamStandings(List<TeamTournamentDto> tournamentTeams, List<GameResultDto> gameResults)
        {
            var teamsStandings = tournamentTeams.Select(
                t => new TeamStandingsDto
                {
                    TeamId = t.TeamId,
                    TeamName = t.TeamName,
                    Points = 0,
                    SetsRatio = CalculateSetsRatio(GetTeamWonSets(t.TeamId, gameResults), GetTeamLostSets(t.TeamId, gameResults)),
                    BallsRatio = CalculateBallsRatio(GetTeamWonBalls(t.TeamId, gameResults), GetTeamLostBalls(t.TeamId, gameResults))
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
            }
        }

        private void CalculateBallsStatistics(List<GameResultDto> gameResults, List<StandingsEntry> standings)
        {
            foreach (var item in standings)
            {
                item.BallsWon = GetTeamWonBalls(item.TeamId, gameResults);
                item.BallsLost = GetTeamLostBalls(item.TeamId, gameResults);
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

        private Dictionary<int, List<TeamTournamentDto>> GetTeamsInTournamentByDivisions(int tournamentId)
        {
            var teamsByDivisions = _tournamentTeamsQuery.Execute(new FindByTournamentIdCriteria { TournamentId = tournamentId });

            return teamsByDivisions.GroupBy(t => t.DivisionId).ToDictionary(t => t.Key, t => t.ToList());
        }

        private List<GameResultDto> GetGamesResultsForDivision(List<GameResultDto> gameResults, List<TeamTournamentDto> teams)
        {
            var teamsIds = teams.Select(t => t.TeamId).ToList();
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
