namespace VolleyManagement.Domain
{
    /// <summary>
    /// Container for global constants.
    /// </summary>
    public static class Constants
    {
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
    }
}