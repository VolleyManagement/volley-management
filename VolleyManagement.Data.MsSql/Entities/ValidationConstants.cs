namespace VolleyManagement.Data.MsSql.Entities
{
    /// <summary>
    /// Database validation constraints
    /// </summary>
    internal class ValidationConstants
    {
        /// <summary>
        /// Tournament entity validation constants
        /// </summary>
        public class Tournament
        {
            public const int MAX_NAME_LENGTH = 60;

            public const int MAX_DESCRIPTION_LENGTH = 300;

            public const int MAX_URL_LENGTH = 255;
        }

        /// <summary>
        /// Player entity validation constants
        /// </summary>
        public class Player
        {
            public const int MAX_FIRST_NAME_LENGTH = 60;

            public const int MAX_LAST_NAME_LENGTH = 60;
        }

        /// <summary>
        /// Team entity validation constants
        /// </summary>
        public class Team
        {
            public const int MAX_NAME_LENGTH = 60;

            public const int MAX_COACH_NAME_LENGTH = 60;

            public const int MAX_ACHIEVEMENTS_LENGTH = 4000;
        }

        /// <summary>
        /// Team entity validation constants
        /// </summary>
        public class Contributor
        {
            public const int MAX_TEAM_NAME_LENGTH = 20;

            public const int MAX_COURSE_NAME_LENGTH = 20;

            public const int MAX_NAME_LENGTH = 30;
        }
    }
}