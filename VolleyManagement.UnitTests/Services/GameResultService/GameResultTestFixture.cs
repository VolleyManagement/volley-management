namespace VolleyManagement.UnitTests.Services.GameResultService
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using VolleyManagement.Domain.GameResultsAggregate;

    /// <summary>
    /// Generates test data for <see cref="GameResult"/>.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class GameResultTestFixture
    {
        private List<GameResult> _gameResults = new List<GameResult>();

        /// <summary>
        /// Generates <see cref="GameResult"/> objects filled with test data.
        /// </summary>
        /// <returns>Instance of <see cref="GameResultTestFixture"/>.</returns>
        public GameResultTestFixture TestGameResults()
        {
            _gameResults.Add(new GameResult
            {
                Id = 1,
                TournamentId = 1,
                HomeTeamId = 1,
                AwayTeamId = 2,
                SetsScore = new Score(3, 2),
                IsTechnicalDefeat = false,
                SetScores = new List<Score>
                {
                    new Score(25, 20),
                    new Score(24, 26),
                    new Score(28, 30),
                    new Score(25, 22),
                    new Score(27, 25)
                }
            });
            _gameResults.Add(new GameResult
            {
                Id = 2,
                TournamentId = 1,
                HomeTeamId = 1,
                AwayTeamId = 3,
                SetsScore = new Score(3, 1),
                IsTechnicalDefeat = false,
                SetScores = new List<Score>
                {
                    new Score(26, 28),
                    new Score(25, 15),
                    new Score(25, 21),
                    new Score(29, 27),
                    new Score(0, 0)
                }
            });
            _gameResults.Add(new GameResult
            {
                Id = 3,
                TournamentId = 1,
                HomeTeamId = 2,
                AwayTeamId = 3,
                SetsScore = new Score(0, 3),
                IsTechnicalDefeat = true,
                SetScores = new List<Score>
                {
                    new Score(0, 25),
                    new Score(0, 25),
                    new Score(0, 25),
                    new Score(0, 0),
                    new Score(0, 0)
                }
            });

            return this;
        }

        /// <summary>
        /// Adds <see cref="GameResult"/> object to collection.
        /// </summary>
        /// <param name="newGameResult"><see cref="GameResult"/> object to add.</param>
        /// <returns>Instance of <see cref="GameResultTestFixture"/>.</returns>
        public GameResultTestFixture Add(GameResult newGameResult)
        {
            _gameResults.Add(newGameResult);
            return this;
        }

        /// <summary>
        /// Builds instance of <see cref="GameResultTestFixture"/>.
        /// </summary>
        /// <returns>Collection of <see cref="GameResult"/> objects filled with test data.</returns>
        public List<GameResult> Build()
        {
            return _gameResults;
        }
    }
}
