namespace VolleyManagement.UnitTests.Comparers.Tournaments
{
    using System.Collections;
    using System.Collections.Generic;
    using VolleyManagement.Domain.Tournaments;

    /// <summary>
    /// Comparer for tournament objects.
    /// </summary>
    internal class TournamentComparer : IEqualityComparer<Tournament>
    {
        /// <summary>
        /// Finds out whether two tournament objects are equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if given tournaments are equal.</returns>
        public bool Equals(Tournament x, Tournament y)
        {
            if (object.ReferenceEquals(x, y))
            {
                return true;
            }

            return x != null && y != null && IsEqual(x, y);
        }

        /// <summary>
        /// Calculates tournament HashCode.
        /// </summary>
        /// <param name="obj">Tournament object.</param>
        /// <returns>Returns hash code for this instance.</returns>
        public int GetHashCode(Tournament obj)
        {
            int hashName = obj.Name.GetHashCode();
            int hashId = obj.Id.GetHashCode();
            return hashName ^ hashId;
        }

        /// <summary>
        /// Finds out whether two tournament objects have the same properties.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if given tournaments have the same properties.</returns>
        private bool IsEqual(Tournament x, Tournament y)
        {
            if (x.Description.Equals(y.Description) && x.Name.Equals(y.Name)
                && x.Id.Equals(y.Id) && x.RegulationsLink.Equals(y.RegulationsLink)
                && x.Scheme.Equals(y.Scheme) && x.Season.Equals(y.Season))
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
