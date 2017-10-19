namespace VolleyManagement.Data.MsSql.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;
    using Contracts;
    using Data.Queries.Common;
    using Data.Queries.GameResult;
    using Domain.GamesAggregate;
    using Entities;

    /// <summary>
    /// Provides implementation of game result queries.
    /// </summary>
    public class GameResultQueries : IQuery<GameResultDto, FindByIdCriteria>,
                                     IQuery<List<GameResultDto>, TournamentGameResultsCriteria>,
                                     IQuery<List<Game>, TournamentRoundsGameResultsCriteria>,
                                     IQuery<List<Game>, GamesByRoundCriteria>,
                                     IQuery<Game, GameByNumberCriteria>
    {
        #region Fields

        private readonly VolleyUnitOfWork _unitOfWork;
        private readonly DbSet<GameResultEntity> _dalGameResults;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="GameResultQueries"/> class.
        /// </summary>
        /// <param name="unitOfWork">Instance of class which implements <see cref="IUnitOfWork"/>.</param>
        public GameResultQueries(IUnitOfWork unitOfWork)
        {
            _unitOfWork = (VolleyUnitOfWork)unitOfWork;
            _dalGameResults = _unitOfWork.Context.GameResults;
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
            return _dalGameResults
                .Where(gr => gr.Id == criteria.Id)
                .Select(GetGameResultDtoMapping())
                .SingleOrDefault();
        }

        /// <summary>
        /// Gets game results of the tournament by specified criteria.
        /// </summary>
        /// <param name="criteria">Tournament's game results criteria.</param>
        /// <returns>List of domain models of game result.</returns>
        public List<GameResultDto> Execute(TournamentGameResultsCriteria criteria)
        {
            var gameResults = _dalGameResults
                .Where(gr => gr.TournamentId == criteria.TournamentId)
                .Select(GetGameResultDtoMapping());

            List<GameResultDto> list = gameResults.Any() ? gameResults.ToList() : new List<GameResultDto>();
            return list;
        }

        /// <summary>
        /// Gets games results of the tournament and rounds by specified criteria.
        /// </summary>
        /// <param name="criteria">Tournament's and round`s game results criteria.</param>
        /// <returns>List of Game of game result.</returns>
        public List<Game> Execute(TournamentRoundsGameResultsCriteria criteria)
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
        public List<Game> Execute(GamesByRoundCriteria criteria)
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
            return _dalGameResults
                .Where(gr => gr.TournamentId == criteria.TournamentId
                && gr.GameNumber == criteria.GameNumber)
                .Select(GetGameMappingExpression())
                .SingleOrDefault();
        }

        #endregion

        #region Mapping

        private static Expression<Func<GameResultEntity, GameResultDto>> GetGameResultDtoMapping()
        {
            return gr => new GameResultDto
            {
                Id = gr.Id,
                TournamentId = gr.TournamentId,
                HomeTeamId = gr.HomeTeamId,
                AwayTeamId = gr.AwayTeamId,
                HomeTeamName = gr.HomeTeam.Name,
                AwayTeamName = gr.AwayTeam.Name,
                Result = new Result
                {
                    SetsScore = new Score
                    {
                        Home = gr.HomeSetsScore,
                        Away = gr.AwaySetsScore,
                        IsTechnicalDefeat = gr.IsTechnicalDefeat
                    },
                    SetScores = new List<Score>
                    {
                        new Score
                        {
                            Home = gr.HomeSet1Score,
                            Away = gr.AwaySet1Score,
                            IsTechnicalDefeat = gr.IsSet1TechnicalDefeat
                        },
                        new Score
                        {
                            Home = gr.HomeSet2Score,
                            Away = gr.AwaySet2Score,
                            IsTechnicalDefeat = gr.IsSet2TechnicalDefeat
                        },
                        new Score
                        {
                            Home = gr.HomeSet3Score,
                            Away = gr.AwaySet3Score,
                            IsTechnicalDefeat = gr.IsSet3TechnicalDefeat
                        },
                        new Score
                        {
                            Home = gr.HomeSet4Score,
                            Away = gr.AwaySet4Score,
                            IsTechnicalDefeat = gr.IsSet4TechnicalDefeat
                        },
                        new Score
                        {
                            Home = gr.HomeSet5Score,
                            Away = gr.AwaySet5Score,
                            IsTechnicalDefeat = gr.IsSet5TechnicalDefeat
                        },
                    }
                },
                GameDate = gr.StartTime,
                Round = gr.RoundNumber,
                GameNumber = gr.GameNumber
            };
        }

        private static Expression<Func<GameResultEntity, Game>> GetGameMappingExpression()
        {
            return gr => new Game
            {
                Id = gr.Id,
                TournamentId = gr.TournamentId,
                HomeTeamId = gr.HomeTeamId,
                AwayTeamId = gr.AwayTeamId,
                GameDate = gr.StartTime,
                Round = gr.RoundNumber,
                GameNumber = gr.GameNumber,
                Result = new Result
                {
                    SetScores = new List<Score>
                    {
                         new Score { Home = gr.HomeSet1Score, Away = gr.AwaySet1Score, IsTechnicalDefeat = gr.IsSet1TechnicalDefeat },
                         new Score { Home = gr.HomeSet2Score, Away = gr.AwaySet2Score, IsTechnicalDefeat = gr.IsSet2TechnicalDefeat },
                         new Score { Home = gr.HomeSet3Score, Away = gr.AwaySet3Score, IsTechnicalDefeat = gr.IsSet3TechnicalDefeat },
                         new Score { Home = gr.HomeSet4Score, Away = gr.AwaySet4Score, IsTechnicalDefeat = gr.IsSet4TechnicalDefeat },
                         new Score { Home = gr.HomeSet5Score, Away = gr.AwaySet5Score, IsTechnicalDefeat = gr.IsSet5TechnicalDefeat }
                    },
                    SetsScore = new Score { Home = gr.HomeSetsScore, Away = gr.AwaySetsScore },
                    IsTechnicalDefeat = gr.IsTechnicalDefeat
                }
            };
        }

        private Converter<GameResultEntity, Game> GetGameMapping()
        {
            return gr => new Game
            {
                Id = gr.Id,
                TournamentId = gr.TournamentId,
                HomeTeamId = gr.HomeTeamId,
                AwayTeamId = gr.AwayTeamId,
                GameDate = gr.StartTime,
                Round = gr.RoundNumber,
                GameNumber = gr.GameNumber,
                Result = new Result
                {
                    SetScores = new List<Score>
                    {
                         new Score { Home = gr.HomeSet1Score, Away = gr.AwaySet1Score, IsTechnicalDefeat = gr.IsSet1TechnicalDefeat },
                         new Score { Home = gr.HomeSet2Score, Away = gr.AwaySet2Score, IsTechnicalDefeat = gr.IsSet2TechnicalDefeat },
                         new Score { Home = gr.HomeSet3Score, Away = gr.AwaySet3Score, IsTechnicalDefeat = gr.IsSet3TechnicalDefeat },
                         new Score { Home = gr.HomeSet4Score, Away = gr.AwaySet4Score, IsTechnicalDefeat = gr.IsSet4TechnicalDefeat },
                         new Score { Home = gr.HomeSet5Score, Away = gr.AwaySet5Score, IsTechnicalDefeat = gr.IsSet5TechnicalDefeat }
                    },
                    SetsScore = new Score { Home = gr.HomeSetsScore, Away = gr.AwaySetsScore },
                    IsTechnicalDefeat = gr.IsTechnicalDefeat
                }
            };
        }

        #endregion
    }
}
