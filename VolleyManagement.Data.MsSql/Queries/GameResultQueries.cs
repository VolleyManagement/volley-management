namespace VolleyManagement.Data.MsSql.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;
    using VolleyManagement.Data.Contracts;
    using VolleyManagement.Data.MsSql.Entities;
    using VolleyManagement.Data.Queries.Common;
    using VolleyManagement.Data.Queries.GameResult;
    using VolleyManagement.Domain.GamesAggregate;

    /// <summary>
    /// Provides implementation of game result queries.
    /// </summary>
    public class GameResultQueries : IQuery<GameResultDto, FindByIdCriteria>,
                                     IQuery<List<GameResultDto>, TournamentGameResultsCriteria>,
                                     IQuery<List<Game>, TournamentRoundsGameResultsCriteria>
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

            return gameResults.Any() ? gameResults.ToList() : new List<GameResultDto>();
        }

        /// <summary>
        /// Gets games results of the tournament and rounds by specified criteria.
        /// </summary>
        /// <param name="criteria">Tournament's and round`s game results criteria.</param>
        /// <returns>List of Game of game result.</returns>
        public List<Game> Execute(TournamentRoundsGameResultsCriteria criteria)
        {
            var games = _dalGameResults
                 .Where(gr => gr.TournamentId == criteria.TournamentId
                     && (gr.RoundNumber == criteria.FirstRoundNumber || gr.RoundNumber == criteria.SecondRoundNumber))
                 .Select(GetGameMapping());

            return games.Any() ? games.ToList() : new List<Game>();
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
                HomeSetsScore = gr.HomeSetsScore,
                AwaySetsScore = gr.AwaySetsScore,
                IsTechnicalDefeat = gr.IsTechnicalDefeat,
                HomeSet1Score = gr.HomeSet1Score,
                AwaySet1Score = gr.AwaySet1Score,
                HomeSet2Score = gr.HomeSet2Score,
                AwaySet2Score = gr.AwaySet2Score,
                HomeSet3Score = gr.HomeSet3Score,
                AwaySet3Score = gr.AwaySet3Score,
                HomeSet4Score = gr.HomeSet4Score,
                AwaySet4Score = gr.AwaySet4Score,
                HomeSet5Score = gr.HomeSet5Score,
                AwaySet5Score = gr.AwaySet5Score,
                GameDate = gr.StartTime,
                Round = gr.RoundNumber
            };
        }

        private static Expression<Func<GameResultEntity, Game>> GetGameMapping()
        {
            return gr => new Game
            {
                Id = gr.Id,
                TournamentId = gr.TournamentId,
                HomeTeamId = gr.HomeTeamId,
                AwayTeamId = gr.AwayTeamId,
                GameDate = gr.StartTime,
                Round = gr.RoundNumber,
                Result = new Result
                {
                    SetScores = new List<Score>
                    {
                         new Score { Home = gr.HomeSet1Score, Away = gr.AwaySet1Score },
                         new Score { Home = gr.HomeSet2Score, Away = gr.AwaySet2Score },
                         new Score { Home = gr.HomeSet3Score, Away = gr.AwaySet3Score },
                         new Score { Home = gr.HomeSet4Score, Away = gr.AwaySet4Score },
                         new Score { Home = gr.HomeSet5Score, Away = gr.AwaySet5Score }
                    },
                    SetsScore = new Score { Home = gr.HomeSetsScore, Away = gr.AwaySetsScore }
                }
            };
        }

        #endregion
    }
}
