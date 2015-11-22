namespace VolleyManagement.Domain.TournamentsAggregate
{
    using System;
    using System.Text.RegularExpressions;

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
    }
}