namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Tournaments;

    /// <summary>
    /// Comparer for group objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class GroupViewModelComparer : IComparer<GroupViewModel>, IComparer
    {
        /// <summary>
        /// Compares two group objects.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of groups.</returns>
        public int Compare(GroupViewModel x, GroupViewModel y)
        {
            return AreEqual(x, y) ? 0 : 1;
        }

        /// <summary>
        /// Compares two group objects (non-generic implementation).
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of groups.</returns>
        public int Compare(object x, object y)
        {
            GroupViewModel firstGroup = x as GroupViewModel;
            GroupViewModel secondGroup = y as GroupViewModel;

            if (firstGroup == null)
            {
                return -1;
            }
            else if (secondGroup == null)
            {
                return 1;
            }

            return Compare(firstGroup, secondGroup);
        }

        /// <summary>
        /// Finds out whether two group objects have the same properties.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if given groups have the same properties.</returns>
        internal bool AreEqual(GroupViewModel x, GroupViewModel y)
        {
            return x.Id == y.Id && x.Name == y.Name && x.DivisionId == y.DivisionId;
        }
    }
}
