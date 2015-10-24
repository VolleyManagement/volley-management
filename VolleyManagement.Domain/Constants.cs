﻿namespace VolleyManagement.Domain
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Container for global constants.
    /// </summary>
    public static class Constants
    {
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
                        Justification = "It's a COnstants sub-class")]
        public static class ExceptionManagement
        {
            public const string ENTITY_ID_KEY = "EntityId";
        }

        /// <summary>
        /// Container for tournament constants.
        /// </summary>
        public static class Tournament
        {
            /// <summary>
            /// The number of month uses for sets the limit date from now for getting expected tournaments
            /// </summary>
            public const int UPCOMING_TOURNAMENTS_MONTH_LIMIT = 3;

            /// <summary>
            /// constant defined the length of the name field
            /// </summary>
            public const int MAX_NAME_LENGTH = 60;

            public const int MAX_DESCRIPTION_LENGTH = 300;

            public const int MAX_SEASON_LENGTH = 9;

            public const int MAX_REGULATION_LENGTH = 255;

            public const short MINIMAL_SEASON_YEAR = 1900;

            public const short MAXIMAL_SEASON_YEAR = 2155;

            /// <summary>
            /// Constant defines the minimum number of the months of a registration
            /// </summary>
            public const byte MINIMUN_REGISTRATION_PERIOD_MONTH = 3;

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

        /// <summary>
        /// Container for user constants.
        /// </summary>
        public static class User
        {
            public const int MAX_NAME_LENGTH = 60;

            public const int PHONE_LENGTH = 10;
        }

        /// <summary>
        /// Container for player constants.
        /// </summary>
        public static class Player
        {
            public const int MAX_FIRST_NAME_LENGTH = 60;

            public const int MAX_LAST_NAME_LENGTH = 60;

            public const int MIN_BIRTH_YEAR = 1900;

            public const int MAX_BIRTH_YEAR = 2100;

            public const int MIN_HEIGHT = 10;

            public const int MAX_HEIGHT = 250;

            public const int MIN_WEIGHT = 10;

            public const int MAX_WEIGHT = 500;

            public const string NAME_VALIDATION_REGEX = @"([ '-]?\p{L})+$";
        }

        /// <summary>
        /// Container for team constants.
        /// </summary>
        public static class Team
        {
            public const int MAX_NAME_LENGTH = 30;

            public const int MAX_COACH_NAME_LENGTH = 60;

            public const int MAX_ACHIEVEMENTS_LENGTH = 4000;
        }
    }
}