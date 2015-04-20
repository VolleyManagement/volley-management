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
            /// <summary>
            /// constant defined the length of the name field
            /// </summary>
            public const int MAX_NAME_LENGTH = 60;

            /// <summary>
            /// constant defined the length of the description field
            /// </summary>
            public const int MAX_DESCRIPTION_LENGTH = 300;

            /// <summary>
            /// constant defined the length of the season field
            /// </summary>
            public const int MAX_SEASON_LENGTH = 9;

            /// <summary>
            /// constant defined the length of the regulation link field
            /// </summary>
            public const int MAX_REGULATION_LENGTH = 255;

            /// <summary>
            /// constant defined the offset when the schema fields saves or gets from database
            /// schema saves in database by byte / in domain model by short
            /// </summary>
            public const short SCHEMA_VALUE_OFFSET_DOMAIN_TO_DB = 1900;

            /// <summary>
            /// constant defines the minimal year available to season
            /// </summary>
            public const short MINIMAL_SEASON_YEAR = 1900;

            /// <summary>
            /// constant defines the maximal year available to season
            /// </summary>
            public const short MAXIMAL_SEASON_YEAR = 2155;
        }

        /// <summary>
        /// Container for user constants.
        /// </summary>
        public static class User
        {
            /// <summary>
            /// constant defined the length of the name field
            /// </summary>
            public const int MAX_NAME_LENGTH = 60;

            /// <summary>
            /// constant defined the length of the telephone
            /// </summary>
            public const int PHONE_LENGTH = 10;
        }

        /// <summary>
        /// Container for player constants.
        /// </summary>
        public static class Player
        {
            /// <summary>
            /// constant defined the length of the first name field
            /// </summary>
            public const int MAX_FIRST_NAME_LENGTH = 60;

            /// <summary>
            /// constant defined the length of the last name field
            /// </summary>
            public const int MAX_LAST_NAME_LENGTH = 60;

            /// <summary>
            /// constant defined the minimum value of the birth year field
            /// </summary>
            public const int MIN_BIRTH_YEAR = 1900;

            /// <summary>
            /// constant defined the maximum value of the birth year field
            /// </summary>
            public const int MAX_BIRTH_YEAR = 2100;

            /// <summary>
            /// constant defined the minimum value of the height field
            /// </summary>
            public const int MIN_HEIGHT = 10;

            /// <summary>
            /// constant defined the maximum value of the height field
            /// </summary>
            public const int MAX_HEIGHT = 250;

            /// <summary>
            /// constant defined the minimum value of the weight field
            /// </summary>
            public const int MIN_WEIGHT = 10;

            /// <summary>
            /// constant defined the maximum value of the weight field
            /// </summary>
            public const int MAX_WEIGHT = 500;

            /// <summary>
            /// constant defined for the correct format of last and first name
            /// </summary>
            public const string NAME_VALIDATION_REGEX = @"([ '-]?\p{L})+$";
        }

        /// <summary>
        /// Container for team constants.
        /// </summary>
        public static class Team
        {
            /// <summary>
            /// constant defined the length of the team name field
            /// </summary>
            public const int MAX_NAME_LENGTH = 30;

            /// <summary>
            /// constant defined the length of the coach name field
            /// </summary>
            public const int MAX_COACH_NAME_LENGTH = 60;

            /// <summary>
            /// constant defined the length of the achievements field
            /// </summary>
            public const int MAX_ACHIEVEMENTS_LENGTH = 4000;
        }
    }
}