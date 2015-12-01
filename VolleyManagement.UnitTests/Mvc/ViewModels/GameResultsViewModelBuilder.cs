namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using UI.Areas.Mvc.ViewModels.GameResults;

    /// <summary>
    /// Builder for test MVC user view models
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
                HomeSetsScore = 2,
                AwaySetsScore = 1,
                HomeSet1Score = 27,
                AwaySet1Score = 25,
                HomeSet2Score = 33,
                AwaySet2Score = 31,
                HomeSet3Score = 23,
                AwaySet3Score = 25,
                HomeSet4Score = 0,
                AwaySet4Score = 0,
                HomeSet5Score = 0,
                AwaySet5Score = 0,
                IsTechnicalDefeat = false
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
        /// Sets home team name of test game results view model
        /// </summary>
        /// <param name="homeTeamId">Value indicates home team name</param>
        /// <returns>Game result view model builder object</returns>
        public GameResultViewModelBuilder WithHomeTeamName(string homeTeamName)
        {
            _gameResultsViewModel.HomeTeamName = homeTeamName;
            return this;
        }

        /// <summary>
        /// Sets away team name of test game results view model
        /// </summary>
        /// <param name="awayTeamId">Value indicates away team name</param>
        /// <returns>Game result view model builder object</returns>
        public GameResultViewModelBuilder WithAwayTeamName(string awayTeamName)
        {
            _gameResultsViewModel.AwayTeamName = awayTeamName;
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