namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using UI.Areas.Mvc.ViewModels.Teams;

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
            TeamNameViewModel firstTeam = x as TeamNameViewModel;
            TeamNameViewModel secondTeam = y as TeamNameViewModel;

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

            Assert.AreEqual(x.Id, y.Id, "Id should be equal");
            Assert.AreEqual(x.Name, y.Name, "Name should be equal");
            Assert.AreEqual(x.DivisionName, y.DivisionName, "DivisionName should be equal");

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
            StringBuilder builder = new StringBuilder();

            builder.Append(obj.Id);
            builder.Append(obj.Name);

            return builder.ToString().GetHashCode();
        }
    }
}
