namespace VolleyManagement.Domain.TournamentsAggregate
{
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
        /// Checks whether divisions count is within the correct range.
        /// </summary>
        /// <param name="count">Divisions count within the tournament.</param>
        /// <returns>True if divisions count is within the range; otherwise, false.</returns>
        public static bool IsDivisionCountWithinRange(int count)
        {
            return count >= Constants.Division.MIN_DIVISIONS_COUNT && count <= Constants.Division.MAX_DIVISIONS_COUNT;
        }
    }
}
