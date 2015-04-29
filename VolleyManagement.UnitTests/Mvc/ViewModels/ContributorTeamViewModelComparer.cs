namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.ContributorsTeam;

    /// <summary>
    /// Comparer for contributor objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class ContributorTeamViewModelComparer : IComparer<ContributorsTeamViewModel>
    {
        /// <summary>
        /// Compares two contributor objects.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of contributor.</returns>
        public int Compare(ContributorsTeamViewModel x, ContributorsTeamViewModel y)
        {
            if (IsEqual(x, y))
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        /// <summary>
        /// Finds out whether two contributor objects have the same properties.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if given contributor have the same properties.</returns>
        private bool IsEqual(ContributorsTeamViewModel x, ContributorsTeamViewModel y)
        {
            return x.Id == y.Id &&
               x.Name == y.Name &&
               x.CourseDirection == y.CourseDirection &&
               x.Contributors == y.Contributors;
        }
    }
}
