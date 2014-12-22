namespace VolleyManagement.Domain.Users
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Tournaments;

    /// <summary>
    /// User validation class.
    /// </summary>
    public static class UserValidation
    {
        /// <summary>
        /// Validates email.
        /// </summary>
        /// <param name="email">Email for validation</param>
        /// <returns>Validity of email</returns>
        public static bool ValidateEmail(string email)
        {
            return string.IsNullOrEmpty(email);
        }

        /// <summary>
        /// Validates telephone.
        /// </summary>
        /// <param name="cellPhone">Telephone for validation</param>
        /// <returns>Validity of Telephone</returns>
        public static bool ValidateCellPhone(string cellPhone)
        {
            return string.IsNullOrEmpty(cellPhone) || cellPhone.Length != Constants.TelephoneLength || !cellPhone.All(char.IsDigit);
        }

        /// <summary>
        /// Validates user name.
        /// </summary>
        /// <param name="userName">User name for validation</param>
        /// <returns>Validity of User name</returns>
        public static bool ValidateUserName(string userName)
        {
            return string.IsNullOrEmpty(userName) || !userName.All(char.IsLetter) || userName.Length > Constants.MaxNameLength;
        }

        /// <summary>
        /// Validates password.
        /// </summary>
        /// <param name="password">Password for validation</param>
        /// <returns>Validity of Password</returns>
        public static bool ValidatePassword(string password)
        {
            return string.IsNullOrEmpty(password);
        }

        /// <summary>
        /// Validates Full name.
        /// </summary>
        /// <param name="fullName">Full name for validation</param>
        /// <returns>Validity of Full name</returns>
        public static bool ValidateFullName(string fullName)
        {
            if (string.IsNullOrEmpty(fullName))
            {
                return false;
            }

            return fullName.Length > 60 || !fullName.Replace(" ", string.Empty).All(char.IsLetter);
        }
    }
}