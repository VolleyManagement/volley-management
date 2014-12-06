namespace VolleyManagement.UnitTests.Comparers.Tournaments
{
    using System.Collections;
    using System.Collections.Generic;
    using VolleyManagement.Domain.Tournaments;

    /// <summary>
    /// Comparer for tournament objects.
    /// </summary>
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
            if (x.Id == y.Id)
            {
                return 0;
            }

            if (x.Id < y.Id)
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
            Tournament secondTournament = y as Tournament;

            if (firstTournament == null || secondTournament == null)
            {
                return -1;
            }

            return Compare(firstTournament, secondTournament);
        }
    }
}
