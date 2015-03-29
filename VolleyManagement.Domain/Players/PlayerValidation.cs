namespace VolleyManagement.Domain.Players
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Player validation class.
    /// </summary>
    public static class PlayerValidation
    {
        /// <summary>
        /// Validates player first name.
        /// </summary>
        /// <param name="firstName">Player first name for validation</param>
        /// <returns>Validity of Player first name</returns>
        public static bool ValidateFirstName(string firstName)
        {
            return string.IsNullOrEmpty(firstName) || !firstName.All(char.IsLetter)
                || firstName.Length > Constants.Player.MAX_FIRST_NAME_LENGTH;
        }

        /// <summary>
        /// Validates player last name.
        /// </summary>
        /// <param name="lastName">Player last name for validation</param>
        /// <returns>Validity of Player last name</returns>
        public static bool ValidateLastName(string lastName)
        {
            return string.IsNullOrEmpty(lastName) || !lastName.All(char.IsLetter)
                || lastName.Length > Constants.Player.MAX_LAST_NAME_LENGTH;
        }

        /// <summary>
        /// Validates player birth year.
        /// </summary>
        /// <param name="birthYear">Player birth year for validation</param>
        /// <returns>Validity of Player birth year.</returns>
        public static bool ValidateBirthYear(int birthYear)
        {
            return birthYear <= Constants.Player.MIN_BIRTH_YEAR && birthYear >= Constants.Player.MAX_BIRTH_YEAR;
        }

        /// <summary>
        /// Validates player height.
        /// </summary>
        /// <param name="height">Player height for validation</param>
        /// <returns>Validity of Player height.</returns>
        public static bool ValidateHeight(int height)
        {
            return height <= Constants.Player.MIN_HEIGHT && height >= Constants.Player.MAX_HEIGHT;
        }

        /// <summary>
        /// Validates player weight.
        /// </summary>
        /// <param name="weight">Player weight for validation</param>
        /// <returns>Validity of Player weight.</returns>
        public static bool ValidateWeight(int weight)
        {
            return weight <= Constants.Player.MIN_WEIGHT && weight >= Constants.Player.MAX_WEIGHT;
        }
    }
}
