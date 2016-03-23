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
        /// Adds game results with all possible scores to collection of <see cref="GameResultDto"/> objects.
        /// </summary>
        /// <returns>Instance of <see cref="GameResultTestFixture"/>.</returns>
        public GameResultTestFixture WithAllPossibleScores()
        {
            _gameResults.Clear();
            _gameResults.Add(new GameResultDto
            {
                Id = 1,
                TournamentId = 1,
                HomeTeamId = 1, AwayTeamId = 2,
                HomeSetsScore = 3, AwaySetsScore = 0,
                IsTechnicalDefeat = false,
                HomeSet1Score = 25, AwaySet1Score = 15,
                HomeSet2Score = 25, AwaySet2Score = 16,
                HomeSet3Score = 25, AwaySet3Score = 19,
                HomeSet4Score = 0, AwaySet4Score = 0,
                HomeSet5Score = 0, AwaySet5Score = 0
            });
            _gameResults.Add(new GameResultDto
            {
                Id = 2,
                TournamentId = 1,
                HomeTeamId = 1, AwayTeamId = 3,
                HomeSetsScore = 3, AwaySetsScore = 1,
                IsTechnicalDefeat = false,
                HomeSet1Score = 24, AwaySet1Score = 26,
                HomeSet2Score = 25, AwaySet2Score = 19,
                HomeSet3Score = 25, AwaySet3Score = 18,
                HomeSet4Score = 25, AwaySet4Score = 23,
                HomeSet5Score = 0, AwaySet5Score = 0
            });
            _gameResults.Add(new GameResultDto
            {
                Id = 3,
                TournamentId = 1,
                HomeTeamId = 2, AwayTeamId = 3,
                HomeSetsScore = 3, AwaySetsScore = 2,
                IsTechnicalDefeat = false,
                HomeSet1Score = 18, AwaySet1Score = 25,
                HomeSet2Score = 25, AwaySet2Score = 10,
                HomeSet3Score = 22, AwaySet3Score = 25,
                HomeSet4Score = 25, AwaySet4Score = 15,
                HomeSet5Score = 25, AwaySet5Score = 12
            });
            _gameResults.Add(new GameResultDto
            {
                Id = 4,
                TournamentId = 1,
                HomeTeamId = 1, AwayTeamId = 2,
                HomeSetsScore = 2, AwaySetsScore = 3,
                IsTechnicalDefeat = false,
                HomeSet1Score = 25, AwaySet1Score = 22,
                HomeSet2Score = 26, AwaySet2Score = 24,
                HomeSet3Score = 23, AwaySet3Score = 25,
                HomeSet4Score = 17, AwaySet4Score = 25,
                HomeSet5Score = 13, AwaySet5Score = 25
            });
            _gameResults.Add(new GameResultDto
            {
                Id = 5,
                TournamentId = 1,
                HomeTeamId = 1, AwayTeamId = 3,
                HomeSetsScore = 1, AwaySetsScore = 3,
                IsTechnicalDefeat = false,
                HomeSet1Score = 24, AwaySet1Score = 26,
                HomeSet2Score = 25, AwaySet2Score = 22,
                HomeSet3Score = 23, AwaySet3Score = 25,
                HomeSet4Score = 13, AwaySet4Score = 25,
                HomeSet5Score = 0, AwaySet5Score = 0
            });
            _gameResults.Add(new GameResultDto
            {
                Id = 6,
                TournamentId = 1,
                HomeTeamId = 2, AwayTeamId = 3,
                HomeSetsScore = 0, AwaySetsScore = 3,
                IsTechnicalDefeat = false,
                HomeSet1Score = 14, AwaySet1Score = 25,
                HomeSet2Score = 27, AwaySet2Score = 29,
                HomeSet3Score = 22, AwaySet3Score = 25,
                HomeSet4Score = 0, AwaySet4Score = 0,
                HomeSet5Score = 0, AwaySet5Score = 0
            });

            return this;
        }

        /// <summary>
        /// Adds game result with no lost sets for one team to collection of <see cref="GameResult"/> objects.
        /// </summary>
        /// <returns>Instance of <see cref="GameResultTestFixture"/>.</returns>
        public GameResultTestFixture WithNoLostSetsForOneTeam()
        {
            _gameResults.Clear();
            _gameResults.Add(new GameResultDto
            {
                Id = 1,
                TournamentId = 1,
                HomeTeamId = 1, AwayTeamId = 2,
                HomeSetsScore = 3, AwaySetsScore = 0,
                IsTechnicalDefeat = false,
                HomeSet1Score = 25, AwaySet1Score = 20,
                HomeSet2Score = 26, AwaySet2Score = 24,
                HomeSet3Score = 30, AwaySet3Score = 28,
                HomeSet4Score = 0, AwaySet4Score = 0,
                HomeSet5Score = 0, AwaySet5Score = 0
            });

            return this;
        }

        /// <summary>
        /// Adds game result with no lost sets and no lost balls for one team to collection of <see cref="GameResult"/> objects.
        /// </summary>
        /// <returns>Instance of <see cref="GameResultTestFixture"/>.</returns>
        public GameResultTestFixture WithNoLostSetsNoLostBallsForOneTeam()
        {
            _gameResults.Clear();
            _gameResults.Add(new GameResultDto
            {
                Id = 1,
                TournamentId = 1,
                HomeTeamId = 1,
                AwayTeamId = 2,
                HomeSetsScore = 3,
                AwaySetsScore = 0,
                IsTechnicalDefeat = true,
                HomeSet1Score = 25,
                AwaySet1Score = 0,
                HomeSet2Score = 25,
                AwaySet2Score = 0,
                HomeSet3Score = 25,
                AwaySet3Score = 0,
                HomeSet4Score = 0,
                AwaySet4Score = 0,
                HomeSet5Score = 0,
                AwaySet5Score = 0
            });

            return this;
        }

        /// <summary>
        /// Adds game results which result in unique points for the teams to collection of <see cref="GameResultDto"/> objects.
        /// </summary>
        /// <returns>Instance of <see cref="GameResultTestFixture"/>.</returns>
        public GameResultTestFixture WithResultsForUniquePoints()
        {
            _gameResults.Clear();
            _gameResults.Add(new GameResultDto
            {
                Id = 1,
                TournamentId = 1,
                HomeTeamId = 3, AwayTeamId = 1,
                HomeTeamName = "TeamNameC", AwayTeamName = "TeamNameA",
                HomeSetsScore = 3, AwaySetsScore = 1,
                IsTechnicalDefeat = false,
                HomeSet1Score = 25, AwaySet1Score = 21,
                HomeSet2Score = 24, AwaySet2Score = 26,
                HomeSet3Score = 28, AwaySet3Score = 26,
                HomeSet4Score = 25, AwaySet4Score = 23,
                HomeSet5Score = 0, AwaySet5Score = 0
            });
            _gameResults.Add(new GameResultDto
            {
                Id = 2,
                TournamentId = 1,
                HomeTeamId = 2, AwayTeamId = 1,
                HomeTeamName = "TeamNameB", AwayTeamName = "TeamNameA",
                HomeSetsScore = 3, AwaySetsScore = 2,
                IsTechnicalDefeat = false,
                HomeSet1Score = 19, AwaySet1Score = 25,
                HomeSet2Score = 23, AwaySet2Score = 25,
                HomeSet3Score = 29, AwaySet3Score = 27,
                HomeSet4Score = 25, AwaySet4Score = 22,
                HomeSet5Score = 25, AwaySet5Score = 23
            });

            return this;
        }

        /// <summary>
        /// Adds game results which result in repetitive points for the teams to collection of <see cref="GameResultDto"/> objects.
        /// </summary>
        /// <returns>Instance of <see cref="GameResultTestFixture"/>.</returns>
        public GameResultTestFixture WithResultsForRepetitivePoints()
        {
            _gameResults.Clear();
            _gameResults.Add(new GameResultDto
            {
                Id = 1,
                TournamentId = 1,
                HomeTeamId = 3, AwayTeamId = 1,
                HomeTeamName = "TeamNameC", AwayTeamName = "TeamNameA",
                HomeSetsScore = 3, AwaySetsScore = 0,
                IsTechnicalDefeat = false,
                HomeSet1Score = 25, AwaySet1Score = 21,
                HomeSet2Score = 28, AwaySet2Score = 26,
                HomeSet3Score = 25, AwaySet3Score = 23,
                HomeSet4Score = 0, AwaySet4Score = 0,
                HomeSet5Score = 0, AwaySet5Score = 0
            });
            _gameResults.Add(new GameResultDto
            {
                Id = 2,
                TournamentId = 1,
                HomeTeamId = 2, AwayTeamId = 1,
                HomeTeamName = "TeamNameB", AwayTeamName = "TeamNameA",
                HomeSetsScore = 3, AwaySetsScore = 1,
                IsTechnicalDefeat = false,
                HomeSet1Score = 19, AwaySet1Score = 25,
                HomeSet2Score = 29, AwaySet2Score = 27,
                HomeSet3Score = 25, AwaySet3Score = 22,
                HomeSet4Score = 25, AwaySet4Score = 23,
                HomeSet5Score = 0, AwaySet5Score = 0
            });

            return this;
        }

        /// <summary>
        /// Adds game results which result in repetitive points and sets ratio for the teams
        /// to collection of <see cref="GameResultDto"/> objects.
        /// </summary>
        /// <returns>Instance of <see cref="GameResultTestFixture"/>.</returns>
        public GameResultTestFixture WithResultsForRepetitivePointsAndSetsRatio()
        {
            _gameResults.Add(new GameResultDto
            {
                Id = 1,
                TournamentId = 1,
                HomeTeamId = 3, AwayTeamId = 1,
                HomeTeamName = "TeamNameC", AwayTeamName = "TeamNameA",
                HomeSetsScore = 3, AwaySetsScore = 1,
                IsTechnicalDefeat = false,
                HomeSet1Score = 25, AwaySet1Score = 21,
                HomeSet2Score = 24, AwaySet2Score = 26,
                HomeSet3Score = 28, AwaySet3Score = 26,
                HomeSet4Score = 25, AwaySet4Score = 23,
                HomeSet5Score = 0, AwaySet5Score = 0
            });
            _gameResults.Add(new GameResultDto
            {
                Id = 2,
                TournamentId = 1,
                HomeTeamId = 2, AwayTeamId = 1,
                HomeTeamName = "TeamNameB", AwayTeamName = "TeamNameA",
                HomeSetsScore = 3, AwaySetsScore = 1,
                IsTechnicalDefeat = false,
                HomeSet1Score = 19, AwaySet1Score = 25,
                HomeSet2Score = 29, AwaySet2Score = 27,
                HomeSet3Score = 25, AwaySet3Score = 22,
                HomeSet4Score = 25, AwaySet4Score = 23,
                HomeSet5Score = 0, AwaySet5Score = 0
            });

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
