namespace VolleyManagement.UnitTests.Services.TournamentRequestService
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Domain.TournamentRequestAggregate;

    /// <summary>
    /// Comparer for tournament request objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class TournamentRequestComparer : IComparer<TournamentRequest>, IComparer, IEqualityComparer<TournamentRequest>
    {
        /// <summary>
        /// Compares two tournament requests objects (non-generic implementation).
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of requests.</returns>
        public int Compare(object x, object y)
        {
            var firstTournamentRequest = x as TournamentRequest;
            var secondTournamentRequest = y as TournamentRequest;

            if (firstTournamentRequest == null)
            {
                return -1;
            }
            else if (secondTournamentRequest == null)
            {
                return 1;
            }

            return Compare(firstTournamentRequest, secondTournamentRequest);
        }

        /// <summary>
        /// Compares two tournament request objects.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of requests.</returns>
        public int Compare(TournamentRequest x, TournamentRequest y)
        {
            return AreEquals(x, y) ? 0 : 1;
        }

        /// <summary>
        /// Finds out whether two request objects have the same properties.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if given requests have the same properties.</returns>
        public bool AreEquals(TournamentRequest x, TournamentRequest y)
        {
            return x.Id == y.Id &&
                x.TeamId == y.TeamId &&
                x.TournamentId == y.TournamentId &&
                x.UserId == y.UserId;
        }

        public bool Equals(TournamentRequest x, TournamentRequest y)
        {
            return AreEquals(x, y);
        }

        public int GetHashCode(TournamentRequest obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
