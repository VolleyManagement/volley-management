using VolleyManagement.WebApi.ViewModels.Tournaments;

namespace VolleyManagement.UnitTests.WebApi
{
    using System.Collections;
    using System.Collections.Generic;
    using VolleyManagement.Domain.Tournaments;

    /// <summary>
    /// Comparer for tournament objects.
    /// </summary>
    internal class TournamentsViewModelComparer : IComparer
    {
        /// <summary>
        /// Compares two tournament objects.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of tournaments.</returns>
        public int Compare(Tournament x, TournamentViewModel y)
        {
            if (IsEqual(x, y))
            {
                return 0;
            }
            else if (x.Id < y.Id)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }

        /// <summary>
        /// Compares two tournament objects (non-generic implementation).
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of tournaments.</returns>
        public int Compare(object x, object y)
        {
            Tournament firstTournament = x as Tournament;
            TournamentViewModel secondTournament = y as TournamentViewModel;

            if (firstTournament == null)
            {
                return -1;
            }
            else if (secondTournament == null)
            {
                return 1;
            }

            return Compare(firstTournament, secondTournament);
        }

        /// <summary>
        /// Finds out whether two tournament objects have the same properties.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if given tournaments have the same properties.</returns>
        private bool IsEqual(Tournament x, TournamentViewModel y)
        {
            if (x.Description.Equals(y.Description) && x.Name.Equals(y.Name)
                && x.Id.Equals(y.Id) && x.RegulationsLink.Equals(y.RegulationsLink)
                && x.Season.Equals(y.Season))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
