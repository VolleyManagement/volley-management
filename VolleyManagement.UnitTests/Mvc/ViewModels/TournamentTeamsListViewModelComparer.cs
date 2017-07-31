namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using UI.Areas.Mvc.ViewModels.Teams;

    /// <summary>
    /// Comparer for tournament teams list view model objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class TournamentTeamsListViewModelComparer : IComparer<TournamentTeamsListViewModel>, IComparer
    {
        /// <summary>
        /// Compares two teams list objects.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of teams lists.</returns>
        public int Compare(TournamentTeamsListViewModel x, TournamentTeamsListViewModel y)
        {
            return AreEqual(x, y) ? 0 : 1;
        }

        /// <summary>
        /// Compares two teams list objects (non-generic implementation).
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of teams lists.</returns>
        public int Compare(object x, object y)
        {
            TournamentTeamsListViewModel firstTeamList = x as TournamentTeamsListViewModel;
            TournamentTeamsListViewModel secondTeamList = y as TournamentTeamsListViewModel;

            if (firstTeamList == null)
            {
                return -1;
            }
            else if (secondTeamList == null)
            {
                return 1;
            }

            return Compare(firstTeamList, secondTeamList);
        }

        /// <summary>
        /// Finds out whether two team objects have the same properties.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if given team have the same properties.</returns>
        public bool AreEqual(TournamentTeamsListViewModel x, TournamentTeamsListViewModel y)
        {
            var teamComparer = new TeamNameViewModelComparer();

            bool result = x.TournamentId == y.TournamentId;
            if (result && x.TeamsList != null)
            {
                result &= x.TeamsList.SequenceEqual(y.TeamsList, teamComparer);
            }

            return result;
        }
    }
}
