namespace VolleyManagement.Domain.TournamentsAggregate
{
    /// <summary>
    /// Group validation class.
    /// </summary>
    public static class GroupValidation
    {
        /// <summary>
        /// Validates group name.
        /// </summary>
        /// <param name="name">Group name to validate.</param>
        /// <returns>True if group name if valid; otherwise, false.</returns>
        public static bool ValidateName(string name)
        {
            return !string.IsNullOrEmpty(name) && name.Length <= Constants.Group.MAX_NAME_LENGTH;
        }
    }
}
