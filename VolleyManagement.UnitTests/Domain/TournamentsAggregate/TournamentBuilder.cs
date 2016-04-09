namespace VolleyManagement.UnitTests.Domain.TournamentsAggregate
{
    using VolleyManagement.Crosscutting.Contracts.Providers;
    using VolleyManagement.Domain.TournamentsAggregate;

    /// <summary>
    /// Represents a builder of <see cref="TournamentBuilder"/> objects for unit tests for <see cref="Tournament"/>.
    /// </summary>
    internal class TournamentBuilder
    {
        private const char CHAR_A = 'A';
        private const char CHAR_B = 'B';
        private const char CHAR_C = 'C';
        private const int MAX_NAME_LENGTH = 60;
        private const int MAX_DESCRIPTION_LENGTH = 300;
        private const int MAX_REGULATIONS_LINK_LENGTH = 255;
        private const int MIN_SEASON_YEAR = 1900;
        private const int MAX_SEASON_YEAR = 2155;
        private const int ONE = 1;
        private const int FOUR = 4;
        private Tournament _tournament;

        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentBuilder"/> class.
        /// </summary>
        public TournamentBuilder()
        {
            _tournament = new Tournament
            {
                Name = new string(CHAR_A, MAX_NAME_LENGTH),
                Description = new string(CHAR_B, MAX_DESCRIPTION_LENGTH),
                RegulationsLink = new string(CHAR_C, MAX_REGULATIONS_LINK_LENGTH),
                Season = MIN_SEASON_YEAR
            };
        }

        /// <summary>
        /// Sets Name to null.
        /// </summary>
        /// <returns>Instance of <see cref="TournamentBuilder"/></returns>
        public TournamentBuilder WithNullName()
        {
            _tournament.Name = null;
            return this;
        }

        /// <summary>
        /// Sets invalid Name length for tournament. Valid length + 1.
        /// </summary>
        /// <returns>Instance of <see cref="TournamentBuilder"/></returns>
        public TournamentBuilder WithInvalidNameLength()
        {
            _tournament.Name = new string(CHAR_A, MAX_NAME_LENGTH + ONE);
            return this;
        }

        /// <summary>
        /// Sets Description to null.
        /// </summary>
        /// <returns>Instance of <see cref="TournamentBuilder"/></returns>
        public TournamentBuilder WithDescriptionNull()
        {
            _tournament.Description = null;
            return this;
        }

        /// <summary>
        /// Sets invalid Description length for tournament. Valid length + 1.
        /// </summary>
        /// <returns>Instance of <see cref="TournamentBuilder"/></returns>
        public TournamentBuilder WithInvalidDescriptionLength()
        {
            _tournament.Description = new string(CHAR_B, MAX_DESCRIPTION_LENGTH + ONE);
            return this;
        }

        /// <summary>
        /// Sets RegulationsLink to null.
        /// </summary>
        /// <returns>Instance of <see cref="TournamentBuilder"/></returns>
        public TournamentBuilder WithNullRegulationLink()
        {
            _tournament.RegulationsLink = null;
            return this;
        }

        /// <summary>
        /// Sets invalid RegulationsLink length for tournament. Valid length + 1.
        /// </summary>
        /// <returns>Instance of <see cref="TournamentBuilder"/></returns>
        public TournamentBuilder WithInvalidRegulationLinkLength()
        {
            _tournament.RegulationsLink = new string(CHAR_C, MAX_REGULATIONS_LINK_LENGTH + ONE);
            return this;
        }

        /// <summary>
        /// Sets invalid season for tournament. First valid year - 1.
        /// </summary>
        /// <returns>Instance of <see cref="TournamentBuilder"/></returns>
        public TournamentBuilder WithInvalidSeasonMin()
        {
            _tournament.Season = MIN_SEASON_YEAR - ONE;
            return this;
        }

        /// <summary>
        /// Sets invalid season for tournament. Last valid year + 1.
        /// </summary>
        /// <returns>Instance of <see cref="TournamentBuilder"/></returns>
        public TournamentBuilder WithInvalidSeasonMax()
        {
            _tournament.Season = MAX_SEASON_YEAR + ONE;
            return this;
        }

        /// <summary>
        /// Sets GameStart for tournament and set state to not started.
        /// </summary>
        /// <returns>Instance of <see cref="TournamentBuilder"/></returns>
        public TournamentBuilder WithNotStartedState()
        {
            _tournament.GamesStart = TimeProvider.Current.UtcNow.AddMonths(FOUR);
            return this;
        }

        /// <summary>
        /// Sets GameStart and GameEnd for tournament and set state to current.
        /// </summary>
        /// <returns>Instance of <see cref="TournamentBuilder"/></returns>
        public TournamentBuilder WithCurrentState()
        {
            _tournament.GamesStart = TimeProvider.Current.UtcNow;
            _tournament.GamesEnd = TimeProvider.Current.UtcNow.AddMonths(ONE);
            return this;
        }

        /// <summary>
        /// Sets GameStart  for tournament and set state to upcoming.
        /// </summary>
        /// <returns>Instance of <see cref="TournamentBuilder"/></returns>
        public TournamentBuilder WithUpcomingState()
        {
            _tournament.GamesStart = TimeProvider.Current.UtcNow.AddMonths(ONE);
            return this;
        }

        /// <summary>
        /// Sets GameStart and GameEnd for tournament and set state to finished.
        /// </summary>
        /// <returns>Instance of <see cref="TournamentBuilder"/></returns>
        public TournamentBuilder WithFinishedState()
        {
            _tournament.GamesStart = TimeProvider.Current.UtcNow;
            _tournament.GamesEnd = TimeProvider.Current.UtcNow.AddMonths(-ONE);
            return this;
        }

        /// <summary>
        /// Builds instance of <see cref="TournamentBuilder"/>.
        /// </summary>
        /// <returns>Instance of <see cref="Tournament"/>.</returns>
        public Tournament Build()
        {
            return _tournament;
        }
    }
}