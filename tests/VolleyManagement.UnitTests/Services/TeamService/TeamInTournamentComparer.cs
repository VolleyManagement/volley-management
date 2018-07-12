namespace VolleyManagement.UnitTests.Services.TeamService
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Domain.TeamsAggregate;
    using FluentAssertions;

    /// <summary>
    /// Comparer for team objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class TeamInTournamentComparer : IComparer<TeamTournamentDto>, IComparer, IEqualityComparer<TeamTournamentDto>
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
            var firstTeam = x as TeamTournamentDto;
            var secondTeam = y as TeamTournamentDto;

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
            y.TeamId.Should().Be(x.TeamId, "TeamId should match.");
            y.TeamName.Should().Be(x.TeamName, "TeamName should match.");
            y.DivisionId.Should().Be(x.DivisionId, "DivisionId should match.");
            y.GroupId.Should().Be(x.GroupId, "GroupId should match.");
            y.GroupName.Should().Be(x.GroupName, "GroupName should match.");

            return true;
        }

        public bool Equals(TeamTournamentDto x, TeamTournamentDto y)
        {
            return AreEqual(x, y);
        }

        public int GetHashCode(TeamTournamentDto obj)
        {
            return obj.TeamId.GetHashCode();
        }
    }
}