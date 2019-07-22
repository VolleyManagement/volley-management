namespace VolleyManagement.Data.MsSql.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;
    using Data.Queries.Common;
    using Data.Queries.GameResult;
    using Domain.GamesAggregate;
    using Entities;
    using System.Data.Entity;

#pragma warning disable S1200 // Classes should not be coupled to too many other classes (Single Responsibility Principle)
    /// <summary>
    /// Provides implementation of game result queries.
    /// </summary>
    public class GameResultQueries : IQuery<GameResultDto, FindByIdCriteria>,
#pragma warning restore S1200 // Classes should not be coupled to too many other classes (Single Responsibility Principle)
                                     IQuery<ICollection<GameResultDto>, TournamentGameResultsCriteria>,
                                     IQuery<ICollection<Game>, TournamentRoundsGameResultsCriteria>,
                                     IQuery<ICollection<Game>, GamesByRoundCriteria>,
                                     IQuery<Game, GameByNumberCriteria>
    {
        #region Fields

        private readonly DbSet<GameResultEntity> _dalGameResults;
        private readonly DbSet<TournamentEntity> _dalTournaments;
        private readonly DbSet<DivisionEntity> _dalDivisions;
        private readonly DbSet<GroupEntity> _dalGroups;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="GameResultQueries"/> class.
        /// </summary>
        /// <param name="unitOfWork">Instance of class which implements <see cref="IUnitOfWork"/>.</param>
        public GameResultQueries(IUnitOfWork unitOfWork)
        {
            var vmUoW = (VolleyUnitOfWork)unitOfWork;
            _dalGameResults = vmUoW.Context.GameResults;
            _dalTournaments = vmUoW.Context.Tournaments;
            _dalDivisions = vmUoW.Context.Divisions;
            _dalGroups = vmUoW.Context.Groups;
        }

        #endregion

        #region Implemenations

        /// <summary>
        /// Finds game result by identifier criteria.
        /// </summary>
        /// <param name="criteria">Identifier criteria.</param>
        /// <returns>Domain model of game result.</returns>
        public GameResultDto Execute(FindByIdCriteria criteria)
        {
            var gamesWithId = _dalGameResults
                .Where(gr => gr.Id == criteria.Id)
                .ToList();

            return gamesWithId
                .Select(gr => GetGameResultDtoMap()(gr))
                .SingleOrDefault();
        }

        /// <summary>
        /// Gets game results of the tournament by specified criteria.
        /// </summary>
        /// <param name="criteria">Tournament's game results criteria.</param>
        /// <returns>List of domain models of game result.</returns>
        public ICollection<GameResultDto> Execute(TournamentGameResultsCriteria criteria)
        {
            var tournamentId = criteria.TournamentId;

            var allGamesWithTeams =
                from game in _dalGameResults
                join tournament in _dalTournaments
                    on game.TournamentId equals tournament.Id
                join division in _dalDivisions
                    on tournament.Id equals division.TournamentId
                join groups in _dalGroups
                    on division.Id equals groups.DivisionId
                where game.TournamentId == tournamentId
                where groups.Teams.Contains(game.HomeTeam)
                select new
                {
                    results = game,
                    divisionName = division.Name,
                    divisionId = division.Id,
                    groupId = groups.Id
                };

            var gamesWithoutTeams =
                from game in _dalGameResults
                join tournament in _dalTournaments
                    on game.TournamentId equals tournament.Id
                join division in _dalDivisions
                    on tournament.Id equals division.TournamentId
                join groups in _dalGroups
                    on division.Id equals groups.DivisionId
                where game.TournamentId == tournamentId
                where game.HomeTeam == null
                select new
                {
                    results = game,
                    divisionName = division.Name,
                    divisionId = division.Id,
                    groupId = groups.Id
                };

            var query = allGamesWithTeams.Union(gamesWithoutTeams);

            var list = query.ToList()
                        .ConvertAll(item => Map(item.results, item.divisionName, item.divisionId, item.groupId));

            return list;
        }

        /// <summary>
        /// Gets games results of the tournament and rounds by specified criteria.
        /// </summary>
        /// <param name="criteria">Tournament's and round`s game results criteria.</param>
        /// <returns>List of Game of game result.</returns>
        public ICollection<Game> Execute(TournamentRoundsGameResultsCriteria criteria)
        {
            // Method ToList() used because it gives opportunity to load
            // specified game results into memory and then convert them.
            // In case of using method Select(Mapper) there is an issue with EF query.
            // If method Select() called set scores mapped in wrong order.
            var games = _dalGameResults
                 .Where(gr => gr.TournamentId == criteria.TournamentId
                     && (gr.RoundNumber == criteria.FirstRoundNumber || gr.RoundNumber == criteria.SecondRoundNumber))
                     .ToList();

            return games.ConvertAll(GetGameMapping());
        }

        /// <summary>
        /// Gets games of the tournament from specified rounds
        /// </summary>
        /// <param name="criteria">Tournament and round number criteria</param>
        /// <returns>Collection of games which satisfy the criteria</returns>
        public ICollection<Game> Execute(GamesByRoundCriteria criteria)
        {
            var games = _dalGameResults
                .Where(gr => gr.TournamentId == criteria.TournamentId
                    && criteria.RoundNumbers.Any(n => gr.RoundNumber == n))
                    .ToList();

            return games.ConvertAll(GetGameMapping());
        }

        /// <summary>
        /// Find game result by criteria.
        /// </summary>
        /// <param name="criteria">Identifier criteria.</param>
        /// <returns>Domain model of game result.</returns>
        public Game Execute(GameByNumberCriteria criteria)
        {
            var gameResult = _dalGameResults
                  .Where(gr => gr.TournamentId == criteria.TournamentId
                               && gr.GameNumber == criteria.GameNumber)
                  .ToList();

            return gameResult.Select(gr => GetGameMapping()(gr))
                .SingleOrDefault();
        }

        #endregion

        #region Mapping

        private static Converter<GameResultEntity, Game> GetGameMapping()
        {
            return gr => new Game {
                Id = gr.Id,
                TournamentId = gr.TournamentId,
                HomeTeamId = gr.HomeTeamId,
                AwayTeamId = gr.AwayTeamId,
                GameDate = gr.StartTime,
                Round = gr.RoundNumber,
                GameNumber = gr.GameNumber,
                Result = MapResult(gr),
                UrlToGameVideo = gr.UrlToGameVideo
            };
        }

        private static Converter<GameResultEntity, GameResultDto> GetGameResultDtoMap()
        {
            return gr => new GameResultDto {
                Id = gr.Id,
                TournamentId = gr.TournamentId,
                HomeTeamId = gr.HomeTeamId,
                AwayTeamId = gr.AwayTeamId,
                GameDate = gr.StartTime,
                Round = gr.RoundNumber,
                GameNumber = gr.GameNumber,
                HomeTeamName = gr.HomeTeam?.Name,
                AwayTeamName = gr.AwayTeam?.Name,
                Result = MapResult(gr),
                UrlToGameVideo = gr.UrlToGameVideo
            };
        }

        private static Result MapResult(GameResultEntity gr)
        {
            return new Result {
                SetScores = new List<Score>
                {
                    new Score { Home = gr.HomeSet1Score, Away = gr.AwaySet1Score, IsTechnicalDefeat = gr.IsSet1TechnicalDefeat },
                    new Score { Home = gr.HomeSet2Score, Away = gr.AwaySet2Score, IsTechnicalDefeat = gr.IsSet2TechnicalDefeat },
                    new Score { Home = gr.HomeSet3Score, Away = gr.AwaySet3Score, IsTechnicalDefeat = gr.IsSet3TechnicalDefeat },
                    new Score { Home = gr.HomeSet4Score, Away = gr.AwaySet4Score, IsTechnicalDefeat = gr.IsSet4TechnicalDefeat },
                    new Score { Home = gr.HomeSet5Score, Away = gr.AwaySet5Score, IsTechnicalDefeat = gr.IsSet5TechnicalDefeat }
                },
                GameScore = new Score { Home = gr.HomeSetsScore, Away = gr.AwaySetsScore, IsTechnicalDefeat = gr.IsTechnicalDefeat },
                Penalty = MapPenalty(gr)
            };
        }

        private static Penalty MapPenalty(GameResultEntity gr)
        {
            Penalty result;

            result = gr.PenaltyTeam != 0 ?
                new Penalty {
                    IsHomeTeam = gr.PenaltyTeam == 1,
                    Amount = gr.PenaltyAmount,
                    Description = gr.PenaltyDescription
                }
                : null;

            return result;
        }

        private static GameResultDto Map(GameResultEntity gr, string divisionName, int divisionId, int groupId)
        {
            var result = GetGameResultDtoMap()(gr);
            result.DivisionName = divisionName;
            result.DivisionId = divisionId;
            result.GroupId = groupId;
            return result;
        }
        #endregion
    }
}
