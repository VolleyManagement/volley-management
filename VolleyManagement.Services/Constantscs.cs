namespace VolleyManagement.Services
{
    /// <summary>
    /// Container for constants.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Constant defined key for Entity's id
        /// </summary>
        public const string ENTITY_ID_KEY = "EntityId";

        /// <summary>
        /// Represents the period between start and end of the applying date
        /// </summary>
        public const int DAYS_BETWEEN_START_AND_END_APPLYING_DATE = 90;

        /// <summary>
        /// Constant defines validation key in tournament controller,
        /// when user typed not unique name
        /// </summary>
        public const string UNIQUE_NAME_KEY = "uniqueName";

        /// <summary>
        /// Constant defines validation key in tournament controller,
        /// when applying date starts before now
        /// </summary>
        public const string APPLYING_START_BEFORE_NOW = "ApplyingStartbeforeNow";

        /// <summary>
        /// Constant defines validation key in tournament controller,
        /// when applying start date is before end
        /// </summary>
        public const string APPLYING_START_DATE_AFTER_END_DATE = "ApplyingStartAfterDate";

        /// <summary>
        /// Constant defines validation key in tournament controller,
        /// when applying period less then defined time
        /// </summary>
        public const string APPLYING_PERIOD_LESS_THREE_MONTH = "ApplyingThreeMonthRule";

        /// <summary>
        /// Constant defines validation key in tournament controller,
        /// when applying end date is after games start
        /// </summary>
        public const string APPLYING_END_DATE_AFTER_START_GAMES = "ApplyingPeriodAfterStartGames";

        /// <summary>
        /// Constant defines validation key in tournament controller,
        /// when start games is after end games
        /// </summary>
        public const string START_GAMES_AFTER_END_GAMES = "StartGamesAfterEndGaes";

        /// <summary>
        /// Constant defines validation key in tournament controller,
        /// when transfer period is before games start
        /// </summary>
        public const string TRANSFER_PERIOD_BEFORE_GAMES_START = "TransferPeriodBeforeGamesStart";

        /// <summary>
        /// Constant defines validation key in tournament controller,
        /// when transfer end before transfer start
        /// </summary>
        public const string TRANSFER_END_BEFORE_TRANSFER_START = "TransferEndBeforeStart";

        /// <summary>
        /// Constant defines validation key in tournament controller,
        /// when transfer end is after games end
        /// </summary>
        public const string TRANSFER_END_AFTER_GAMES_END = "TransferEndAfterGamesEnd";

        /// <summary>
        /// Constant defines the applying start capture
        /// </summary>
        public const string APPLYING_START_CAPTURE = "Applying start date";

        /// <summary>
        /// Constant defines the applying end capture
        /// </summary>
        public const string APPLYING_END_CAPTURE = "Applying end date";

        /// <summary>
        /// Constant defines the games start capture
        /// </summary>
        public const string GAMES_START_CAPTURE = "Games start";

        /// <summary>
        /// Constant defines the games end capture
        /// </summary>
        public const string GAMES_END_CAPTURE = "Games end";

        /// <summary>
        /// Constant defines the transfer start capture
        /// </summary>
        public const string TRANSFER_START_CAPTURE = "Transfer start";

        /// <summary>
        /// Constant defines the transfer end capture
        /// </summary>
        public const string TRANSFER_END_CAPTURE = "Transfer end";
    }
}