namespace VolleyManagement.UnitTests.Services.PlayerService
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Domain.PlayersAggregate;

    /// <summary>
    /// Comparer for player objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class PlayerComparer : IComparer<Player>, IComparer
    {
        /// <summary>
        /// Compares two player objects.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of players.</returns>
        public int Compare(Player x, Player y)
        {
            return AreEqual(x, y) ? 0 : 1;
        }

        /// <summary>
        /// Compares two player objects (non-generic implementation).
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of players.</returns>
        public int Compare(object x, object y)
        {
            var firstPlayer = x as Player;
            var secondPlayer = y as Player;

            if (firstPlayer == null)
            {
                return -1;
            }
            else if (secondPlayer == null)
            {
                return 1;
            }

            return Compare(firstPlayer, secondPlayer);
        }

        /// <summary>
        /// Finds out whether two player objects have the same properties.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if given players have the same properties.</returns>
        public bool AreEqual(Player x, Player y)
        {
            return x.Id == y.Id &&
                x.FirstName == y.FirstName &&
                x.LastName == y.LastName &&
                x.BirthYear == y.BirthYear &&
                x.Height == y.Height &&
                x.Weight == y.Weight &&
                x.TeamId == y.TeamId;
        }
    }
}