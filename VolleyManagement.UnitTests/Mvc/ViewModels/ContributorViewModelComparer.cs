namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using VolleyManagement.UI.Areas.Mvc.ViewModels.Contributors;

    /// <summary>
    /// Comparer for contributor objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class ContributorViewModelComparer : IComparer<ContributorViewModel>
    {
        /// <summary>
        /// Compares two contributor objects.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of contributor.</returns>
        public int Compare(ContributorViewModel x, ContributorViewModel y)
        {
            if (IsEqual(x, y))
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        /// <summary>
        /// Finds out whether two contributor objects have the same properties.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if given contributor have the same properties.</returns>
        private bool IsEqual(ContributorViewModel x, ContributorViewModel y)
        {
            return x.Id == y.Id &&
                x.FirstName == y.FirstName &&
                x.LastName == y.LastName &&
                x.ContributorTeamId == y.ContributorTeamId;
        }
    }
}
