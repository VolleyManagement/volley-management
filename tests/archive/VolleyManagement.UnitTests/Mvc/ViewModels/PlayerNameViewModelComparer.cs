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
    internal class PlayerNameViewModelComparer : IComparer<PlayerNameViewModel>, IComparer, IEqualityComparer<PlayerNameViewModel>
    {
        /// <summary>
        /// Compares two players objects.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of players.</returns>
        public int Compare(PlayerNameViewModel x, PlayerNameViewModel y)
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
            var firstPlayer = x as PlayerNameViewModel;
            var secondPlayer = y as PlayerNameViewModel;

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

        public bool Equals(PlayerNameViewModel x, PlayerNameViewModel y)
        {
            return AreEqual(x, y);
        }

        public int GetHashCode(PlayerNameViewModel obj)
        {
            return obj.Id.GetHashCode();
        }

        /// <summary>
        /// Finds out whether two player objects have the same properties.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if given players have the same properties.</returns>
        internal bool AreEqual(PlayerNameViewModel x, PlayerNameViewModel y)
        {
            return x.Id == y.Id &&
                   x.FirstName == y.FirstName &&
                   x.LastName == y.LastName;
        }
    }
}
