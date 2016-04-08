namespace VolleyManagement.UnitTests.Domain.GameAggregate
{
    using VolleyManagement.Domain.GamesAggregate;

    /// <summary>
    /// Represents a builder of <see cref="SetsScoreBuilder"/> objects for unit tests for <see cref="ResultValidation"/>.
    /// </summary>
    internal class SetsScoreBuilder
    {
        private Score _setScore;

        /// <summary>
        /// Initializes a new instance of the <see cref="SetsScoreBuilder"/> class.
        /// </summary>
        public SetsScoreBuilder()
        {
            _setScore = new Score(0, 0);
        }

        /// <summary>
        /// Sets set score by the specified set number.
        /// </summary>
        /// <param name="homeTeamSetsScore">Set score for home team</param>
        /// <param name="awayTeamSetsScore">Set score for away team</param>
        /// <returns>Instance of <see cref="SetsScoreBuilder"/>.</returns>
        public SetsScoreBuilder WithScore(byte homeTeamSetsScore, byte awayTeamSetsScore)
        {
            _setScore = new Score(homeTeamSetsScore, awayTeamSetsScore);
            return this;
        }

        /// <summary>
        /// Builds instance of <see cref="SetsScoreBuilder"/>.
        /// </summary>
        /// <returns>Instance of <see cref="Score"/>.</returns>
        public Score Build()
        {
            return _setScore;
        }
    }
}