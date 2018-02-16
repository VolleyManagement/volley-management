namespace VolleyManagement.Domain.GamesAggregate
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
        /// <param name="isTechnicalDefeat">Indicating whether the technical defeat has taken place.</param>
        public Score(byte home, byte away, bool isTechnicalDefeat)
        {
            Home = home;
            Away = away;
            IsTechnicalDefeat = isTechnicalDefeat;
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

        /// <summary>
        /// Gets or sets a value indicating whether the technical defeat has taken place.
        /// </summary>
        public bool IsTechnicalDefeat { get; set; }

        /// <summary>
        /// Gets a value indicating whether gets an indicator whether score is empty.
        /// </summary>
        /// <returns>True if score is empty; otherwise, false.</returns>
        public bool IsEmpty => Home == 0 && Away == 0;

        public static implicit operator Score((byte Home, byte Away) tuple)
        {
            return new Score(tuple.Home, tuple.Away);
        }

        public static implicit operator Score((byte Home, byte Away, bool IsTechnicalDefeat) tuple)
        {
            return new Score(tuple.Home, tuple.Away, tuple.IsTechnicalDefeat);
        }
    }
}
