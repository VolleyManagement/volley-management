namespace VolleyManagement.Domain.Tournaments
{
    using System;

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
            public const int MaxNameLength = 60;

            /// <summary>
            /// constant defined the length of the description field
            /// </summary>
            public const int MaxDescriptionLength = 300;

            /// <summary>
            /// constant defined the length of the season field
            /// </summary>
            public const int MaxSeasonLength = 9;

            /// <summary>
            /// constant defined the length of the regulation link field
            /// </summary>
            public const int MaxRegLinkLength = 255;
        }

        /// <summary>
        /// Container for user constants.
        /// </summary>
        public static class User
        {
            /// <summary>
            /// constant defined the length of the name field
            /// </summary>
            public const int MaxNameLength = 60;

            /// <summary>
            /// constant defined the length of the telephone
            /// </summary>
            public const int PhoneLength = 10;
        }
    }
}