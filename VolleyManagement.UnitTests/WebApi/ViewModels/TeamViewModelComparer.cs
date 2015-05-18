namespace VolleyManagement.UnitTests.WebApi.ViewModels
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using VolleyManagement.UI.Areas.WebApi.ViewModels.Teams;

    /// <summary>
    /// Comparer for team objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class TeamViewModelComparer : IComparer<TeamViewModel>, IComparer
    {
        /// <summary>
        /// Compares two team objects.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of teams.</returns>
        public int Compare(TeamViewModel x, TeamViewModel y)
        {
            return this.IsEqual(x, y) ? 0 : 1;
        }

        /// <summary>
        /// Compares two team objects (non-generic implementation).
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of teams.</returns>
        public int Compare(object x, object y)
        {
            TeamViewModel firstTeam = x as TeamViewModel;
            TeamViewModel secondTeam = y as TeamViewModel;

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
        /// <returns>True if given teams have the same properties.</returns>
        private bool IsEqual(TeamViewModel x, TeamViewModel y)
        {
            return x.Id == y.Id &&
                x.Name == y.Name &&
                x.CaptainId == y.CaptainId &&
                x.Coach == y.Coach &&
                x.Achievements == y.Achievements;
        }
    }
}
