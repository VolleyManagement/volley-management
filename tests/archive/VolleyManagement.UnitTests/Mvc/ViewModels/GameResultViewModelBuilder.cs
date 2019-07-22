namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Domain.GamesAggregate;
    using UI.Areas.Mvc.ViewModels.GameResults;

    /// <summary>
    /// Builder for test MVC game result view models
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class GameResultViewModelBuilder
    {
        /// <summary>
        /// Holds test Game result view model instance
        /// </summary>
        private GameResultViewModel _gameResultsViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameResultViewModelBuilder"/> class
        /// </summary>
        public GameResultViewModelBuilder()
        {
            _gameResultsViewModel = new GameResultViewModel()
            {
                HomeTeamId = 1,
                AwayTeamId = 2,
                HomeTeamName = "HomeTeam",
                AwayTeamName = "AwayTeam",
                GameDate = DateTime.Parse("2016-04-03 10:00"),
                Round = 1,
                GameNumber = 0,
                GameScore = new ScoreViewModel(3, 1),
                SetScores = new List<ScoreViewModel>()
                    {
                        new ScoreViewModel(27, 25),
                        new ScoreViewModel(33, 31),
                        new ScoreViewModel(27, 25),
                        new ScoreViewModel(24, 26),
                        new ScoreViewModel(),
                    },
                IsTechnicalDefeat = false,
                TournamentId = 1,
                UrlToGameVideo = "http://test-url-a.com",
            };
        }

        /// <summary>
        /// Sets id of test test results view model
        /// </summary>
        /// <param name="id">Id for test user view model</param>
        /// <returns>Game result view model builder object</returns>
        public GameResultViewModelBuilder WithId(int id)
        {
            _gameResultsViewModel.Id = id;
            return this;
        }

        /// <summary>
        /// Sets name of test game results view model
        /// </summary>
        /// <param name="tournamentId">Tournament id</param>
        /// <returns>Game result view model builder object</returns>
        public GameResultViewModelBuilder WithTournamentId(int tournamentId)
        {
            _gameResultsViewModel.TournamentId = tournamentId;
            return this;
        }

        /// <summary>
        /// Sets technical defeat value of test game results view model
        /// </summary>
        /// <param name="technicalDefeat">Value indicates technical defeat</param>
        /// <returns>Game result view model builder object</returns>
        public GameResultViewModelBuilder WithTechnicalDefeat(bool technicalDefeat)
        {
            _gameResultsViewModel.IsTechnicalDefeat = technicalDefeat;
            return this;
        }

        /// <summary>
        /// Sets home team id of test game results view model
        /// </summary>
        /// <param name="homeTeamId">Value indicates home team id</param>
        /// <returns>Game result view model builder object</returns>
        public GameResultViewModelBuilder WithHomeTeamId(int homeTeamId)
        {
            _gameResultsViewModel.HomeTeamId = homeTeamId;
            return this;
        }

        /// <summary>
        /// Sets away team id of test game results view model
        /// </summary>
        /// <param name="awayTeamId">Value indicates away team id</param>
        /// <returns>Game result view model builder object</returns>
        public GameResultViewModelBuilder WithAwayTeamId(int awayTeamId)
        {
            _gameResultsViewModel.AwayTeamId = awayTeamId;
            return this;
        }

        /// <summary>
        /// Sets away team name of test game results view model
        /// </summary>
        /// <param name="awayTeamName">Value indicates away team name</param>
        /// <returns>Game result view model builder object</returns>
        public GameResultViewModelBuilder WithAwayTeamName(string awayTeamName)
        {
            _gameResultsViewModel.AwayTeamName = awayTeamName;
            return this;
        }

        /// <summary>
        /// Sets home team name of test game results view model
        /// </summary>
        /// <param name="homeTeamName">Value indicates home team name</param>
        /// <returns>Game result view model builder object</returns>
        public GameResultViewModelBuilder WithHomeTeamName(string homeTeamName)
        {
            _gameResultsViewModel.HomeTeamName = homeTeamName;
            return this;
        }

        /// <summary>
        /// Sets date of game
        /// </summary>
        /// <param name="date">Value indicates game's date</param>
        /// <returns>Game result view model builder object</returns>
        public GameResultViewModelBuilder WithDate(DateTime date)
        {
            _gameResultsViewModel.GameDate = date;
            return this;
        }

        /// <summary>
        /// Sets round for game
        /// </summary>
        /// <param name="round">Value indicates round for particular game</param>
        /// <returns>Game result view model builder object</returns>
        public GameResultViewModelBuilder WithRound(int round)
        {
            _gameResultsViewModel.Round = round;
            return this;
        }

        /// <summary>
        /// Sets home and away sets score for game
        /// </summary>
        /// <param name="homeSetScore">Value indicates sets score for home team</param>
        /// <param name="awaySetsScore">Value indicates sets score for away team</param>
        /// <returns>Game result view model builder object</returns>
        public GameResultViewModelBuilder WithSetsScore(byte homeSetScore, byte awaySetsScore)
        {
            _gameResultsViewModel.GameScore = new ScoreViewModel(homeSetScore, awaySetsScore);
            return this;
        }

        /// <summary>
        /// Sets home and away set scores for game
        /// </summary>
        /// <param name="setScores">Value indicates set scores of game results</param>
        /// <returns>Game result view model builder object</returns>
        public GameResultViewModelBuilder WithSetScores(List<ScoreViewModel> setScores)
        {
            _gameResultsViewModel.SetScores = setScores;
            return this;
        }

        public GameResultViewModelBuilder WithPenalty(Penalty penalty)
        {
            _gameResultsViewModel.HasPenalty = true;
            _gameResultsViewModel.IsHomeTeamPenalty = penalty.IsHomeTeam;
            _gameResultsViewModel.PenaltyAmount = penalty.Amount;
            _gameResultsViewModel.PenaltyDescrition = penalty.Description;
            return this;
        }

        /// <summary>
        /// Builds test game results view model
        /// </summary>
        /// <returns>test game results view model</returns>
        public GameResultViewModel Build()
        {
            return _gameResultsViewModel;
        }
    }
}