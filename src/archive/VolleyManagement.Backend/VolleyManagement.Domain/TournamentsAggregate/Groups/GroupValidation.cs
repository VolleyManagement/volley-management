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

        /// <summary>
        /// Checks whether groups count is within the correct range.
        /// </summary>
        /// <param name="count">Groups count within the division.</param>
        /// <returns>True if groups count is within the range; otherwise, false.</returns>
        public static bool IsGroupCountWithinRange(int count)
        {
            return count >= Constants.Group.MIN_GROUPS_COUNT && count <= Constants.Group.MAX_GROUPS_COUNT;
        }
    }
}
