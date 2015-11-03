namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System.Collections;

    using VolleyManagement.UI.Areas.Mvc.ViewModels.ContributorsTeam;

    /// <summary>
    /// Comparer for ContributorsTeamViewModel objects.
    /// </summary>
    class ContributorTeamMvcViewModelComparer : IComparer
    {
        /// <summary>
        /// Compares two contributor team objects.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of contributor team.</returns>
        public int Compare(object x, object y)
        {
            return new ContributorTeamViewModelComparer().Compare(x as ContributorsTeamViewModel, y as ContributorsTeamViewModel);
        }
    }
}
