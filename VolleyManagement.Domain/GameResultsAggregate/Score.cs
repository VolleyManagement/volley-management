namespace VolleyManagement.Domain.GameResultsAggregate
{
    /// <summary>
    /// Represents a game score.
    /// </summary>
    public struct Score
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Score"/> structure.
        /// </summary>
        /// <param name="home">Score of the home team.</param>
        /// <param name="away">Score of the away team.</param>
        public Score(byte home, byte away)
            : this()
        {
            this.Home = home;
            this.Away = away;
        }

        /// <summary>
        /// Gets or sets the score of the home team.
        /// </summary>
        public byte Home { get; set; }

        /// <summary>
        /// Gets or sets the score of the away team.
        /// </summary>
        public byte Away { get; set; }
    }
}
