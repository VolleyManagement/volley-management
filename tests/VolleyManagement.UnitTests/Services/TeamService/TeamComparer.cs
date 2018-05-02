namespace VolleyManagement.UnitTests.Services.TeamService
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Domain.TeamsAggregate;

    /// <summary>
    /// Comparer for team objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class TeamComparer : IComparer<Team>, IComparer
    {
        /// <summary>
        /// Compares two player objects.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of players.</returns>
        public int Compare(Team x, Team y)
        {
            return AreEqual(x, y) ? 0 : 1;
        }

        /// <summary>
        /// Compares two team objects (non-generic implementation).
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of teams.</returns>
        public int Compare(object x, object y)
        {
            var firstTeam = x as Team;
            var secondTeam = y as Team;

            if (firstTeam == null)
            {
                return -1;
            }
            else if (secondTeam == null)
            {
                return 1;
            }

            return Compare(firstTeam, secondTeam);
        }

        /// <summary>
        /// Finds out whether two team objects have the same properties.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if given team have the same properties.</returns>
        public bool AreEqual(Team x, Team y)
        {
            var simpleDataIsEquialent = x.Id == y.Id &&
                x.Name.Equals(y.Name) &&
                x.Coach.Equals(y.Coach) &&
                x.Achievements.Equals(y.Achievements) &&
                x.Captain.Id == y.Captain.Id;

            if (simpleDataIsEquialent)
            {
                var xRosterIds = x.Roster.Select(p => p.Id);
                var yRosterIds = y.Roster.Select(p => p.Id);

                return xRosterIds.SequenceEqual(yRosterIds);
            }
            else
            {
                return false;
            }
        }
    }
}