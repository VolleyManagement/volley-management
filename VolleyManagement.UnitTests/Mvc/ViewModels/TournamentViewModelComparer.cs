namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using VolleyManagement.UI.Areas.Mvc.ViewModels.Tournaments;

    /// <summary>
    /// Comparer for tournament objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class TournamentViewModelComparer : IComparer<TournamentViewModel>
    {
        /// <summary>
        /// Compares two tournament objects.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of tournaments.</returns>
        public int Compare(TournamentViewModel x, TournamentViewModel y)
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
        /// Finds out whether two tournament objects have the same properties.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if given tournaments have the same properties.</returns>
        private bool IsEqual(TournamentViewModel x, TournamentViewModel y)
        {
            return x.SeasonsList.SequenceEqual(y.SeasonsList) &&
                x.Id == y.Id &&
                x.Description == y.Description &&
                x.Name == y.Name &&
                x.RegulationsLink == y.RegulationsLink &&
                x.Season == y.Season &&
                x.Scheme == y.Scheme &&
                x.IsTransferEnabled == y.IsTransferEnabled;
        }
    }
}
