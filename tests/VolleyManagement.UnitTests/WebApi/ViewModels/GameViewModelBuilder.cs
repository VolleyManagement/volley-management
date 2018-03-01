using System;

namespace VolleyManagement.UnitTests.WebApi.ViewModels
{
    using System.Diagnostics.CodeAnalysis;
    using UI.Areas.WebApi.ViewModels.Games;

    /// <summary>
    /// Builder for test game view models
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class GameViewModelBuilder
    {
        /// <summary>
        /// Holds test game view model instance
        /// </summary>
        private GameViewModel _gameViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameViewModelBuilder"/> class
        /// </summary>
        public GameViewModelBuilder()
        {
            _gameViewModel = new GameViewModel()
            {
                Id = 1,
                HomeTeamName = "HomeTeam",
                AwayTeamName = "AwayTeam",
                Date = new DateTime(2016, 4, 3, 13, 0, 0, DateTimeKind.Utc),
                Result = new GameViewModel.GameResult()
            };
        }

        /// <summary>
        /// Sets id of test game view model
        /// </summary>
        /// <param name="id">Id for test game view model</param>
        /// <returns>Game view model builder object</returns>
        public GameViewModelBuilder WithId(int id)
        {
            _gameViewModel.Id = id;
            return this;
        }

        /// <summary>
        /// Sets home team name of test game view model
        /// </summary>
        /// <param name="name">Home team name for test game view model</param>
        /// <returns>Game view model builder object</returns>
        public GameViewModelBuilder WithHomeTeamName(string name)
        {
            _gameViewModel.HomeTeamName = name;
            return this;
        }

        /// <summary>
        /// Sets away team name of test game view model
        /// </summary>
        /// <param name="name">Away team name for test game view model</param>
        /// <returns>Game view model builder object</returns>
        public GameViewModelBuilder WithAwayTeamName(string name)
        {
            _gameViewModel.AwayTeamName = name;
            return this;
        }

        /// <summary>
        /// Sets game date of test game view model
        /// </summary>
        /// <param name="date">Game date for test game view model</param>
        /// <returns>Game view model builder object</returns>
        public GameViewModelBuilder WithGameDate(DateTime date)
        {
            _gameViewModel.Date = date;
            return this;
        }

        /// <summary>
        /// Sets game result of test game view model
        /// </summary>
        /// <param name="result">Game result for test game view model</param>
        /// <returns>Game view model builder object</returns>
        public GameViewModelBuilder WithGameResult(GameViewModel.GameResult result)
        {
            _gameViewModel.Result = result;
            return this;
        }

        /// <summary>
        /// Builds test game view model
        /// </summary>
        /// <returns>test game view model</returns>
        public GameViewModel Build()
        {
            return _gameViewModel;
        }
    }
}
