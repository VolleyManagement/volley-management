namespace VolleyManagement.Domain.Tournaments
{
    using System;

    /// <summary>
    /// Container for global constants.
    /// </summary>
    public static class Constants
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
}