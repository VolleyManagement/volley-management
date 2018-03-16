namespace VolleyManagement.UI.Areas.WebApi.ViewModels.GameReports
{
    using Domain.GameReportsAggregate;

    /// <summary>
    /// Represents a result part of view model for <see cref="ShortGameResultDto"/>.
    /// </summary>
    public class ShortGameResultViewModel
    {
        public ShortGameResultViewModel() { }

        public ShortGameResultViewModel(byte roundNumber, byte homeSetScore, byte awaySetScore)
           : this(homeSetScore, awaySetScore, false) { }
       
        public ShortGameResultViewModel(byte roundNumber, byte homeSetScore, byte awaySetScore, bool isTechnicalDefeat)
            : this(homeSetScore, awaySetScore, isTechnicalDefeat)
        {
            RoundNumber = roundNumber;
        }

        public ShortGameResultViewModel(byte homeSetScore, byte awaySetScore)
            :this(homeSetScore, awaySetScore, false) { }
        
        public ShortGameResultViewModel(byte homeSetScore, byte awaySetScore, bool isTechnicalDefeat)
        {
            HomeSetsScore = homeSetScore;
            AwaySetsScore = awaySetScore;

            IsTechnicalDefeat = isTechnicalDefeat;
        }

        /// <summary>
        /// Gets or sets the final score of the game for the home team.
        /// </summary>
        public byte? HomeSetsScore { get; set; }

        /// <summary>
        /// Gets or sets the final score of the game for the away team.
        /// </summary>
        public byte? AwaySetsScore { get; set; }

        /// <summary>
        /// Gets or sets a number of round game scheduled to be played
        /// </summary>
        public byte RoundNumber { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the technical defeat has taken place.
        /// </summary>
        public bool IsTechnicalDefeat { get; set; }

        public static ShortGameResultViewModel CreatePlannedGame(byte roundNumber)
        {
            return new ShortGameResultViewModel
            {
                RoundNumber = roundNumber
            };
        }
    }
}
