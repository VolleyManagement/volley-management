﻿namespace VolleyManagement.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;
    using Data.Contracts;
    using Data.Queries.Common;
    using Data.Queries.GameResult;
    using Data.Queries.Team;
    using Data.Queries.Tournament;
    using Domain.GameReportsAggregate;
    using Domain.GamesAggregate;
    using Domain.TeamsAggregate;
    using Domain.TournamentsAggregate;

#pragma warning disable S1200 // Classes should not be coupled to too many other classes (Single Responsibility Principle)
    /// <summary>
    /// Represents an implementation of IGameReportService contract.
    /// </summary>
    public class GameReportService : IGameReportService
#pragma warning restore S1200 // Classes should not be coupled to too many other classes (Single Responsibility Principle)
    {
        #region Queries

        private readonly IQuery<ICollection<GameResultDto>, TournamentGameResultsCriteria> _tournamentGameResultsQuery;
        private readonly IQuery<ICollection<TeamTournamentDto>, FindByTournamentIdCriteria> _tournamentTeamsQuery;
        private readonly IQuery<TournamentScheduleDto, TournamentScheduleInfoCriteria> _tournamentScheduleDtoByIdQuery;
        private readonly IQuery<Tournament, FindByIdCriteria> _tournamentByIdQuery;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="GameReportService"/> class.
        /// </summary>
        /// <param name="tournamentGameResultsQuery">Query for getting tournament's game results.</param>
        /// <param name="tournamentTeamsQuery">Query for getting tournament's game teams.</param>
        /// <param name="tournamentScheduleDtoByIdQuery">Get tournament data transfer object query.</param>
        public GameReportService(
            IQuery<ICollection<GameResultDto>, TournamentGameResultsCriteria> tournamentGameResultsQuery,
            IQuery<ICollection<TeamTournamentDto>, FindByTournamentIdCriteria> tournamentTeamsQuery,
            IQuery<TournamentScheduleDto, TournamentScheduleInfoCriteria> tournamentScheduleDtoByIdQuery,
            IQuery<Tournament, FindByIdCriteria> tournamentByIdQuery)
        {
            _tournamentGameResultsQuery = tournamentGameResultsQuery;
            _tournamentTeamsQuery = tournamentTeamsQuery;
            _tournamentScheduleDtoByIdQuery = tournamentScheduleDtoByIdQuery;
            _tournamentByIdQuery = tournamentByIdQuery;
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

            var tournament = _tournamentByIdQuery.Execute(new FindByIdCriteria(tournamentId));
            var gameResults = _tournamentGameResultsQuery.Execute(new TournamentGameResultsCriteria { TournamentId = tournamentId });
            Dictionary<(int divisionId, string divisionName), List<TeamTournamentDto>> teamsInTournamentByDivisions = GetTeamsInTournamentByDivisions(tournamentId);

            foreach (var groupedTeams in teamsInTournamentByDivisions)
            {
                var standings = CalculateStandingsForDivision(groupedTeams.Value, gameResults);

                var standingsDto = new StandingsDto
                {
                    DivisionId = groupedTeams.Key.divisionId,
                    DivisionName = groupedTeams.Key.divisionName,
                    LastUpdateTime = tournament.LastTimeUpdated,
                    Standings = standings
                };
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
            var tournament = _tournamentByIdQuery.Execute(new FindByIdCriteria(tournamentId));
            var gameResults = _tournamentGameResultsQuery.Execute(new TournamentGameResultsCriteria { TournamentId = tournamentId });
            Dictionary<(int divisionId, string divisionName), List<TeamTournamentDto>> teamsInTournamentByDivisions = GetTeamsInTournamentByDivisions(tournamentId);

            var pivotStandings = new TournamentStandings<PivotStandingsDto>();

            foreach (var groupedTeams in teamsInTournamentByDivisions)
            {
                var gameResultsForDivision = GetGamesResultsForDivision(gameResults, groupedTeams.Value);

                var teamStandingsInDivision = CalculateStandingsForDivision(groupedTeams.Value, gameResultsForDivision)
                    .Select(MapToTeamStandingsDto())
                    .ToList();

                var shortGameResults = gameResultsForDivision.Where(g => g.AwayTeamId != null)
                    .Select(MapToShortGameResult())
                    .ToList();

                pivotStandings.Divisions.Add(new PivotStandingsDto(teamStandingsInDivision, shortGameResults)
                {
                    DivisionId = groupedTeams.Key.divisionId,
                    DivisionName = groupedTeams.Key.divisionName,
                    LastUpdateTime = tournament.LastTimeUpdated
                });
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

        private static List<StandingsEntry> CalculateStandingsForDivision(List<TeamTournamentDto> teams, IEnumerable<GameResultDto> gameResults)
        {
            var standings = CreateEntriesForTeams(teams);

            var gameResultsForDivision = GetGamesResultsForDivision(gameResults, teams);

            foreach (var gameResult in gameResultsForDivision.Where(gr => gr.HasResult))
            {
                var standingsHomeTeamEntry = standings.Single(se => se.TeamId == gameResult.HomeTeamId);
                var standingsAwayTeamEntry = standings.Single(se => se.TeamId == gameResult.AwayTeamId);

                CalculateGamesStatistics(standingsHomeTeamEntry, standingsAwayTeamEntry, gameResult);

                CalculateSetsStatistics(gameResultsForDivision, standings);
                CalculateBallsStatistics(gameResultsForDivision, standings);
            }
            return standings.OrderByDescending(s => s.Points)
                .ThenByDescending(s => s.GamesWon)
                .ThenByDescending(s => s.SetsRatio)
                .ThenByDescending(s => s.BallsRatio)
                .ToList();
        }

        private static List<StandingsEntry> CreateEntriesForTeams(IEnumerable<TeamTournamentDto> tournamentTeams)
        {
            return tournamentTeams.Select(team => new StandingsEntry
            {
                TeamId = team.TeamId,
                TeamName = team.TeamName
            })
                .ToList();
        }

        private static void CalculateGamesStatistics(StandingsEntry homeTeamEntry, StandingsEntry awayTeamEntry, GameResultDto gameResult)
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
                default:
                    throw new InvalidOperationException();

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

        private static void CalculateSetsStatistics(List<GameResultDto> gameResults, List<StandingsEntry> standings)
        {
            foreach (var item in standings)
            {
                item.SetsWon = GetTeamWonSets(item.TeamId, gameResults);
                item.SetsLost = GetTeamLostSets(item.TeamId, gameResults);
            }
        }

        private static void CalculateBallsStatistics(List<GameResultDto> gameResults, List<StandingsEntry> standings)
        {
            foreach (var item in standings)
            {
                item.BallsWon = GetTeamWonBalls(item.TeamId, gameResults);
                item.BallsLost = GetTeamLostBalls(item.TeamId, gameResults);
            }
        }

        private static int GetTeamWonSets(int teamId, List<GameResultDto> games)
        {
            var result = 0;
            result += games.Where(g => g.HomeTeamId == teamId).Sum(g => g.Result.GameScore.Home);
            result += games.Where(g => g.AwayTeamId == teamId).Sum(g => g.Result.GameScore.Away);
            return result;
        }

        private static int GetTeamLostSets(int teamId, List<GameResultDto> games)
        {
            var result = 0;
            result += games.Where(g => g.HomeTeamId == teamId).Sum(g => g.Result.GameScore.Away);
            result += games.Where(g => g.AwayTeamId == teamId).Sum(g => g.Result.GameScore.Home);
            return result;
        }

        private static int GetTeamWonBalls(int teamId, List<GameResultDto> games)
        {
            var wonBalls = games.Where(g => g.HomeTeamId == teamId)
                                .Where(item => !item.Result.GameScore.IsTechnicalDefeat)
                                .Sum(CalculateHomeSetBallsForNonTechnicalDefeatSets);

            wonBalls += games.Where(g => g.AwayTeamId == teamId)
                             .Where(item => !item.Result.GameScore.IsTechnicalDefeat)
                             .Sum(CalculateAwaySetBallsForNonTechnicalDefeatSets);
            return wonBalls;
        }

        private static int GetTeamLostBalls(int teamId, List<GameResultDto> games)
        {
            var results = games.Where(g => g.HomeTeamId == teamId).ToList();
            var lostBalls = results.Where(item => !item.Result.GameScore.IsTechnicalDefeat)
                                   .Sum(CalculateAwaySetBallsForNonTechnicalDefeatSets);

            results = games.Where(g => g.AwayTeamId == teamId).ToList();
            lostBalls += results.Where(item => !item.Result.GameScore.IsTechnicalDefeat)
                                .Sum(CalculateHomeSetBallsForNonTechnicalDefeatSets);
            return lostBalls;
        }

        private Dictionary<(int, string), List<TeamTournamentDto>> GetTeamsInTournamentByDivisions(int tournamentId)
        {
            var teamsByDivisions = _tournamentTeamsQuery.Execute(new FindByTournamentIdCriteria { TournamentId = tournamentId });

            return teamsByDivisions.GroupBy(t => (t.DivisionId, t.DivisionName)).ToDictionary(t => t.Key, t => t.ToList());
        }

        private static List<GameResultDto> GetGamesResultsForDivision(IEnumerable<GameResultDto> gameResults, IEnumerable<TeamTournamentDto> teams)
        {
            var teamsIds = teams.Select(t => t.TeamId).ToList();
            return gameResults.Where(gr => teamsIds.Contains(gr.AwayTeamId.GetValueOrDefault()) &&
                                           teamsIds.Contains(gr.HomeTeamId.GetValueOrDefault())).ToList();
        }

        private static int CalculateHomeSetBallsForNonTechnicalDefeatSets(GameResultDto item)
        {
            return item.Result.SetScores.Where(s => !s.IsTechnicalDefeat)
                                                      .Sum(r => r.Home);
        }

        private static int CalculateAwaySetBallsForNonTechnicalDefeatSets(GameResultDto item)
        {
            return item.Result.SetScores.Where(s => !s.IsTechnicalDefeat)
                                                      .Sum(r => r.Away);
        }

        private static Func<GameResultDto, ShortGameResultDto> MapToShortGameResult()
        {
            return g => new ShortGameResultDto
            {
                HomeTeamId = g.HomeTeamId.GetValueOrDefault(),
                AwayTeamId = g.AwayTeamId.GetValueOrDefault(),
                HomeGameScore = g.HasResult ? g.Result.GameScore.Home : byte.MinValue,
                AwayGameScore = g.HasResult ? g.Result.GameScore.Away : byte.MinValue,
                IsTechnicalDefeat = g.HasResult && g.Result.GameScore.IsTechnicalDefeat,
                WasPlayed = g.HasResult,
                RoundNumber = g.Round
            };
        }

        private static Func<StandingsEntry, TeamStandingsDto> MapToTeamStandingsDto()
        {
            return se => new TeamStandingsDto
            {
                TeamId = se.TeamId,
                TeamName = se.TeamName,
                Points = se.Points,
                SetsRatio = se.SetsRatio,
                BallsRatio = se.BallsRatio
            };
        }

        #endregion
    }
}
