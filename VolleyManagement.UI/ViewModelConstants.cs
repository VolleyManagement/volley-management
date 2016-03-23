namespace VolleyManagement.UI
{
    /// <summary>
    /// Container for ViewModel constants.
    /// </summary>
    public static class ViewModelConstants
    {
        /// <summary>
        /// constant defined for the correct format of last and first name
        /// of the player.
        /// </summary>
        internal const string NAME_VALIDATION_REGEX = @"(^(([a-zA-Zа-яА-ЯёЁіІїЇєЄ]+)([\-' ]?)([a-zA-Zа-яА-ЯёЁіІїЇєЄ]+))+)$"; // @"([ '-]?\p{L})+$"
    }
}