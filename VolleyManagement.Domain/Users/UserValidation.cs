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
        ///  Validity of value
        /// </summary>
        static bool invalid = false;

        /// <summary>
        /// Validates email.
        /// </summary>
        /// <param name="email">Email for validation</param>
        /// <returns>Validity of email</returns>
        public static bool ValidateEmail(string email)
        {
            invalid = false;
            if (string.IsNullOrEmpty(email))
                return true;
            email = System.Text.RegularExpressions.Regex.Replace(email, @"(@)(.+)$", DomainMapper);
            if (invalid)
                return true;
            return !System.Text.RegularExpressions.Regex.IsMatch(email,
                   @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                   @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$",
                   RegexOptions.IgnoreCase);
        }

        /// <summary>
        ///  Translate Unicode characters that are outside the US-ASCII character range
        /// </summary>
        /// <param name="match">Represents the results from a single regular expression match</param>
        private static string DomainMapper(Match match)
        {
            IdnMapping idn = new IdnMapping();
            string domainName = match.Groups[2].Value;
            try
            {
                domainName = idn.GetAscii(domainName);
            }
            catch (ArgumentException)
            {
                invalid = true;
            }
            return match.Groups[1].Value + domainName;
        }
        /// <summary>
        /// Validates telephone.
        /// </summary>
        /// <param name="cellPhone">Telephone for validation</param>
        /// <returns>Validity of Telephone</returns>
        public static bool ValidateCellPhone(string cellPhone)
        {
            if (string.IsNullOrEmpty(cellPhone))
            {
                return false;
            }

            string template = @"^(\+)?[\- ]?[\d]{1,2}?[\- ]?(\(?\d{2,3}\)?)?[\- ]?[\d]{1}[\- ]?[\d]{1}[\- ]?[\d]{1}[\- ]?[\d]{1}[\- ]?[\d]{1}[\- ]?[\d]{1}[\- ]?[\d]{1}$";
            var helpExpression = new System.Text.RegularExpressions.Regex(template);
            return !helpExpression.IsMatch(cellPhone);
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