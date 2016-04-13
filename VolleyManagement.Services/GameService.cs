namespace VolleyManagement.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using VolleyManagement.Contracts;
    using VolleyManagement.Contracts.Exceptions;
    using VolleyManagement.Data.Contracts;
    using VolleyManagement.Data.Exceptions;
    using VolleyManagement.Data.Queries.Common;
    using VolleyManagement.Data.Queries.GameResult;
    using VolleyManagement.Domain.GamesAggregate;
    using VolleyManagement.Domain.Properties;
    using VolleyManagement.Domain.TournamentsAggregate; 
    using GameResultConstants = VolleyManagement.Domain.Constants.GameResult;

    /// <summary>
    /// Defines an implementation of <see cref="IGameService"/> contract.
    /// </summary>
    public class GameService : IGameService
    {
        #region Fields

        private readonly IGameRepository _gameRepository;

        #endregion

        #region Query objects

        private readonly IQuery<GameResultDto, FindByIdCriteria> _getByIdQuery;
        private readonly IQuery<List<GameResultDto>, TournamentGameResultsCriteria> _tournamentGameResultsQuery;
        private readonly IQuery<Tournament, FindByIdCriteria> _tournamentByIdQuery; 

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="GameService"/> class.
        /// </summary>
        /// <param name="gameRepository">Instance of class which implements <see cref="IGameRepository"/>.</param>
        /// <param name="getByIdQuery">Query which gets <see cref="GameResultDto"/> object by its identifier.</param>
        /// <param name="tournamentGameResultsQuery">Query which gets <see cref="GameResultDto"/> objects
        /// of the specified tournament.</param>
        /// /// <param name="getTournamentByIdQuery">Query which gets <see cref="Tournament"/> object by its identifier.</param>
        public GameService(
            IGameRepository gameRepository,
            IQuery<GameResultDto, FindByIdCriteria> getByIdQuery,
            IQuery<List<GameResultDto>, TournamentGameResultsCriteria> tournamentGameResultsQuery,
            IQuery<Tournament, FindByIdCriteria> getTournamentByIdQuery)
        {
            _gameRepository = gameRepository;
            _getByIdQuery = getByIdQuery;
            _tournamentGameResultsQuery = tournamentGameResultsQuery;
            _tournamentByIdQuery = getTournamentByIdQuery; 
        }

        #endregion

        #region Implementation

        /// <summary>
        /// Creates a new game.
        /// </summary>
        /// <param name="game">Game to create.</param>
        public void Create(Game game)
        {
            if (game == null)
            {
                throw new ArgumentNullException("game");
            }

            ValidateGame(game);

            _gameRepository.Add(game);
            _gameRepository.UnitOfWork.Commit();
        }

        /// <summary>
        /// Gets game result by its identifier.
        /// </summary>
        /// <param name="id">Identifier of game result.</param>
        /// <returns>Instance of <see cref="GameResultDto"/> or null if nothing is obtained.</returns>
        public GameResultDto Get(int id)
        {
            return _getByIdQuery.Execute(new FindByIdCriteria { Id = id });
        }

        /// <summary>
        /// Gets game results of the tournament specified by its identifier.
        /// </summary>
        /// <param name="tournamentId">Identifier of the tournament.</param>
        /// <returns>List of game results of specified tournament.</returns>
        public List<GameResultDto> GetTournamentResults(int tournamentId)
        {
            var gameResults = _tournamentGameResultsQuery
                .Execute(
                new TournamentGameResultsCriteria { TournamentId = tournamentId });

            return gameResults == null ? null : gameResults.ToList(); 
        }

        /// <summary>
        /// Edits specified instance of game.
        /// </summary>
        /// <param name="game">Game to update.</param>
        public void Edit(Game game)
        {
            try
            {
                _gameRepository.Update(game);
            }
            catch (ConcurrencyException ex)
            {
                throw new MissingEntityException(ServiceResources.ExceptionMessages.GameNotFound, ex);
            }

            _gameRepository.UnitOfWork.Commit();
        }

        /// <summary>
        /// Deletes game by its identifier.
        /// </summary>
        /// <param name="id">Identifier of game.</param>
        public void Delete(int id)
        {
            _gameRepository.Remove(id);
            _gameRepository.UnitOfWork.Commit();
        }

        #endregion

        #region Validation methods

        private void ValidateGame(Game game)
        {
            ValidateTeams(game.HomeTeamId, game.AwayTeamId);
            ValidateGameInTournament(game); 
            if (game.Result == null)
            {
                game.Result = new Result();
                return;
            }

            ValidateSetsScore(game.Result.SetsScore, game.Result.IsTechnicalDefeat);
            ValidateSetsScoreMatchesSetScores(game.Result.SetsScore, game.Result.SetScores);
            ValidateSetScoresValues(game.Result.SetScores, game.Result.IsTechnicalDefeat);
            ValidateSetScoresOrder(game.Result.SetScores);
        }

        private void ValidateTeams(int homeTeamId, int awayTeamId)
        {
            if (GameValidation.AreTheSameTeams(homeTeamId, awayTeamId))
            {
                throw new ArgumentException(Resources.GameResultSameTeam);
            }
        }

        private void ValidateSetsScore(Score setsScore, bool isTechnicalDefeat)
        {
            if (!ResultValidation.IsSetsScoreValid(setsScore, isTechnicalDefeat))
            {
                throw new ArgumentException(
                    string.Format(
                    Resources.GameResultSetsScoreInvalid,
                    GameResultConstants.TECHNICAL_DEFEAT_SETS_WINNER_SCORE,
                    GameResultConstants.TECHNICAL_DEFEAT_SETS_LOSER_SCORE));
            }
        }

        private void ValidateSetsScoreMatchesSetScores(Score setsScore, IList<Score> setScores)
        {
            if (!ResultValidation.AreSetScoresMatched(setsScore, setScores))
            {
                throw new ArgumentException(Resources.GameResultSetsScoreNoMatchSetScores);
            }
        }

        private void ValidateSetScoresValues(IList<Score> setScores, bool isTechnicalDefeat)
        {
            bool isPreviousOptionalSetUnplayed = false;

            for (int i = 0; i < setScores.Count; i++)
            {
                if (i < GameResultConstants.SETS_COUNT_TO_WIN)
                {
                    if (!ResultValidation.IsRequiredSetScoreValid(setScores[i], isTechnicalDefeat))
                    {
                        throw new ArgumentException(
                            string.Format(
                            Resources.GameResultRequiredSetScores,
                            GameResultConstants.SET_POINTS_MIN_VALUE_TO_WIN,
                            GameResultConstants.SET_POINTS_MIN_DELTA_TO_WIN,
                            GameResultConstants.TECHNICAL_DEFEAT_SET_WINNER_SCORE,
                            GameResultConstants.TECHNICAL_DEFEAT_SET_LOSER_SCORE));
                    }
                }
                else
                {
                    if (!ResultValidation.IsOptionalSetScoreValid(setScores[i], isTechnicalDefeat))
                    {
                        throw new ArgumentException(
                            string.Format(
                            Resources.GameResultOptionalSetScores,
                            GameResultConstants.SET_POINTS_MIN_VALUE_TO_WIN,
                            GameResultConstants.SET_POINTS_MIN_DELTA_TO_WIN,
                            GameResultConstants.UNPLAYED_SET_HOME_SCORE,
                            GameResultConstants.UNPLAYED_SET_AWAY_SCORE));
                    }

                    if (isPreviousOptionalSetUnplayed)
                    {
                        if (!ResultValidation.IsSetUnplayed(setScores[i]))
                        {
                            throw new ArgumentException(Resources.GameResultPreviousOptionalSetUnplayed);
                        }
                    }

                    isPreviousOptionalSetUnplayed = ResultValidation.IsSetUnplayed(setScores[i]);
                }
            }
        }

        private void ValidateSetScoresOrder(IList<Score> setScores)
        {
            if (!ResultValidation.AreSetScoresOrdered(setScores))
            {
                throw new ArgumentException(Resources.GameResultSetScoresNotOrdered);
            }
        }

        private void ValidateGameInTournament(Game game)
        {
            Tournament tournament = _tournamentByIdQuery
                .Execute(new FindByIdCriteria { Id = game.TournamentId });

            if (tournament == null)
            {
                throw new ArgumentException(Resources.NoSuchToruanment); 
            }

            ValidateFreeDayGame(game);
            ValidateGameDate(tournament, game);
            ValidateGamesInRound(tournament.Id, game);             
            if (tournament.Scheme == TournamentSchemeEnum.One)
            {
                ValidateGamesInTournamentSchemeOne(tournament.Id, game);
            }
            else if (tournament.Scheme == TournamentSchemeEnum.Two)
            {
                ValidateGamesInTournamentSchemeTwo(tournament.Id, game); 
            }
        }

        private void ValidateGamesInRound(int tourunmentId, Game newGame)
        {
            var games = this.GetTournamentResults(tourunmentId);

            if (games != null)
            {
                List<GameResultDto> gamesInRound = games.Where(gr => gr.Round == newGame.Round).ToList();

                foreach (GameResultDto game in gamesInRound)
                {
                    if (GameValidation.AreSameOrderTeamsInGames(game, newGame))
                    {
                        throw new ArgumentException(
                           string.Format(
                           Resources.SameGameInRound,
                           game.HomeTeamName,
                           game.AwayTeamName,
                           game.Round.ToString()));
                    }

                    if (GameValidation.IsTheSameTeamInTwoGames(game, newGame))
                    {
                        throw new ArgumentException(
                          string.Format(
                          Resources.SameTeamInRound,
                           game.HomeTeamName,
                           game.AwayTeamName));
                    }
                }
            }
        }

        private void ValidateGamesInTournamentSchemeTwo(int tournamentId, Game newGame)
        {
            var allGames = this.GetTournamentResults(tournamentId);

            if (allGames != null)
            {
                var games = allGames.Where(gr => gr.Round != newGame.Round).ToList(); 

                var duplicates = games
                    .Where(x => GameValidation.AreSameOrderTeamsInGames(x, newGame))
                    .ToList();

                if (GameValidation.IsFreeDayGame(newGame))
                {
                    if (duplicates.Count == GameValidation.MAX_DUPLICATE_GAMES_IN_SCHEMA_TWO)
                    {
                        throw new ArgumentException(
                            string.Format(
                            Resources.SameGameInTournamentSchemeTwo,
                            duplicates.First().HomeTeamName,
                            duplicates.First().AwayTeamName));
                    }
                }
                else
                {
                    if (duplicates.Count > 0)
                    {
                        SwitchTeamsOrder(newGame);

                        int switchedDuplicatesCount = games
                            .Where(x => GameValidation.AreSameOrderTeamsInGames(x, newGame))
                            .Count();

                        if (switchedDuplicatesCount > 0)
                        {
                            throw new ArgumentException(
                            string.Format(
                            Resources.SameGameInTournamentSchemeTwo,
                            duplicates.First().HomeTeamName,
                            duplicates.First().AwayTeamName));
                        }
                    }
                }
            }
        }

        private void ValidateGamesInTournamentSchemeOne(int tournamentId, Game newGame)
        {
            var queryGames = this.GetTournamentResults(tournamentId);

            List<GameResultDto> games = queryGames == null
                ? new List<GameResultDto>() : queryGames.ToList();

            var duplicates = games
                .Where(x => GameValidation.AreSameTeamsInGames(x, newGame))
                .ToList(); 

            if (duplicates.Count > 0)
            {
                throw new ArgumentException(
                    string.Format(
                    Resources.SameGameInTournamentSchemeOne,
                    duplicates.First().HomeTeamName,
                    duplicates.First().AwayTeamName));
            } 
        }

        private void ValidateFreeDayGame(Game game)
        {
            if (GameValidation.IsFreeDayTeam(game.HomeTeamId) 
                && GameValidation.IsFreeDayTeam(game.AwayTeamId))
            {
                throw new ArgumentException(
                    string.Format(
                    Resources.NoTeamsInGame, game.Round));
            }
            else if (GameValidation.IsFreeDayTeam(game.HomeTeamId))
            {
                SwitchTeamsOrder(game);
            }
        }

        private void ValidateGameDate(Tournament tournament, Game game)
        {
            if (DateTime.Compare(tournament.GamesStart, game.GameDate) > 0
                || DateTime.Compare(tournament.GamesEnd, game.GameDate) < 0)
            {
                throw new ArgumentException(Resources.WrongRoundDate);
            }
        }

        private void SwitchTeamsOrder(Game game)
        {
            int tempHomeId = game.HomeTeamId; 
            game.HomeTeamId = game.AwayTeamId; 
            game.AwayTeamId = tempHomeId; 
        }
        #endregion
    }
}
