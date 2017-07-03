namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using UI.Areas.Mvc.ViewModels.Division;

    /// <summary>
    /// Equality comparer for division view model objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class DivisionViewModelEqualityComparer : IEqualityComparer<DivisionViewModel>
    {
        /// <summary>
        /// Check if objects are equal
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if objects are equal</returns>
        public bool Equals(DivisionViewModel x, DivisionViewModel y)
        {
            return x != null &&
                   y != null &&
                   x.Id == y.Id &&
                   x.Name == y.Name &&
                   x.Groups.SequenceEqual(y.Groups, new GroupViewModelEqualityComparer());
        }

        /// <summary>
        /// Get hash code for the division view model object
        /// </summary>
        /// <param name="obj">Division view model object</param>
        /// <returns>Division's Name as hash code</returns>
        public int GetHashCode(DivisionViewModel obj)
        {
            return obj.Name.GetHashCode();
        }
    }
}
