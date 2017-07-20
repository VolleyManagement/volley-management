namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using UI.Areas.Mvc.ViewModels.GameResults;

    /// <summary>
    /// Builder for test MVC game view models
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class GameViewModelBuilder
    {
        private const int GAME_DEFAULT_ID = 1;
        private const int TOURNAMENT_DEFAULT_ID = 1;
        private const int HOME_TEAM_DEFAULT_ID = 1;
        private const int AWAY_TEAM_DEFAULT_ID = 1;
        private const int DEFAULT_ROUND = 1;

        /// <summary>
        /// Holds test game view model instance
        /// </summary>
        private GameViewModel _gameViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameViewModelBuilder"/> class
        /// </summary>
        public GameViewModelBuilder()
        {
            _gameViewModel = new GameViewModel
            {
                TournamentId = TOURNAMENT_DEFAULT_ID,
                AwayTeamId = AWAY_TEAM_DEFAULT_ID,
                HomeTeamId = HOME_TEAM_DEFAULT_ID,
                Round = DEFAULT_ROUND,
                GameDate = default(DateTime),
                GameTime = default(TimeSpan)
            };
        }

        /// <summary>
        /// Builds test game view model
        /// </summary>
        /// <returns>Test game view model</returns>
        public GameViewModel Build()
        {
            return _gameViewModel;
        }
    }
}
