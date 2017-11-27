namespace VolleyManagement.Crosscutting.Contracts.Extensions
{
    using System;
    using System.ComponentModel;
    using System.Linq;

    /// <summary>
    /// Extension methods for enumeration
    /// </summary>
    [Obsolete("Should be removed")]
    public static class EnumExtensions
    {
        /// <summary>
        /// Method to get description for specific enumeration value.
        /// </summary>
        /// <param name="value">Enumeration value</param>
        /// <returns>Description string</returns>
        public static string ToDescription(this Enum value)
        {
            var attribute = value.GetAttribute<DescriptionAttribute>();
            return attribute == null ? value.ToString() : attribute.Description;
        }

        /// <summary>
        /// Generic method to get any type of attribute.
        /// </summary>
        /// <typeparam name="T">Type of attribute.</typeparam>
        /// <param name="value">Enumeration value.</param>
        /// <returns>Specific attribute</returns>
        private static T GetAttribute<T>(this Enum value)
            where T : Attribute
        {
            var type = value.GetType();
            var memberInfo = type.GetMember(value.ToString());
            var attributes = memberInfo[0].GetCustomAttributes(typeof(T), false);
            return (T)attributes.FirstOrDefault();
        }
    }
}