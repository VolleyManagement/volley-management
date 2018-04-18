namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using UI.Areas.Mvc.ViewModels.Teams;

    /// <summary>
    /// Comparer for team objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class TeamViewModelComparer : IComparer<TeamViewModel>, IComparer
    {
        /// <summary>
        /// Compares two player objects.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of players.</returns>
        public int Compare(TeamViewModel x, TeamViewModel y)
        {
            return AreEqual(x, y) ? 0 : 1;
        }

        /// <summary>
        /// Compares two team objects (non-generic implementation).
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of teams.</returns>
        public int Compare(object x, object y)
        {
            var firstTeam = x as TeamViewModel;
            var secondTeam = y as TeamViewModel;

            if (firstTeam == null)
            {
                return -1;
            }
            else if (secondTeam == null)
            {
                return 1;
            }

            return Compare(firstTeam, secondTeam);
        }

        /// <summary>
        /// Finds out whether two team objects have the same properties.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if given team have the same properties.</returns>
        public bool AreEqual(TeamViewModel x, TeamViewModel y)
        {
            var playerComparer = new PlayerNameViewModelComparer();

            var result = x.Id == y.Id &&
                   x.Name == y.Name &&
                   x.Coach == y.Coach &&
                   x.Achievements == y.Achievements &&
                   playerComparer.AreEqual(x.Captain, y.Captain);

            if (result)
            {
                if ((x.Roster != null && y.Roster == null)
                    && (x.AddedPlayers != null && y.AddedPlayers == null)
                    && (x.DeletedPlayers != null && y.DeletedPlayers == null)
                    || (x.Roster == null && y.Roster != null)
                    || (x.Roster.Count != y.Roster.Count)
                    || (x.AddedPlayers == null && y.AddedPlayers != null)
                    || (x.AddedPlayers.Count != y.AddedPlayers.Count)
                    || (x.AddedPlayers == null && y.AddedPlayers != null)
                    || (x.DeletedPlayers.Count != y.DeletedPlayers.Count))
                {
                    result = false;
                }
            }

            if (result && x.Roster != null)
            {
                foreach (var xPlayer in x.Roster)
                {
                    var playerFound = false;
                    foreach (var yPlayer in y.Roster)
                    {
                        if (playerComparer.AreEqual(xPlayer, yPlayer))
                        {
                            playerFound = true;
                        }
                    }

                    if (!playerFound)
                    {
                        result = false;
                        break;
                    }
                }
            }
            if (result && x.AddedPlayers != null)
            {
                foreach (var xPlayer in x.AddedPlayers)
                {
                    var playerFound = false;
                    foreach (var yPlayer in y.AddedPlayers)
                    {
                        if (xPlayer.Equals(yPlayer))
                        {
                            playerFound = true;
                        }
                    }

                    if (!playerFound)
                    {
                        result = false;
                        break;
                    }
                }
            }

            if (result && x.DeletedPlayers != null)
            {
                foreach (var xPlayer in x.DeletedPlayers)
                {
                    var playerFound = false;
                    foreach (var yPlayer in y.DeletedPlayers)
                    {
                        if (xPlayer.Equals(yPlayer))
                        {
                            playerFound = true;
                        }
                    }

                    if (!playerFound)
                    {
                        result = false;
                        break;
                    }
                }
            }
            return result;
        }
    }
}