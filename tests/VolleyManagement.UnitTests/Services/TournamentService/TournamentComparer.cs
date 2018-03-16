namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Domain.TournamentsAggregate;

    /// <summary>
    /// Comparer for tournament objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class TournamentComparer : IComparer<Tournament>, IComparer
    {
        /// <summary>
        /// Compares two tournament objects.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of tournaments.</returns>
        public int Compare(Tournament x, Tournament y)
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
            Tournament firstTournament = x as Tournament;
            Tournament secondTournament = y as Tournament;

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
        internal bool AreEqual(Tournament x, Tournament y)
        {
            return x.Id == y.Id
                && x.Name == y.Name
                && x.Description == y.Description
                && x.Season == y.Season
                && x.Scheme == y.Scheme
                && x.RegulationsLink == y.RegulationsLink
                && x.IsArchived == y.IsArchived;
        }
    }
}
