namespace VolleyManagement.UnitTests.Services.TeamService
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Domain.TournamentsAggregate;

    /// <summary>
    /// Comparer for Group objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class GroupComparer : IComparer<Group>, IComparer
    {
        /// <summary>
        /// Compares two Groups.
        /// </summary>
        /// <param name="x">The first Group to compare.</param>
        /// <param name="y">The second Group to compare.</param>
        /// <returns>A signed integer that indicates the relative values of Groups.</returns>
        public int Compare(Group x, Group y)
        {
            return AreEqual(x, y) ? 0 : 1;
        }

        /// <summary>
        /// Compares two Group objects (non-generic implementation).
        /// </summary>
        /// <param name="x">The first Group to compare.</param>
        /// <param name="y">The second Group to compare.</param>
        /// <returns>A signed integer that indicates the relative values of teams.</returns>
        public int Compare(object x, object y)
        {
            Group firstGroup = x as Group;
            Group secondGroup = y as Group;

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
        /// Finds out whether two Groups have the same properties.
        /// </summary>
        /// <param name="x">The first Group to compare.</param>
        /// <param name="y">The second Group to compare.</param>
        /// <returns>True if given team have the same properties.</returns>
        public bool AreEqual(Group x, Group y)
        {
            // Check primitive fields
            return x.Id == y.Id &&
                   x.Name == y.Name &&
                   x.DivisionId == y.DivisionId;
        }
    }
}