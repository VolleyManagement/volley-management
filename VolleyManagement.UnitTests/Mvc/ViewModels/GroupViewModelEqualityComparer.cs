namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Division;

    /// <summary>
    /// Equality comparer for group view model objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class GroupViewModelEqualityComparer : IEqualityComparer<GroupViewModel>
    {
        /// <summary>
        /// Check if objects are equal
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if objects are equal</returns>
        public bool Equals(GroupViewModel x, GroupViewModel y)
        {
            return x != null &&
                   y != null &&
                   x.Id == y.Id &&
                   x.Name == y.Name;
        }

        /// <summary>
        /// Get hash code for the group view model object
        /// </summary>
        /// <param name="obj">Group view model object</param>
        /// <returns>Group's Name as hash code</returns>
        public int GetHashCode(GroupViewModel obj)
        {
            return obj.Name.GetHashCode();
        }
    }
}
