namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;
    using Xunit;
    using UI.Areas.Mvc.ViewModels.Teams;
    using FluentAssertions;

    /// <summary>
    /// Comparer for team name view model objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class TeamNameViewModelComparer : IComparer<TeamNameViewModel>, IComparer, IEqualityComparer<TeamNameViewModel>
    {
        /// <summary>
        /// Compares two team objects (non-generic implementation).
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of teams.</returns>
        public int Compare(object x, object y)
        {
            var firstTeam = x as TeamNameViewModel;
            var secondTeam = y as TeamNameViewModel;

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
        /// Compares two teams objects.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of players.</returns>
        public int Compare(TeamNameViewModel x, TeamNameViewModel y)
        {
            return AreEqual(x, y) ? 0 : 1;
        }

        /// <summary>
        /// Finds out whether two team objects have the same properties.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if given team have the same properties.</returns>
        public bool AreEqual(TeamNameViewModel x, TeamNameViewModel y)
        {

            y.Id.Should().Be(x.Id, "Id should be equal");
            y.Name.Should().Be(x.Name, "Name should be equal");
            y.DivisionName.Should().Be(x.DivisionName, "DivisionName should be equal");
            y.GroupName.Should().Be(x.GroupName, "GroupName should be equal");

            return true;
        }

        /// <summary>
        /// Finds out whether two lists of team objects have the same properties.
        /// </summary>
        /// <param name="x">The first list of object to compare.</param>
        /// <param name="y">The second list of object to compare.</param>
        /// <returns>True if given lists of team have the same properties.</returns>
        public bool AreEqual(List<TeamNameViewModel> x, List<TeamNameViewModel> y)
        {
            for (var i = 0; i < x.Count; i++)
            {
                if (!AreEqual(x[i], y[i]))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Finds out whether two team objects have the same properties.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if given team have the same properties.</returns>
        public bool Equals(TeamNameViewModel x, TeamNameViewModel y)
        {
            return AreEqual(x, y);
        }

        /// <summary>
        /// Get objects hash code
        /// </summary>
        /// <param name="obj">object for getting hash code</param>
        /// <returns>integer hash code</returns>
        public int GetHashCode(TeamNameViewModel obj)
        {
            var builder = new StringBuilder();

            builder.Append(obj.Id);
            builder.Append(obj.Name);

            return builder.ToString().GetHashCode();
        }
    }
}
