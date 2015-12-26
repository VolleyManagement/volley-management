namespace VolleyManagement.UnitTests.Services.GameResultService
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using VolleyManagement.Domain.GameResultsAggregate;

    /// <summary>
    /// Generates test data for <see cref="GameResultDto"/>.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class GameResultTestFixture
    {
        private List<GameResultDto> _gameResults = new List<GameResultDto>();

        /// <summary>
        /// Generates <see cref="GameResultDto"/> objects filled with test data.
        /// </summary>
        /// <returns>Instance of <see cref="GameResultTestFixture"/>.</returns>
        public GameResultTestFixture TestGameResults()
        {
            _gameResults.Add(new GameResultDto
            {
                Id = 1,
                TournamentId = 1,
                HomeTeamId = 1, AwayTeamId = 2,
                HomeTeamName = "TeamNameA", AwayTeamName = "TeamNameB",
                HomeSetsScore = 3, AwaySetsScore = 2,
                IsTechnicalDefeat = false,
                HomeSet1Score = 25, AwaySet1Score = 20,
                HomeSet2Score = 24, AwaySet2Score = 26,
                HomeSet3Score = 28, AwaySet3Score = 30,
                HomeSet4Score = 25, AwaySet4Score = 22,
                HomeSet5Score = 27, AwaySet5Score = 25,
            });
            _gameResults.Add(new GameResultDto
            {
                Id = 2,
                TournamentId = 1,
                HomeTeamId = 1, AwayTeamId = 3,
                HomeTeamName = "TeamNameA", AwayTeamName = "TeamNameC",
                HomeSetsScore = 3, AwaySetsScore = 1,
                IsTechnicalDefeat = false,
                HomeSet1Score = 26, AwaySet1Score = 28,
                HomeSet2Score = 25, AwaySet2Score = 15,
                HomeSet3Score = 25, AwaySet3Score = 21,
                HomeSet4Score = 29, AwaySet4Score = 27,
                HomeSet5Score = 0, AwaySet5Score = 0
            });
            _gameResults.Add(new GameResultDto
            {
                Id = 3,
                TournamentId = 1,
                HomeTeamId = 2, AwayTeamId = 3,
                HomeTeamName = "TeamNameB", AwayTeamName = "TeamNameC",
                HomeSetsScore = 0, AwaySetsScore = 3,
                IsTechnicalDefeat = true,
                HomeSet1Score = 0, AwaySet1Score = 25,
                HomeSet2Score = 0, AwaySet2Score = 25,
                HomeSet3Score = 0, AwaySet3Score = 25,
                HomeSet4Score = 0, AwaySet4Score = 0,
                HomeSet5Score = 0, AwaySet5Score = 0
            });

            return this;
        }

        /// <summary>
        /// Adds <see cref="GameResultDto"/> object to collection.
        /// </summary>
        /// <param name="newGameResult"><see cref="GameResultDto"/> object to add.</param>
        /// <returns>Instance of <see cref="GameResultTestFixture"/>.</returns>
        public GameResultTestFixture Add(GameResultDto newGameResult)
        {
            _gameResults.Add(newGameResult);
            return this;
        }

        /// <summary>
        /// Builds instance of <see cref="GameResultTestFixture"/>.
        /// </summary>
        /// <returns>Collection of <see cref="GameResultDto"/> objects filled with test data.</returns>
        public List<GameResultDto> Build()
        {
            return _gameResults;
        }
    }
}
