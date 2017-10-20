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
        public Score(byte home, byte away, bool isTechnicalDefeat = false)
        {
            Home = home;
            Away = away;
            IsTechnicalDefeat = isTechnicalDefeat;
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
        /// Gets the score of the home team for statistics.
        /// </summary>
        public byte HomeBallsForStatistics
        {
            get
            {
                return IsTechnicalDefeat ? (byte)0 : Home;
            }
        }

        /// <summary>
        /// Gets the score of the away team  for statistics.
        /// </summary>
        public byte AwayBallsForStatistics
        {
            get
            {
                return IsTechnicalDefeat ? (byte)0 : Away;
            }
        }

        /// <summary>
        /// Gets a value indicating whether gets an indicator whether score is empty.
        /// </summary>
        /// <returns>True if score is empty; otherwise, false.</returns>
        public bool IsEmpty
        {
            get
            {
                return Home == 0 && Away == 0;
            }
        }
    }
}
