namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using VolleyManagement.Domain.TeamsAggregate;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.GameResults;

    /// <summary>
    /// Comparer for game view model objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class GameViewModelComparer : IComparer<GameViewModel>, IComparer
    {
        /// <summary>
        /// Compares two game objects.
        /// </summary>
        /// <param name="x">The first object to compare</param>
        /// <param name="y">The second object to compare</param>
        /// <returns>A signed integer that indicates the relative values of games.</returns>
        public int Compare(GameViewModel x, GameViewModel y)
        {
            return AreEqual(x, y) ? 0 : 1;
        }

        /// <summary>
        /// Compares two games objects (non-generic implementation).
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of games.</returns>
        public int Compare(object x, object y)
        {
            var firstGame = x as GameViewModel;
            var secondGame = y as GameViewModel;

            if (firstGame == null)
            {
                return -1;
            }
            else if (secondGame == null)
            {
                return 1;
            }

            return Compare(firstGame, secondGame);
        }

        /// <summary>
        /// Finds out whether two game objects have the same properties.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if given games have the same properties.</returns>
        public bool AreEqual(GameViewModel x, GameViewModel y)
        {
            bool result = x.TournamentId == y.TournamentId &&
                          x.HomeTeamId == y.HomeTeamId &&
                          x.AwayTeamId == y.AwayTeamId &&
                          x.Round == y.Round &&
                          x.GameDate == y.GameDate;

            if (result && x.Teams != null)
            {
                result &= x.Teams.Select(team => team.Value).SequenceEqual(
                          y.Teams.Select(team => team.Value));
            }

            if (result && x.TeamsWithFreeDay != null)
            {
                result &= x.TeamsWithFreeDay.Select(team => team.Value).SequenceEqual(
                          y.TeamsWithFreeDay.Select(team => team.Value));
            }

            if (result && x.Rounds != null)
            {
                result &= (x.Rounds.Items as IEnumerable<int>).SequenceEqual(
                           y.Rounds.Items as IEnumerable<int>);
            }

            return result;
        }
    }
}
