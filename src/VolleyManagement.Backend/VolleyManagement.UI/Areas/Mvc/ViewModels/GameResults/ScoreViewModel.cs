namespace VolleyManagement.UI.Areas.Mvc.ViewModels.GameResults
{
    using Domain.GamesAggregate;

    public class ScoreViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScoreViewModel"/> class that contains default score.
        /// </summary>
        public ScoreViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScoreViewModel"/> class that contains default score.
        /// </summary>
        public ScoreViewModel(byte home, byte away) : this(home, away, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScoreViewModel"/> class that contains specified score.
        /// </summary>
        /// <param name="home">Score of the home team.</param>
        /// <param name="away">Score of the away team.</param>
        /// <param name="isTechnicalDefeat">Indicating whether the technical defeat has taken place.</param>
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
        public bool IsEmpty
        {
            get
            {
                return Home == 0 && Away == 0;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the technical defeat has taken place.
        /// </summary>
        public bool IsTechnicalDefeat { get; set; }

        /// <summary>
        /// Gets formatted game score
        /// </summary>
        public string GetFormattedScore
        {
            get
            {
                return IsTechnicalDefeat ? $"{Home}:{Away}*" : $"{Home}:{Away}";
            }
        }

        /// <summary>
        /// Maps view model of score to domain model of game.
        /// </summary>
        /// <returns>Domain model of game.</returns>
        public Score ToDomain()
        {
            return new Score {
                Home = Home,
                Away = Away,
                IsTechnicalDefeat = IsTechnicalDefeat
            };
        }
    }
}
