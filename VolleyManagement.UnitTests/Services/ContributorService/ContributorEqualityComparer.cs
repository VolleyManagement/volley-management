namespace VolleyManagement.UnitTests.Services.ContributorService
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using VolleyManagement.Domain.ContributorsAggregate;

    /// <summary>
    /// Equality comparer for contributor objects
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class ContributorEqualityComparer : IEqualityComparer<Contributor>
    {
        /// <summary>
        /// Check if objects are equal
        /// </summary>
        /// <param name="x">first object to compare</param>
        /// <param name="y">second object to compare</param>
        /// <returns>True if objects are equal</returns>
        public bool Equals(Contributor x, Contributor y)
        {
            return x != null &&
                   y != null &&
                   x.Id == y.Id &&
                   x.Name == y.Name &&
                   x.ContributorTeamId == y.ContributorTeamId;
        }

        /// <summary>
        /// Get hash code for the contributor object
        /// </summary>
        /// <param name="obj">Contributor object</param>
        /// <returns>Contributor's Id as hash code</returns>
        public int GetHashCode(Contributor obj)
        {
            return obj.Id;
        }
    }
}
