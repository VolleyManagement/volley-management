namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using VolleyManagement.Mvc.ViewModels.Tournaments;

    /// <summary>
    /// Comparer for tournament objects.
    /// </summary>
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
        /// Checks whether lists are equal or not
        /// </summary>
        /// <param name="x">first list</param>
        /// <param name="y">second list</param>
        /// <returns>True, if equal</returns>
        private bool AreSeasonListsEqual(IList<string> x, IList<string> y)
        {
            if (x.Count != y.Count)
            {
                return false;
            }

            bool areSeasonListsEqual = true;
            for (int i = 0; i < x.Count; i++)
            {
                if (!x[i].Equals(y[i]))
                {
                    areSeasonListsEqual = false;
                    break;
                }
            }

            return areSeasonListsEqual;
        }

        /// <summary>
        /// Finds out whether two tournament objects have the same properties.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if given tournaments have the same properties.</returns>
        private bool IsEqual(TournamentViewModel x, TournamentViewModel y)
        {
            if (AreSeasonListsEqual(x.SeasonsList, y.SeasonsList) &&
                x.Id == y.Id &&
                x.Description == y.Description &&
                x.Name == y.Description &&
                x.RegulationsLink == y.Description &&
                x.Season == y.Description &&
                x.Scheme == y.Scheme)
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
