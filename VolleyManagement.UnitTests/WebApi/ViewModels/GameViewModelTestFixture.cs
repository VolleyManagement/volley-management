namespace VolleyManagement.UnitTests.WebApi.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using VolleyManagement.Domain.GamesAggregate;
    using VolleyManagement.UI.Areas.WebApi.ViewModels.Games;

    /// <summary>
    /// Class for generating test data
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class GameViewModelTestFixture
    {
        private const string DATE_A = "2016-04-03T10:00:00+03:00";

        private const string DATE_B = "2016-04-07T10:00:00+03:00";

        private const string DATE_C = "2016-04-10T10:00:00+03:00";

        /// <summary>
        /// Holds collection of games
        /// </summary>
        private IList<GameViewModel> _games = new List<GameViewModel>();

        /// <summary>
        /// Adds games to collection
        /// </summary>
        /// <returns>Builder object with collection of games</returns>
        public GameViewModelTestFixture TestGames()
        {
            _games.Add(new GameViewModel()
            {
                Id = 1,
                HomeTeamName = "TeamNameA",
                AwayTeamName = "TeamNameB",
                GameDate = DATE_A,
                Result = new GameViewModel.GameResult
                {
                    TotalScore = new Score(3, 2),
                    SetScores = new List<Score>()
                    {
                        new Score(25, 20),
                        new Score(24, 26),
                        new Score(28, 30),
                        new Score(25, 22),
                        new Score(27, 25)
                    },
                    IsTechnicalDefeat = false,
                }
            });
            _games.Add(new GameViewModel()
            {
                Id = 2,
                HomeTeamName = "TeamNameA",
                AwayTeamName = "TeamNameC",
                GameDate = DATE_B,
                Result = new GameViewModel.GameResult
                {
                    TotalScore = new Score(3, 1),
                    SetScores = new List<Score>()
                    {
                        new Score(26, 28),
                        new Score(25, 15),
                        new Score(25, 21),
                        new Score(29, 27),
                        new Score(0, 0)
                    },
                    IsTechnicalDefeat = false,
                }
            });
            _games.Add(new GameViewModel()
            {
                Id = 3,
                HomeTeamName = "TeamNameB",
                AwayTeamName = "TeamNameC",
                GameDate = DATE_C,
                Result = new GameViewModel.GameResult
                {
                    TotalScore = new Score(0, 3),
                    SetScores = new List<Score>()
                    {
                        new Score(0, 25),
                        new Score(0, 25),
                        new Score(0, 25),
                        new Score(0, 0),
                        new Score(0, 0)
                    },
                    IsTechnicalDefeat = true,
                }
            });
            return this;
        }

        /// <summary>
        /// Add game to collection.
        /// </summary>
        /// <param name="newGame">Game to add.</param>
        /// <returns>Builder object with collection of games.</returns>
        public GameViewModelTestFixture AddGame(GameViewModel newGame)
        {
            _games.Add(newGame);
            return this;
        }

        /// <summary>
        /// Builds test data
        /// </summary>
        /// <returns>Game collection</returns>
        public IList<GameViewModel> Build()
        {
            return _games;
        }
    }
}
