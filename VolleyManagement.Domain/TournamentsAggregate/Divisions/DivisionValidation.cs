namespace VolleyManagement.Domain.TournamentsAggregate
{
    using System.Collections.Generic;

    /// <summary>
    /// Division validation class.
    /// </summary>
    public static class DivisionValidation
    {
        /// <summary>
        /// Validates division name.
        /// </summary>
        /// <param name="name">Division name for validation</param>
        /// <returns>Validity of Division name</returns>
        public static bool ValidateName(string name)
        {
            return string.IsNullOrEmpty(name) || name.Length > Constants.Division.MAX_NAME_LENGTH;
        }

        /// <summary>
        /// Checks whether groups count is within the correct range.
        /// </summary>
        /// <param name="count">Groups count within the division.</param>
        /// <returns>True if groups count is within the range; otherwise, false.</returns>
        public static bool IsGroupsCountWithinRange(int count)
        {
            return count >= Constants.Division.MIN_GROUPS_COUNT && count <= Constants.Division.MAX_GROUPS_COUNT;
        }
    }
}
