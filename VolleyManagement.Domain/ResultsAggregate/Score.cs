namespace VolleyManagement.Domain.ResultsAggregate
{
    /// <summary>
    /// Represents a game score.
    /// </summary>
    public class Score
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Score"/> class that contains default score.
        /// </summary>
        public Score()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Score"/> class that contains specified score.
        /// </summary>
        /// <param name="home">Score of the home team.</param>
        /// <param name="away">Score of the away team.</param>
        public Score(byte home, byte away)
        {
            Home = home;
            Away = away;
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
