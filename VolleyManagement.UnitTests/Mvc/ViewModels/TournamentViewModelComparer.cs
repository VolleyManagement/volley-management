namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Tournaments;

    /// <summary>
    /// Comparer for tournament objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class TournamentViewModelComparer : IComparer<TournamentViewModel>, IComparer
    {
        /// <summary>
        /// Compares two tournament objects.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of tournaments.</returns>
        public int Compare(TournamentViewModel x, TournamentViewModel y)
        {
            return AreEqual(x, y) ? 0 : 1;
        }

        /// <summary>
        /// Compares two tournament objects (non-generic implementation).
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of tournaments.</returns>
        public int Compare(object x, object y)
        {
            TournamentViewModel firstTournament = x as TournamentViewModel;
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
        private bool AreEqual(TournamentViewModel x, TournamentViewModel y)
        {
            bool result = x.Id == y.Id
                && x.Name == y.Name
                && x.Description == y.Description
                && x.Season == y.Season
                && x.SeasonsList.SequenceEqual(y.SeasonsList)
                && x.Scheme == y.Scheme
                && x.RegulationsLink == y.RegulationsLink
                && x.IsTransferEnabled == y.IsTransferEnabled;

            if (result)
            {
                var divisionComparer = new DivisionViewModelComparer();

                foreach (var xDivision in x.Divisions)
                {
                    bool divisionFound = false;

                    foreach (var yDivision in y.Divisions)
                    {
                        if (divisionComparer.AreEqual(xDivision, yDivision))
                        {
                            divisionFound = true;
                        }
                    }

                    if (!divisionFound)
                    {
                        result = false;
                        break;
                    }
                }
            }

            return result;
        }
    }
}
