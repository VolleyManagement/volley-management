namespace VolleyManagement.Domain
{
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
            public const int MAX_NAME_LENGTH = 60;

            public const int MAX_DESCRIPTION_LENGTH = 300;

            public const int MAX_SEASON_LENGTH = 9;

            public const int MAX_REGULATION_LENGTH = 255;

            public const short SCHEMA_VALUE_OFFSET_DOMAIN_TO_DB = 1900;

            public const short MINIMAL_SEASON_YEAR = 1900;

            public const short MAXIMAL_SEASON_YEAR = 2155;
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