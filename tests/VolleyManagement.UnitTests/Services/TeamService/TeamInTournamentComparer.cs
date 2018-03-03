namespace VolleyManagement.UnitTests.Services.TeamService
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Domain.TeamsAggregate;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Comparer for team objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class TeamInTournamentComparer : IComparer<TeamTournamentDto>, IComparer
    {
        /// <summary>
        /// Compares two player objects.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of players.</returns>
        public int Compare(TeamTournamentDto x, TeamTournamentDto y)
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
            TeamTournamentDto firstTeam = x as TeamTournamentDto;
            TeamTournamentDto secondTeam = y as TeamTournamentDto;

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
        public bool AreEqual(TeamTournamentDto x, TeamTournamentDto y)
        {
            Assert.AreEqual(x.TeamId, y.TeamId, "TeamId should match.");
            Assert.AreEqual(x.TeamName, y.TeamName, "TeamName should match.");
            Assert.AreEqual(x.DivisionId, y.DivisionId, "DivisionId should match.");
            Assert.AreEqual(x.GroupId, y.GroupId, "GroupId should match.");
            Assert.AreEqual(x.GroupName, y.GroupName, "GroupName should match.");

            return true;
        }
    }
}