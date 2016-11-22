using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolleyManagement.UI.Areas.Mvc.ViewModels.Tournaments;

namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    /// <summary>
    /// Comparer for tournament objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class TournamentApplyViewModelComparer : IComparer<TournamentApplyViewModel>, IComparer
    {
        /// <summary>
        /// Compares two tournament objects.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of tournaments.</returns>
        public int Compare(TournamentApplyViewModel x, TournamentApplyViewModel y)
        {
            return AreEqual(x, y) ? 0 : 1;
        }

        /// <summary>
        /// Compares two tournament objects (non-generic implementation).
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of tournaments.</returns>
        public int Compare(object x, object y)
        {
            TournamentApplyViewModel firstTournament = x as TournamentApplyViewModel;
            TournamentApplyViewModel secondTournament = y as TournamentApplyViewModel;

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
        /// Finds out whether two tournament objects have the same properties.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if given tournaments have the same properties.</returns>
        private bool AreEqual(TournamentApplyViewModel x, TournamentApplyViewModel y)
        {
            var teamComparer = new TeamNameViewModelComparer();
            bool result = x.Id == y.Id
                   && x.TeamId == y.TeamId
                   && x.TournamentTitle == y.TournamentTitle;

            if (result)
            {
                foreach (var xTeam in x.Teams)
                {
                    bool teamFound = false;
                    foreach (var yTeam in y.Teams)
                    {
                        if (teamComparer.AreEqual(xTeam, yTeam))
                        {
                            teamFound = true;
                        }
                    }

                    if (!teamFound)
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
