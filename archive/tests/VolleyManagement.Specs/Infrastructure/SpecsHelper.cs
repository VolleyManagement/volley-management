namespace VolleyManagement.Specs.Infrastructure
{
    /// <summary>
    /// Provides help methods for spec tests
    /// </summary>
    public static class SpecsHelper
    {
        /// <summary>
        /// Splits Full Player name into first and last names 
        /// </summary>
        /// <param name="fullName"></param>
        /// <returns></returns>
        public static (string FirstName, string LastName) SplitFullNameToFirstLastNames(string fullName)
        {
            var whitespaceCharIndex = fullName.LastIndexOf(' ');
            var firstName = fullName.Substring(0, whitespaceCharIndex);
            var lastName = fullName.Substring(whitespaceCharIndex + 1);

            return (firstName, lastName);
        }
    }
}
