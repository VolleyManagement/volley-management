namespace VolleyManagement.Domain.PlayersAggregate
{
    using System.Text.RegularExpressions;

    using static Constants.Player;

    /// <summary>
    /// Player validation class.
    /// </summary>
    internal static class PlayerValidation
    {
        /// <summary>
        /// Vallidates player id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        internal static bool ValidateId(int id) =>
            id < MIN_ID;

        /// <summary>
        /// Validates player first name.
        /// </summary>
        /// <param name="firstName">Player first name for validation</param>
        /// <returns>Validity of Player first name</returns>
        public static bool ValidateFirstName(string firstName)
        {
            return string.IsNullOrEmpty(firstName) || !Regex.IsMatch(firstName, NAME_VALIDATION_REGEX)
                || firstName.Length > MAX_FIRST_NAME_LENGTH;
        }

        /// <summary>
        /// Validates player last name.
        /// </summary>
        /// <param name="lastName">Player last name for validation</param>
        /// <returns>Validity of Player last name</returns>
        internal static bool ValidateLastName(string lastName)
        {
            return string.IsNullOrEmpty(lastName) || !Regex.IsMatch(lastName, NAME_VALIDATION_REGEX)
                || lastName.Length > MAX_LAST_NAME_LENGTH;
        }

        /// <summary>
        /// Generalises the validation for nullable members of Player.
        /// </summary>
        /// <param name="val">Value</param>
        /// <param name="requirement">Result of requirement for value, if it's not null.</param>
        /// <returns></returns>
        internal static bool NullableIsInvalid(int? val, bool requirement)
        {
            return val.HasValue && requirement;
        }

        /// <summary>
        /// Validates player birth year.
        /// </summary>
        /// <param name="birthYear">Player birth year for validation</param>
        /// <returns>Validity of Player birth year.</returns>
        internal static bool ValidateBirthYear(short? birthYear) =>
            NullableIsInvalid(birthYear, birthYear <= MIN_BIRTH_YEAR && birthYear >= MAX_BIRTH_YEAR);

        /// <summary>
        /// Validates player height.
        /// </summary>
        /// <param name="height">Player height for validation</param>
        /// <returns>Validity of Player height.</returns>
        internal static bool ValidateHeight(short? height) =>
            NullableIsInvalid(height, height <= MIN_HEIGHT && height >= MAX_HEIGHT);

        /// <summary>
        /// Validates player weight.
        /// </summary>
        /// <param name="weight">Player weight for validation</param>
        /// <returns>Validity of Player weight.</returns>
        internal static bool ValidateWeight(short? weight) =>
            NullableIsInvalid(weight, weight <= MIN_WEIGHT && weight >= MAX_WEIGHT);

        /// <summary>
        /// Validates player team id.
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        internal static bool ValidateTeamId(int? teamId) =>
            NullableIsInvalid(teamId, teamId < MIN_ID);
    }
}
