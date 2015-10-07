namespace VolleyManagement.UnitTests.Services.ContributorService
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using VolleyManagement.Domain.ContributorsAggregate;

    /// <summary>
    /// Comparer for contributor team objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class ContributorTeamComparer : IComparer<ContributorTeam>, IComparer
    {
        /// <summary>
        /// Compares two contributor team objects.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of contributors team.</returns>
        public int Compare(ContributorTeam x, ContributorTeam y)
        {
            return AreEqual(x, y) ? 0 : 1;
        }

        /// <summary>
        /// Compares two contributors team objects (non-generic implementation).
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of contributors.</returns>
        public int Compare(object x, object y)
        {
            ContributorTeam firstTeamContributor = x as ContributorTeam;
            ContributorTeam secondTeamContributor = y as ContributorTeam;

            if (firstTeamContributor == null)
            {
                return -1;
            }
            else if (secondTeamContributor == null)
            {
                return 1;
            }

            return Compare(firstTeamContributor, secondTeamContributor);
        }

        /// <summary>
        /// Finds out whether two contributors team objects have the same properties.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if given contributors team have the same properties.</returns>
        public bool AreEqual(ContributorTeam x, ContributorTeam y)
        {
            return x.Id == y.Id &&
                x.Name == y.Name &&
                x.CourseDirection == y.CourseDirection &&
                x.Contributors == y.Contributors;
        }
    }
}