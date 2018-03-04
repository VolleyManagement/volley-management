namespace VolleyManagement.UI.Areas.WebAPI.ViewModels.Schedule
{
    public class ScoreViewModel
    {
        public ScoreViewModel()
        {
        }

        public ScoreViewModel(byte home, byte away)
            : this(home, away, false)
        {
        }

        public ScoreViewModel(byte home, byte away, bool isTechnicalDefeat)
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
        /// Gets a value indicating whether gets an indicator whether score is empty.
        /// </summary>
        /// <returns>True if score is empty; otherwise, false.</returns>
        public bool IsEmpty => Home == 0 && Away == 0;

        /// <summary>
        /// Gets or sets a value indicating whether the technical defeat has taken place.
        /// </summary>
        public bool IsTechnicalDefeat { get; set; }
    }
}
