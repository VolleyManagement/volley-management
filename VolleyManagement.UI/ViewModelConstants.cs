namespace VolleyManagement.UI
{
    /// <summary>
    /// Container for ViewModel constants.
    /// </summary>
    public static class ViewModelConstants
    {
        /// <summary>
        /// Defines a number of teams that are treated as top teams within the tournament.
        /// </summary>
        public const int NUMBER_OF_TOP_TEAMS = 3;

        /// <summary>
        /// constant defined for the correct format of last and first name
        /// of the player.
        /// </summary>
        internal const string NAME_VALIDATION_REGEX = @"([ '-]?\p{L})+$";
    }
}