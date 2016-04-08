namespace VolleyManagement.UnitTests.Domain.GameAggregate
{
    using System.Collections.Generic;
    using VolleyManagement.Domain.GamesAggregate;

    /// <summary>
    /// Generates test data for <see cref="Score"/>.
    /// </summary>
    public class SetScoresTestFixture
    {
        private List<Score> _setScores;

        /// <summary>
        ///  Initializes a new instance of the <see cref="SetScoresTestFixture"/> class
        /// </summary>
        /// <returns>Instance of <see cref="SetScoresTestFixture"/>.</returns>
        public SetScoresTestFixture()
        {
            _setScores = new List<Score>
            {
                new Score(25, 20),
                new Score(24, 26),
                new Score(28, 30),
                new Score(25, 22),
                new Score(27, 25)
            };
        }

        /// <summary>
        /// Adds sets results with scores to collection of <see cref="Score"/> objects.
        /// </summary>
        /// <returns>Instance of <see cref="SetScoresTestFixture"/>.</returns>
        public SetScoresTestFixture WithFourOneSetScores()
        {
            _setScores = new List<Score>
            {
                new Score(25, 20),
                new Score(25, 20),
                new Score(25, 20),
                new Score(25, 20),
                new Score(23, 25)
            };
            return this;
        }

        /// <summary>
        /// Adds sets results with scores to collection of <see cref="Score"/> objects.
        /// </summary>
        /// <returns>Instance of <see cref="SetScoresTestFixture"/>.</returns>
        public SetScoresTestFixture WithOneFourSetScores()
        {
            _setScores = new List<Score>
            {
                new Score(25, 20),
                new Score(20, 25),
                new Score(20, 25),
                new Score(20, 25),
                new Score(20, 25)
            };
            return this;
        }

        /// <summary>
        /// Adds sets results with scores to collection of <see cref="Score"/> objects.
        /// </summary>
        /// <returns>Instance of <see cref="SetScoresTestFixture"/>.</returns>
        public SetScoresTestFixture WithOneThreeSetScores()
        {
            _setScores = new List<Score>
            {
                new Score(25, 20),
                new Score(20, 25),
                new Score(20, 25),
                new Score(20, 25)
            };
            return this;
        }

        /// <summary>
        /// Builds instance of <see cref="SetScoresTestFixture"/>.
        /// </summary>
        /// <returns>Collection of <see cref="Score"/> objects filled with test data.</returns>
        public List<Score> Build()
        {
            return _setScores;
        }
    }
}