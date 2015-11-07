namespace VolleyManagement.UnitTests.WebApi.ViewModels
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using VolleyManagement.UI.Areas.WebApi.ViewModels.ContributorsTeam;

    /// <summary>
    /// Comparer for contributor objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class ContributorTeamViewModelComparer : IComparer<ContributorsTeamViewModel>, IComparer
    {
        /// <summary>
        /// Compares two contributors team objects.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of contributors.</returns>
        public int Compare(ContributorsTeamViewModel x, ContributorsTeamViewModel y)
        {
            return this.IsEqual(x, y) ? 0 : 1;
        }

        /// <summary>
        /// Compares two contributors team objects (non-generic implementation).
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of contributors team.</returns>
        public int Compare(object x, object y)
        {
            ContributorsTeamViewModel firstContributorTeam = x as ContributorsTeamViewModel;
            ContributorsTeamViewModel secondContributorTeam = y as ContributorsTeamViewModel;

            if (firstContributorTeam == null)
            {
                return -1;
            }
            else if (secondContributorTeam == null)
            {
                return 1;
            }

            return Compare(firstContributorTeam, secondContributorTeam);
        }

        /// <summary>
        /// Finds out whether two contributors team objects have the same properties.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if given contributors team have the same properties.</returns>
        private bool IsEqual(ContributorsTeamViewModel x, ContributorsTeamViewModel y)
        {
            return x.Id == y.Id &&
               x.Name == y.Name &&
               x.CourseDirection == y.CourseDirection &&
               x.Contributors.SequenceEqual(y.Contributors);
        }
    }
}
