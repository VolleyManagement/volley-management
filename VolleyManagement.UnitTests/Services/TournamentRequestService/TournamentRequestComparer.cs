namespace VolleyManagement.UnitTests.Services.TournamentRequestService
{
    using System.Diagnostics.CodeAnalysis;
    using VolleyManagement.Domain.TournamentRequestAggregate;

    /// <summary>
    /// Comparer for tournament requests objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class TournamentRequestComparer
    {
        /// <summary>
        /// Compares two tournament request objects.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of tournaments request.</returns>
        public int Compare(TournamentRequest x, TournamentRequest y)
        {
            return AreEqual(x, y) ? 0 : 1;
        }

        /// <summary>
        /// Compares two tournament request objects (non-generic implementation).
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of tournaments request.</returns>
        public int Compare(object x, object y)
        {
            TournamentRequest firstTournament = x as TournamentRequest;
            TournamentRequest secondTournament = y as TournamentRequest;

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
        /// Finds out whether two tournament request objects have the same properties.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if given tournaments request have the same properties.</returns>
        internal bool AreEqual(TournamentRequest x, TournamentRequest y)
        {
            return x.Id == y.Id
                   && x.UserId == y.UserId
                   && x.TeamId == y.TeamId
                   && x.TournamentId == y.TournamentId;
        }
    }
}
