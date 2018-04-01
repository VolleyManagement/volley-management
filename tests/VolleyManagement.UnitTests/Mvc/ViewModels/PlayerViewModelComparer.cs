namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using UI.Areas.Mvc.ViewModels.Players;

    /// <summary>
    /// Comparer for player objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class PlayerViewModelComparer : IComparer<PlayerViewModel>, IComparer
    {
        /// <summary>
        /// Compares two players objects.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of players.</returns>
        public int Compare(PlayerViewModel x, PlayerViewModel y)
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
            var firstPlayer = x as PlayerViewModel;
            var secondPlayer = y as PlayerViewModel;

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
        private bool AreEqual(PlayerViewModel x, PlayerViewModel y)
        {
            return x.Id == y.Id &&
                x.FirstName == y.FirstName &&
                x.LastName == y.LastName &&
                x.BirthYear == y.BirthYear &&
                x.Height == y.Height &&
                x.Weight == y.Weight;
        }
    }
}
