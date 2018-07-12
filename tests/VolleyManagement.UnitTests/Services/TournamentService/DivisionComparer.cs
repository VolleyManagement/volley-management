namespace VolleyManagement.UnitTests.Services.TeamService
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Domain.TournamentsAggregate;

    /// <summary>
    /// Comparer for team objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class DivisionComparer : IComparer<Division>, IComparer, IEqualityComparer<Division>
    {
        /// <summary>
        /// Compares two Divisions.
        /// </summary>
        /// <param name="x">The first Division to compare.</param>
        /// <param name="y">The second Division to compare.</param>
        /// <returns>A signed integer that indicates the relative values of Divisions.</returns>
        public int Compare(Division x, Division y)
        {
            return AreEqual(x, y) ? 0 : 1;
        }

        /// <summary>
        /// Compares two Division objects (non-generic implementation).
        /// </summary>
        /// <param name="x">The first Division to compare.</param>
        /// <param name="y">The second Division to compare.</param>
        /// <returns>A signed integer that indicates the relative values of teams.</returns>
        public int Compare(object x, object y)
        {
            var firstDivision = x as Division;
            var secondDivision = y as Division;

            if (firstDivision == null)
            {
                return -1;
            }
            else if (secondDivision == null)
            {
                return 1;
            }

            return Compare(firstDivision, secondDivision);
        }

        /// <summary>
        /// Finds out whether two Divisions have the same properties.
        /// </summary>
        /// <param name="x">The first Division to compare.</param>
        /// <param name="y">The second Division to compare.</param>
        /// <returns>True if given team have the same properties.</returns>
        public bool AreEqual(Division x, Division y)
        {
            // Check primitive fields
            return x.Id == y.Id &&
                   x.Name == y.Name &&
                   x.TournamentId == y.TournamentId;
        }

        public bool Equals(Division x, Division y)
        {
            return AreEqual(x, y);
        }

        public int GetHashCode(Division obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
