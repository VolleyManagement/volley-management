namespace VolleyManagement.UnitTests.Services.ContributorService
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using VolleyManagement.Domain.ContributorsAggregate;

    /// <summary>
    /// Comparer for contributor objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class ContributorComparer : IComparer<Contributor>, IComparer
    {
        /// <summary>
        /// Compares two contributor objects.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of contributors.</returns>
        public int Compare(Contributor x, Contributor y)
        {
            return AreEqual(x, y) ? 0 : 1;
        }

        /// <summary>
        /// Compares two contributor objects (non-generic implementation).
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of contributors.</returns>
        public int Compare(object x, object y)
        {
            Contributor firstContributor = x as Contributor;
            Contributor secondContributor = y as Contributor;

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
        public bool AreEqual(Contributor x, Contributor y)
        {
            return x.Id == y.Id &&
                x.Name == y.Name &&
                x.ContributorTeamId == y.ContributorTeamId;
        }
    }
}