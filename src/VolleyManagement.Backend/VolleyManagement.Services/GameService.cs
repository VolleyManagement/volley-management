namespace VolleyManagement.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;
    using Contracts.Authorization;
    using Contracts.Exceptions;
    using Crosscutting.Contracts.Providers;
    using Data.Contracts;
    using Data.Exceptions;
    using Data.Queries.Common;
    using Data.Queries.GameResult;
    using Data.Queries.Team;
    using Data.Queries.Tournament;
    using Domain.GamesAggregate;
    using Domain.Properties;
    using Domain.RolesAggregate;
    using Domain.TeamsAggregate;
    using Domain.TournamentsAggregate;
    using GameResultConstants = Domain.Constants.GameResult;

#pragma warning disable S1200 // Classes should not be coupled to too many other classes (Single Responsibility Principle)
    /// <summary>
    /// Defines an implementation of <see cref="IGameService"/> contract.
    /// </summary>
    public class GameService : IGameService
#pragma warning restore S1200 // Classes should not be coupled to too many other classes (Single Responsibility Principle)
    {
        #region Fields
        private const string FIRST_TEAM_PLACEHOLDER = "<Team 1>";
        private const string SECOND_TEAM_PLACEHOLDER = "<Team 2>";
        private const string WINNER_PREFIX = "Winner";
        private const string LOOSER_PREFIX = "Looser";

        private readonly IGameRepository _gameRepository;
        private readonly IAuthorizationService _authService;
        private readonly ITournamentRepository _tournamentRepository;

        #endregion

        #region Query objects

        private readonly IQuery<Tournament, FindByIdCriteria> _getTournamentInstanceByIdQuery;
        private readonly IQuery<TournamentScheduleDto, TournamentScheduleInfoCriteria> _tournamentScheduleDtoByIdQuery;
        private readonly IQuery<ICollection<Game>, TournamentRoundsGameResultsCriteria> _gamesByTournamentIdRoundsNumberQuery;
        private readonly IQuery<ICollection<Game>, GamesByRoundCriteria> _gamesByTournamentIdInRoundsByNumbersQuery;
        private readonly IQuery<GameResultDto, FindByIdCriteria> _getByIdQuery;
        private readonly IQuery<ICollection<GameResultDto>, TournamentGameResultsCriteria> _tournamentGameResultsQuery;
        private readonly IQuery<ICollection<TeamTournamentDto>, FindByTournamentIdCriteria> _tournamentTeamsQuery;

        #endregion

        #region Constructor

#pragma warning disable S107 // Methods should not have too many parameters
        /// <summary>
        /// Initializes a new instance of the <see cref="GameService"/> class.
        /// </summary>
        /// <param name="gameRepository">Instance of class which implements <see cref="IGameRepository"/>.</param>
        /// <param name="getByIdQuery">Query which gets <see cref="GameResultDto"/> object by its identifier.</param>
        /// <param name="tournamentGameResultsQuery">Query which gets <see cref="GameResultDto"/> objects
        /// of the specified tournament.</param>
        /// <param name="getTournamentByIdQuery">Query which gets <see cref="Tournament"/> object by its identifier.</param>
        /// <param name="gamesByTournamentIdRoundsNumberQuery">Query which gets <see cref="Game"/> object by its identifier.</param>
        /// <param name="authService">Authorization service</param>
        /// <param name="gamesByTournamentIdInRoundsByNumbersQuery">Query which gets list of <see cref="Game"/> objects.</param>
        /// <param name="gameNumberByTournamentIdQuery">Query which gets game by number</param>
        /// <param name="getTournamentInstanceByIdQuery">Query which gets <see cref="Tournament"/> object by its identifier</param>
        /// <param name="tournamentRepository">Instance of class which implements <see cref="ITournamentRepository"/></param>
        /// <param name="tournamentTeamsQuery"></param>
        public GameService(
            IGameRepository gameRepository,
            IQuery<GameResultDto, FindByIdCriteria> getByIdQuery,
            IQuery<ICollection<GameResultDto>, TournamentGameResultsCriteria> tournamentGameResultsQuery,
            IQuery<TournamentScheduleDto, TournamentScheduleInfoCriteria> getTournamentByIdQuery,
            IQuery<ICollection<Game>, TournamentRoundsGameResultsCriteria> gamesByTournamentIdRoundsNumberQuery,
            IAuthorizationService authService,
            IQuery<ICollection<Game>, GamesByRoundCriteria> gamesByTournamentIdInRoundsByNumbersQuery,
            IQuery<Game, GameByNumberCriteria> gameNumberByTournamentIdQuery,
            IQuery<Tournament, FindByIdCriteria> getTournamentInstanceByIdQuery,
            ITournamentRepository tournamentRepository,
            IQuery<ICollection<TeamTournamentDto>, FindByTournamentIdCriteria> tournamentTeamsQuery)
#pragma warning restore S107 // Methods should not have too many parameters
        {
            _gameRepository = gameRepository;
            _getByIdQuery = getByIdQuery;
            _tournamentGameResultsQuery = tournamentGameResultsQuery;
            _tournamentScheduleDtoByIdQuery = getTournamentByIdQuery;
            _gamesByTournamentIdRoundsNumberQuery = gamesByTournamentIdRoundsNumberQuery;
            _gamesByTournamentIdInRoundsByNumbersQuery = gamesByTournamentIdInRoundsByNumbersQuery;
            _tournamentRepository = tournamentRepository;
            _tournamentTeamsQuery = tournamentTeamsQuery;
            _getTournamentInstanceByIdQuery = getTournamentInstanceByIdQuery;
            _authService = authService;
        }

        #endregion

        #region Implementation

        /// <summary>
        /// Creates a new game.
        /// </summary>
        /// <param name="game">Game to create.</param>
        public void Create(Game game)
        {
            _authService.CheckAccess(AuthOperations.Games.Create);

            if (game == null)
            {
                throw new ArgumentNullException("game");
            }

            if (game.Result != null)
            {
                ValidateResult(game.Result);
            }

            var tournamentScheduleInfo = _tournamentScheduleDtoByIdQuery
                .Execute(new TournamentScheduleInfoCriteria { TournamentId = game.TournamentId });

            ValidateGame(game, tournamentScheduleInfo);
            if (game.Result != null)
            {
                UpdateTournamentLastTimeUpdated(game);
            }
            else
            {
                // Set default empty result
                game.Result = new Result();
            }

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
            var gameResultsDto = _getByIdQuery.Execute(new FindByIdCriteria { Id = id });
            if (gameResultsDto != null)
            {
                gameResultsDto.AllowEditTotalScore = ValidateEditingSchemePlayoff(gameResultsDto);
            }

            return gameResultsDto;
        }

        /// <summary>
        /// Gets game results of the tournament specified by its identifier.
        /// </summary>
        /// <param name="tournamentId">Identifier of the tournament.</param>
        /// <returns>List of game results of specified tournament.</returns>
        public ICollection<GameResultDto> GetTournamentResults(int tournamentId)
        {
            var allGames = QueryAllTournamentGames(tournamentId)
                .Where(gr => gr.HasResult)
                .ToList();

            var tournamentInfo = _tournamentScheduleDtoByIdQuery
                .Execute(new TournamentScheduleInfoCriteria { TournamentId = tournamentId });

            if (tournamentInfo.Scheme == TournamentSchemeEnum.PlayOff)
            {
                SetAbilityToEditResults(allGames);
            }

            return allGames;
        }

        public ICollection<GameResultDto> GetTournamentGames(int tournamentId)
        {
            var allGames = QueryAllTournamentGames(tournamentId);

            var tournamentInfo = _tournamentScheduleDtoByIdQuery
                .Execute(new TournamentScheduleInfoCriteria { TournamentId = tournamentId });

            if (tournamentInfo.Scheme == TournamentSchemeEnum.PlayOff)
            {
                UpdateTeamNamesForPlayoff(allGames, CalculateNumberOfRounds(tournamentInfo.Divisions.First().TeamCount));
            }

            return allGames;
        }

        /// <summary>
        /// Edits specified instance of game.
        /// </summary>
        /// <param name="game">Game to update.</param>
        public void Edit(Game game)
        {
            _authService.CheckAccess(AuthOperations.Games.Edit);

            var tournamentScheduleInfo = _tournamentScheduleDtoByIdQuery
                .Execute(new TournamentScheduleInfoCriteria { TournamentId = game.TournamentId });

            ValidateGame(game, tournamentScheduleInfo);

            // Add autogeneration
            if (tournamentScheduleInfo.Scheme == TournamentSchemeEnum.PlayOff
                && (game.Result != null || game.AwayTeamId == null))
            {
                ScheduleNextGames(game, tournamentScheduleInfo);
            }

            // Provide persisting results in case of editing dates
            var existingGame = Get(game.Id);
            var gameHasResults = existingGame != null && existingGame.Result.SetScores.Any(s => !s.IsEmpty);

            if (gameHasResults)
            {
                game.Result = existingGame.Result;
            }

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
        /// Edits result of specified instance of game.
        /// </summary>
        /// <param name="game">Game which result have to be to update.</param>
        public void EditGameResult(Game game)
        {
            _authService.CheckAccess(AuthOperations.Games.EditResult);

            ValidateResult(game.Result);

            var tournamentScheduleInfo = _tournamentScheduleDtoByIdQuery
                .Execute(new TournamentScheduleInfoCriteria { TournamentId = game.TournamentId });

            ValidateGame(game, tournamentScheduleInfo);

            // Add autogeneration
            if (tournamentScheduleInfo.Scheme == TournamentSchemeEnum.PlayOff)
            {
                ScheduleNextGames(game, tournamentScheduleInfo);
            }

            try
            {
                _gameRepository.Update(game);
            }
            catch (ConcurrencyException ex)
            {
                throw new MissingEntityException(ServiceResources.ExceptionMessages.GameNotFound, ex);
            }

            UpdateTournamentLastTimeUpdated(game);
            _gameRepository.UnitOfWork.Commit();
        }

        /// <summary>
        /// Deletes game by its identifier.
        /// </summary>
        /// <param name="id">Identifier of game.</param>
        public void Delete(int id)
        {
            _authService.CheckAccess(AuthOperations.Games.Delete);

            var game = Get(id);

            if (game == null)
            {
                throw new ArgumentException("invalid game id", nameof(id));
            }

            ValidateGameInRoundOnDelete(game);
            _gameRepository.Remove(id);
            _gameRepository.UnitOfWork.Commit();
        }

        /// <summary>
        /// Method swap all games between two rounds.
        /// </summary>
        /// <param name="tournamentId">Identifier of tournament.</param>
        /// <param name="firstRoundNumber">Identifier of first round number.</param>
        /// <param name="secondRoundNumber">Identifier of second round number.</param>
        public void SwapRounds(int tournamentId, byte firstRoundNumber, byte secondRoundNumber)
        {
            _authService.CheckAccess(AuthOperations.Games.SwapRounds);

            var games = _gamesByTournamentIdRoundsNumberQuery.Execute(
                new TournamentRoundsGameResultsCriteria 
                {
                    TournamentId = tournamentId,
                    FirstRoundNumber = firstRoundNumber,
                    SecondRoundNumber = secondRoundNumber
                });

            try
            {
                foreach (var game in games)
                {
                    game.Round = game.Round == firstRoundNumber ? secondRoundNumber : firstRoundNumber;
                    _gameRepository.Update(game);
                }
            }
            catch (ConcurrencyException ex)
            {
                throw new MissingEntityException(ServiceResources.ExceptionMessages.GameNotFound, ex);
            }

            _gameRepository.UnitOfWork.Commit();
        }

        /// <summary>
        /// Removes all games in tournament by tournament's id
        /// </summary>
        /// <param name="tournamentId">Identifier of the tournament</param>
        public void RemoveAllGamesInTournament(int tournamentId)
        {
            var gamesToRemove = QueryAllTournamentGames(tournamentId);
            foreach (var game in gamesToRemove)
            {
                _gameRepository.Remove(game.Id);
            }
        }

        /// <summary>
        /// Adds collection of new games.
        /// </summary>
        /// <param name="games">Collection of games to add</param>
        public void AddGames(ICollection<Game> games)
        {
            foreach (var game in games)
            {
                _gameRepository.Add(game);
            }
        }

        #endregion

        #region Validation methods

        private void ValidateGame(Game game, TournamentScheduleDto tournamentScheduleInfo)
        {
            ValidateTeams(game.HomeTeamId, game.AwayTeamId, tournamentScheduleInfo);
            ValidateGameInTournament(game, tournamentScheduleInfo);
        }

        private static void ValidateResult(Result result)
        {
            ValidateSetsScore(result.GameScore, result.GameScore.IsTechnicalDefeat);
            ValidateSetsScoreMatchesSetScores(result.GameScore, result.SetScores);
            ValidateSetScoresValues(result.SetScores, result.GameScore.IsTechnicalDefeat);
            ValidateSetScoresOrder(result.SetScores);
        }

        private static void ValidateTeams(int? homeTeamId, int? awayTeamId, TournamentScheduleDto tournamentScheduleInfo)
        {
            if (tournamentScheduleInfo.Scheme == TournamentSchemeEnum.PlayOff)
            {
                if (!(homeTeamId == null && awayTeamId == null) &&
                    GameValidation.AreTheSameTeams(homeTeamId, awayTeamId))
                {
                    throw new ArgumentException(Resources.GameResultSameTeam);
                }
            }
            else if (GameValidation.AreTheSameTeams(homeTeamId, awayTeamId))
            {
                throw new ArgumentException(Resources.GameResultSameTeam);
            }
        }

        private static void ValidateSetsScore(Score setsScore, bool isTechnicalDefeat)
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

        private static void ValidateSetsScoreMatchesSetScores(Score setsScore, IList<Score> setScores)
        {
            if (!ResultValidation.AreSetScoresMatched(setsScore, setScores))
            {
                throw new ArgumentException(Resources.GameResultSetsScoreNoMatchSetScores);
            }
        }

        private static void ValidateSetScoresValuesHigherThanSetCountToWin(IList<Score> setScores, int i,
            int setOrderNumber, ref bool isPreviousOptionalSetUnplayed, bool isTechnicalDefeat)
        {
            if (!ResultValidation.IsOptionalSetScoreValid(setScores[i], isTechnicalDefeat, setOrderNumber))
            {
                if (setOrderNumber == GameResultConstants.MAX_SETS_COUNT)
                {
                    throw new ArgumentException(
                    string.Format(
                    Resources.GameResultFifthSetScoreInvalid,
                    GameResultConstants.FIFTH_SET_POINTS_MIN_VALUE_TO_WIN,
                    GameResultConstants.SET_POINTS_MIN_DELTA_TO_WIN));
                }

                throw new ArgumentException(
                    string.Format(
                    Resources.GameResultOptionalSetScores,
                    GameResultConstants.SET_POINTS_MIN_VALUE_TO_WIN,
                    GameResultConstants.SET_POINTS_MIN_DELTA_TO_WIN,
                    GameResultConstants.UNPLAYED_SET_HOME_SCORE,
                    GameResultConstants.UNPLAYED_SET_AWAY_SCORE));
            }

            if (isPreviousOptionalSetUnplayed && !ResultValidation.IsSetUnplayed(setScores[i]))
            {
                throw new ArgumentException(Resources.GameResultPreviousOptionalSetUnplayed);
            }

            isPreviousOptionalSetUnplayed = ResultValidation.IsSetUnplayed(setScores[i]);
        }
        private static void ValidateSetScoresValues(IList<Score> setScores, bool isTechnicalDefeat)
        {
            var isPreviousOptionalSetUnplayed = false;

            for (int i = 0, setOrderNumber = 1; i < setScores.Count; i++, setOrderNumber++)
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
                    ValidateSetScoresValuesHigherThanSetCountToWin(setScores, i, setOrderNumber,
                        ref isPreviousOptionalSetUnplayed, isTechnicalDefeat);
                }
            }
        }

        private static void ValidateSetScoresOrder(IList<Score> setScores)
        {
            if (!ResultValidation.AreSetScoresOrdered(setScores))
            {
                throw new ArgumentException(Resources.GameResultSetScoresNotOrdered);
            }
        }

        private void ValidateGameInTournament(Game game, TournamentScheduleDto tournamentScheduleInfo)
        {
            if (tournamentScheduleInfo == null)
            {
                throw new ArgumentException(Resources.NoSuchToruanment);
            }

            var allGamesInTournament = QueryAllTournamentGames(game.TournamentId);
            var oldGameToUpdate = allGamesInTournament.SingleOrDefault(gr => gr.Id == game.Id);

            if (oldGameToUpdate != null)
            {
                allGamesInTournament.Remove(oldGameToUpdate);
            }

            PutFreeDayTeamAsAway(game);
            ValidateGameDateSet(game);
            ValidateGameDate(tournamentScheduleInfo, game);
            ValidateGameInRound(game, allGamesInTournament, tournamentScheduleInfo);
            if (tournamentScheduleInfo.Scheme == TournamentSchemeEnum.One)
            {
                ValidateGamesInTournamentSchemeOne(game, allGamesInTournament);
            }
            else if (tournamentScheduleInfo.Scheme == TournamentSchemeEnum.Two)
            {
                ValidateGamesInTournamentSchemeTwo(game, allGamesInTournament);
            }
        }

        private void ValidateGameInRound(Game newGame, ICollection<GameResultDto> games,
            TournamentScheduleDto tournamentSсheduleInfo)
        {
            var teamsInTournament =
                _tournamentTeamsQuery.Execute(new FindByTournamentIdCriteria 
                {
                    TournamentId = tournamentSсheduleInfo.Id
                });

            var newGameDivisionId = (int?)0;

            if (IsNotPlayOffThemeAndBothTeamsNotNull(newGame, tournamentSсheduleInfo))
            {
                newGameDivisionId = teamsInTournament
                    .First(t => t.TeamId == newGame.AwayTeamId || t.TeamId == newGame.HomeTeamId).DivisionId;
            }

            var gamesInSameRoundSameDivision = (
                        from game in games
                        where game.Round == newGame.Round
                        join homeTeam in teamsInTournament
                            on game.HomeTeamId equals homeTeam.TeamId
                        where homeTeam.DivisionId == newGameDivisionId
                        join awayTeam in teamsInTournament
                            on game.HomeTeamId equals awayTeam.TeamId
                        where awayTeam.DivisionId == newGameDivisionId
                        select game
                ).ToList();
            ValidateGameInRoundOnCreate(
                newGame,
                gamesInSameRoundSameDivision,
                tournamentSсheduleInfo);
        }

        private static bool IsNotPlayOffThemeAndBothTeamsNotNull(Game newGame, TournamentScheduleDto tournamentSсheduleInfo)
        {
            return !(newGame.AwayTeamId == null &&
                     newGame.HomeTeamId == null &&
                     tournamentSсheduleInfo.Scheme == TournamentSchemeEnum.PlayOff);
        }

        private static void ValidateGameInRoundOnCreate(
            Game newGame,
            List<GameResultDto> gamesInRound,
            TournamentScheduleDto tournamentScheduleInfo)
        {
            // We are sure that newGame is been created, not edited
            foreach (var game in gamesInRound)
            {
                if (GameValidation.AreSameTeamsInGames(game, newGame))
                {
                    ValidateAreSameTeamsInGames(game, newGame, tournamentScheduleInfo);
                }
                else if (GameValidation.IsTheSameTeamInTwoGames(game, newGame))
                {
                    ValidateIsTheSameTeamInTwoGames(game, newGame, tournamentScheduleInfo);
                }
            }
        }

        private static void ValidateAreSameTeamsInGames(
            GameResultDto game,
            Game newGame,
            TournamentScheduleDto tournamentScheduleInfo)
        {
            string errorMessage = null;
            var tmpres = (tournamentScheduleInfo.Scheme !=
                     TournamentSchemeEnum.PlayOff
                ? Resources.SameFreeDayGameInRound
                : string.Format(
                    Resources.SameTeamInRound,
                    game.HomeTeamId));
            errorMessage = GameValidation.IsFreeDayGame(newGame)
                ? tmpres
                : string.Format(
                    Resources.SameGameInRound,
                    game.HomeTeamName,
                    game.AwayTeamName,
                    game.Round.ToString());
            throw new ArgumentException(errorMessage);
        }

        private static void ValidateIsTheSameTeamInTwoGames(
            GameResultDto game,
            Game newGame,
            TournamentScheduleDto tournamentScheduleInfo)
        {
            if (tournamentScheduleInfo.Scheme == TournamentSchemeEnum.PlayOff)
            {
                return;
            }

            if (GameValidation.IsFreeDayGame(newGame))
            {
                if (game.HomeTeamId != newGame.HomeTeamId
                    && game.AwayTeamId != newGame.HomeTeamId)
                {
                    throw new ArgumentException(Resources.SameFreeDayGameInRound);
                }
                else if (game.HomeTeamId != newGame.HomeTeamId
                         || game.AwayTeamId != newGame.HomeTeamId)
                {
                    throw new ArgumentException(string.Format(
                        Resources.SameTeamInRound,
                        (game.HomeTeamId == newGame.HomeTeamId)
                            ? game.HomeTeamName
                            : game.AwayTeamName));
                }
            }
            else
            {
                throw new ArgumentException(string.Format(
                    Resources.SameTeamInRound,
                    (game.HomeTeamId == newGame.HomeTeamId
                    || game.HomeTeamId == newGame.AwayTeamId)
                        ? game.HomeTeamName
                        : game.AwayTeamName));
            }
        }

        private static void ValidateGameInRoundOnDelete(GameResultDto gameToDelete)
        {
            if (gameToDelete.HasResult)
            {
                throw new ArgumentException(Resources.WrongDeletingGame);
            }
        }

        private static void ValidateGamesInTournamentSchemeTwo(Game newGame, IEnumerable<GameResultDto> games)
        {
            var tournamentGames = games
                .Where(gr => gr.Round != newGame.Round)
                .ToList();

            var duplicates = tournamentGames
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

                    var switchedDuplicatesCount = tournamentGames
                            .Count(x => GameValidation.AreSameOrderTeamsInGames(x, newGame));

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

        private static void ValidateGamesInTournamentSchemeOne(Game newGame, IEnumerable<GameResultDto> games)
        {
            var tournamentGames = games
                .Where(gr => gr.Round != newGame.Round)
                .ToList();

            var duplicates = tournamentGames
                .Where(x => GameValidation.AreSameTeamsInGames(x, newGame))
                .ToList();

            if (duplicates.Count > 0)
            {
                var awayTeamName = GameValidation.IsFreeDayGame(newGame)
                    ? GameResultConstants.FREE_DAY_TEAM_NAME
                    : duplicates.First().AwayTeamName;

                throw new ArgumentException(
                    string.Format(
                    Resources.SameGameInTournamentSchemeOne,
                    duplicates.First().HomeTeamName,
                    awayTeamName));
            }
        }

        private static void PutFreeDayTeamAsAway(Game game)
        {
            if (GameValidation.IsFreeDayTeam(game.HomeTeamId))
            {
                SwitchTeamsOrder(game);
            }
        }

        private static void ValidateGameDateSet(Game game)
        {
            if (!game.GameDate.HasValue)
            {
                throw new ArgumentException(Resources.RoundDateNotSet);
            }
        }

        private static void ValidateGameDate(TournamentScheduleDto tournament, Game game)
        {
            if (DateTime.Compare(tournament.StartDate, game.GameDate.Value) > 0
                || DateTime.Compare(tournament.EndDate, game.GameDate.Value) < 0)
            {
                throw new ArgumentException(Resources.WrongRoundDate);
            }
        }

        private static void SwitchTeamsOrder(Game game)
        {
            if (!GameValidation.IsFreeDayGame(game))
            {
                var tempHomeId = game.HomeTeamId;
                game.HomeTeamId = game.AwayTeamId;
                game.AwayTeamId = tempHomeId;
            }
        }
        #endregion

        #region Schedule autogeneration methods

        private void ScheduleNextGames(Game finishedGame, TournamentScheduleDto tournamentScheduleInfo)
        {
            var gamesToUpdate = GetGamesToSchedule(finishedGame, tournamentScheduleInfo);
            foreach (var nextGame in gamesToUpdate)
            {
                _gameRepository.Update(nextGame);
            }
        }

        private List<Game> GetGamesToSchedule(Game finishedGame, TournamentScheduleDto torunamentScheduleInfo)
        {
            var gamesToUpdate = new List<Game>();

            var gamesInCurrentAndNextRounds = _gamesByTournamentIdInRoundsByNumbersQuery
                .Execute(new GamesByRoundCriteria 
                {
                    TournamentId = torunamentScheduleInfo.Id,
                    RoundNumbers = new List<byte>
                    {
                        finishedGame.Round,
                        Convert.ToByte(finishedGame.Round + 1)
                    }
                });

            // Schedule next games only if finished game is not in last round
            if (!IsGameInLastRound(finishedGame, gamesInCurrentAndNextRounds))
            {
                gamesToUpdate.AddRange(GetGamesToUpdate(finishedGame, gamesInCurrentAndNextRounds));

                if (finishedGame.AwayTeamId.HasValue
                    && finishedGame.Result.GameScore.Home == 0
                    && finishedGame.Result.GameScore.Away == 0)
                {
                    foreach (var game in gamesToUpdate)
                    {
                        ClearGame(finishedGame, game);
                    }
                }
            }

            return gamesToUpdate;
        }

        private static List<Game> GetGamesToUpdate(Game finishedGame, ICollection<Game> gamesInCurrentAndNextRounds)
        {
            var gamesToUpdate = new List<Game>();

            var gamesInCurrentRound = gamesInCurrentAndNextRounds
                        .Where(g => g.Round == finishedGame.Round)
                        .ToList();

            if (IsSemiFinalGame(finishedGame, gamesInCurrentRound)
                       && (finishedGame.AwayTeamId != null))
            {
                gamesToUpdate.Add(
                    GetNextLoserGame(
                    finishedGame,
                    gamesInCurrentAndNextRounds));
            }

            gamesToUpdate.Add(
                GetNextWinnerGame(
                finishedGame,
                gamesInCurrentAndNextRounds));

            return gamesToUpdate;
        }

        private static void ClearGame(Game finishedGame, Game newGame)
        {
            if (finishedGame.GameNumber % 2 != 0)
            {
                newGame.HomeTeamId = null;
            }
            else
            {
                newGame.AwayTeamId = null;
            }
        }

        private static Game GetNextWinnerGame(Game finishedGame, ICollection<Game> games)
        {
            var numberOfRounds = GetNumberOfRounds(finishedGame, games);
            var nextGameNumber = GetNextGameNumber(finishedGame.GameNumber, numberOfRounds);
            if (IsSemiFinalGame(finishedGame, games))
            {
                nextGameNumber++;
            }

            var nextGame = games
                .SingleOrDefault(g => g.GameNumber == nextGameNumber);

            if (finishedGame.HomeTeamId != null)
            {
                var winnerTeamId = 0;
#pragma warning disable S3240 // The simplest possible condition syntax should be used
                if (finishedGame.AwayTeamId == null)
#pragma warning restore S3240 // The simplest possible condition syntax should be used
                {
                    winnerTeamId = finishedGame.HomeTeamId.Value;
                }
                else
                {
                    winnerTeamId = finishedGame.Result.GameScore.Home > finishedGame.Result.GameScore.Away ?
                                   finishedGame.HomeTeamId.Value : finishedGame.AwayTeamId.Value;
                }

                if (finishedGame.GameNumber % 2 != 0)
                {
                    nextGame.HomeTeamId = winnerTeamId;
                }
                else
                {
                    nextGame.AwayTeamId = winnerTeamId;
                }
            }
            return nextGame;
        }

        private static Game GetNextLoserGame(Game finishedGame, ICollection<Game> games)
        {
            var numberOfRounds = GetNumberOfRounds(finishedGame, games);
            // Assume that finished game is a semifinal game
            var nextGameNumber = GetNextGameNumber(finishedGame.GameNumber, numberOfRounds);
            var nextGame = games.SingleOrDefault(g => g.GameNumber == nextGameNumber);

            var loserTeamId = finishedGame.Result.GameScore.Home > finishedGame.Result.GameScore.Away ?
                finishedGame.AwayTeamId.Value : finishedGame.HomeTeamId.Value;

            if (finishedGame.GameNumber % 2 != 0)
            {
                nextGame.HomeTeamId = loserTeamId;
            }
            else
            {
                nextGame.AwayTeamId = loserTeamId;
            }

            return nextGame;
        }

        private static int GetNextGameNumber(int gameNumber, int numberOfRounds)
        {
            return (gameNumber + 1) / 2
                + Convert.ToInt32(Math.Pow(2, numberOfRounds - 1));
        }

        private static (int Home, int Away) GetUpstreamGameNumbers(GameResultDto game, int numberOfRounds)
        {
            var gameNumber =
            (game.Round == numberOfRounds &&
             game.GameNumber % 2 == 0) //Final game - has to be adjusted because of Bronze game
                ? game.GameNumber - 1
                : game.GameNumber;

            var away = 2 * gameNumber - Convert.ToInt32(Math.Pow(2, numberOfRounds));
            var home = away - 1;

            return (home, away);
        }

        private static bool IsSemiFinalGame(Game finishedGame, IEnumerable<Game> games)
        {
            var numberOfRounds = GetNumberOfRounds(finishedGame, games);

            return finishedGame.Round == numberOfRounds - 1;
        }

        private static int GetNumberOfRounds(Game finishedGame, IEnumerable<Game> games)
        {
            var gamesInCurrntRound = games.Where(g => g.Round == finishedGame.Round).ToList();

            return Convert.ToInt32(Math.Log(gamesInCurrntRound.Count, 2))
                + finishedGame.Round;
        }

        private static bool IsGameInLastRound(Game finishedGame, IEnumerable<Game> games)
        {
            var roundNum = games.Max(g => g.Round);
            return roundNum == finishedGame.Round;
        }

        private static void SetAbilityToEditResults(List<GameResultDto> allGames)
        {
            var gamesToAllowEditTotalScore = allGames.Where(
                game => game.HomeTeamId.HasValue
                && game.GameDate.HasValue
                && NextGames(allGames, game)
                .All(next => next.Result.GameScore.Home == 0 && next.Result.GameScore.Away == 0))
                .ToList();

            var gamesNotAllowEditTotalScore = allGames.Where(
                game => game.HomeTeamId.HasValue
                && game.GameDate.HasValue
                && NextGames(allGames, game)
                .All(next => next.Result.GameScore.Home < 4 && next.Result.GameScore.Away < 4))
                .ToList();

            foreach (var game in gamesToAllowEditTotalScore)
            {
                game.AllowEditTotalScore = true;
            }

            foreach (var game in gamesNotAllowEditTotalScore)
            {
                game.AllowEditTotalScore = false;
            }
        }

        private static List<GameResultDto> NextGames(List<GameResultDto> allGames, GameResultDto currentGame)
        {
            if (allGames == null)
            {
                throw new ArgumentNullException(nameof(allGames));
            }

            var numberOfRounds = Convert.ToByte(Math.Sqrt(allGames.Count));
            if (currentGame.Round == numberOfRounds)
            {
                return new List<GameResultDto>();
            }

            var games = new List<GameResultDto>();

            var nextGameNumber = GetNextGameNumber(currentGame.GameNumber, numberOfRounds);
            games.Add(allGames.SingleOrDefault(g => g.GameNumber == nextGameNumber));
            if (currentGame.Round == numberOfRounds - 1)
            {
                var lastGame = allGames.SingleOrDefault(g => g.GameNumber == nextGameNumber + 1);
                if (lastGame != null)
                {
                    games.Add(lastGame);
                }
            }

            return games;
        }

        #endregion

        #region private methods

        private ICollection<GameResultDto> QueryAllTournamentGames(int tournamentId)
        {
            return _tournamentGameResultsQuery
                .Execute(
                    new TournamentGameResultsCriteria { TournamentId = tournamentId });
        }

        private void UpdateTournamentLastTimeUpdated(Game game)
        {
            var tournament = _getTournamentInstanceByIdQuery
               .Execute(new FindByIdCriteria {
                   Id = game.TournamentId
               });
            tournament.LastTimeUpdated = TimeProvider.Current.UtcNow;
            _tournamentRepository.Update(tournament);
        }
        private static void UpdateTeamNamesIfNotFirstRound(GameResultDto game, int numberOfRounds)
        {
            var prefix = IsBronzeGame(game, numberOfRounds)
                                    ? LOOSER_PREFIX
                                    : WINNER_PREFIX;
            var (home, away) = GetUpstreamGameNumbers(game, numberOfRounds);

            if (game.HomeTeamId == null)
            {
                game.HomeTeamName = $"{prefix}{home}";
            }
            if (game.AwayTeamId == null)
            {
                game.AwayTeamName = $"{prefix}{away}";
            }
        }
        private static void UpdateTeamNamesForPlayoff(IEnumerable<GameResultDto> allGames, int numberOfRounds)
        {
            foreach (var game in allGames.Where(game => game.HomeTeamId == null || game.AwayTeamId == null))
            {
                if (game.Round == 1)
                {
                    if (game.HomeTeamId == null && game.AwayTeamId == null)
                    {
                        game.HomeTeamName = FIRST_TEAM_PLACEHOLDER;
                        game.AwayTeamName = SECOND_TEAM_PLACEHOLDER;
                    }
                }
                else
                {
                    UpdateTeamNamesIfNotFirstRound(game, numberOfRounds);
                }
            }
        }

        private static bool IsBronzeGame(GameResultDto game, int numberOfRounds)
        {
            return game.Round == numberOfRounds
                   && game.GameNumber % 2 != 0;
        }

        private bool ValidateEditingSchemePlayoff(GameResultDto game)
        {
            if (game != null&& game.TournamentId!=null)
            {
                var tournamentInfo = _tournamentScheduleDtoByIdQuery
                    .Execute(new TournamentScheduleInfoCriteria {TournamentId = game.TournamentId});

                var gamesInCurrentAndNextRounds = _gamesByTournamentIdInRoundsByNumbersQuery
                    .Execute(new GamesByRoundCriteria {
                        TournamentId = tournamentInfo.Id,
                        RoundNumbers = new List<byte> 
                        {
                            game.Round,
                            Convert.ToByte(game.Round + 1)
                        }
                    });
                var gameone = new Game {
                    Round = game.Round
                };
                var numbersofRounds = GetNumberOfRounds(gameone, gamesInCurrentAndNextRounds);
                var nextGameNumber = GetNextGameNumber(game.GameNumber, numbersofRounds);
                var nextGame = gamesInCurrentAndNextRounds.SingleOrDefault(g => g.GameNumber == nextGameNumber);
                if (nextGame != null && nextGame.AwayTeamId != null && nextGame.HomeTeamId != null)
                {
                    return false;
                }
            }
            return true;
        }

        // Method is not mine: It has to be refactored 
        private static byte CalculateNumberOfRounds(int teamCount)
        {
            byte rounds = 0;
            for (byte i = 0; i < teamCount; i++)
            {
                if (Math.Pow(2, i) >= teamCount)
                {
                    rounds = i;
                    break;
                }
            }

            return rounds;
        }

        #endregion
    }
}
