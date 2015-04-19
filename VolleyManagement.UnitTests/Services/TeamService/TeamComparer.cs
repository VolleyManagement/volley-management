namespace VolleyManagement.UnitTests.Services.TeamService
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using PlayerService;
    using VolleyManagement.Domain.Players;
    using VolleyManagement.Domain.Teams;

    /// <summary>
    /// Comparer for team objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class TeamComparer : IComparer<Team>, IComparer
    {
        private PlayerComparer _playerComparer = new PlayerComparer();

        /// <summary>
        /// Compares two player objects.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of players.</returns>
        public int Compare(Team x, Team y)
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
            Team firstTeam = x as Team;
            Team secondTeam = y as Team;

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
        public bool AreEqual(Team x, Team y)
        {
            // Check primitive fields
            bool fieldsCheck = x.Id == y.Id &&
                x.Name == y.Name &&
                x.Coach == y.Coach &&
                x.Achievements == y.Achievements;

            // Check captains
            bool captainsCheck = _playerComparer.AreEqual(x.Captain, y.Captain);

            // Check rosters
            bool rostersCheck = true;
            if (x.Roster != null && y.Roster != null)
            {
                var rostersTeamX = x.Roster.ToList<Player>();
                var rostersTeamY = y.Roster.ToList<Player>();

                rostersCheck = rostersTeamX.Count == rostersTeamY.Count;
                if (rostersCheck)
                {
                    for (int i = 0; i < rostersTeamX.Count; i++)
                    {
                        if (!_playerComparer.AreEqual(rostersTeamX[i], rostersTeamY[i]))
                        {
                            rostersCheck = false;
                            break;
                        }
                    }
                }
            }
            else if ((x.Roster != null && y.Roster == null) ||
                     (x.Roster == null && y.Roster != null))
            {
                rostersCheck = false;
            }

            return fieldsCheck && captainsCheck && rostersCheck;
        }
    }
}