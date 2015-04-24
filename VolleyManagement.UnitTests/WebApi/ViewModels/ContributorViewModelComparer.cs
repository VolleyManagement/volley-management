namespace VolleyManagement.UnitTests.WebApi.ViewModels
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using VolleyManagement.UI.Areas.WebApi.ViewModels.Contributors;

    /// <summary>
    /// Comparer for contributor objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class ContributorViewModelComparer : IComparer<ContributorViewModel>, IComparer
    {
        /// <summary>
        /// Compares two contributors objects.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of contributors.</returns>
        public int Compare(ContributorViewModel x, ContributorViewModel y)
        {
            return this.IsEqual(x, y) ? 0 : 1;
        }

        /// <summary>
        /// Compares two contributor objects (non-generic implementation).
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of contributors.</returns>
        public int Compare(object x, object y)
        {
            ContributorViewModel firstContributor = x as ContributorViewModel;
            ContributorViewModel secondContributor = y as ContributorViewModel;

            if (firstContributor == null)
            {
                return -1;
            }
            else if (secondContributor == null)
            {
                return 1;
            }

            return Compare(firstContributor, secondContributor);
        }

        /// <summary>
        /// Finds out whether two contributor objects have the same properties.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if given contributors have the same properties.</returns>
        private bool IsEqual(ContributorViewModel x, ContributorViewModel y)
        {
            return x.Id == y.Id &&
                x.Name == y.Name &&
                x.ContributorTeamId == y.ContributorTeamId;
        }
    }
}
