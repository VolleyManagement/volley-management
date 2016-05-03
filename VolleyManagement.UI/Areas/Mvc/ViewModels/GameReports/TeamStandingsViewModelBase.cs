namespace VolleyManagement.UI.Areas.Mvc.ViewModels.GameReports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Represents an abstract class with data to be able set correct standings of teams in tournament.
    /// </summary>
    public abstract class TeamStandingsViewModelBase
    {
        /// <summary>
        /// Gets or sets the team's position.
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// Gets or sets the number of point for the team.
        /// </summary>
        public int Points { get; set; }

        /// <summary>
        /// Gets or sets the ratio of number of sets the team won to number of sets the team lost.
        /// </summary>
        public float? SetsRatio { get; set; }

        /// <summary>
        /// Gets or sets the ratio of number of balls the team won to number of balls the team lost.
        /// </summary>
        public float? BallsRatio { get; set; }

        /// <summary>
        /// Set positions for teams in tournament according their results
        /// </summary>
        /// <typeparam name="T">The element type of the list. Must be inheritor of <see cref="TeamStandingsViewModelBase"/></typeparam>
        /// <param name="entries">Collection of entries with team's tournament data</param>
        /// <returns>Collection of entries with team's tournament data with set position for every team</returns>        
        public static List<T> SetPositions<T>(List<T> entries) where T : TeamStandingsViewModelBase
        {
            for (int i = 0; i < entries.Count; i++)
            {
                if (i != 0)
                {
                    if (AreScoresCompletelyEqual(entries[i], entries[i - 1]))
                    {
                        entries[i].Position = entries[i - 1].Position;
                        continue;
                    }
                }

                entries[i].Position = i + 1;
            }

            return entries;
        }

        private static bool AreScoresCompletelyEqual(TeamStandingsViewModelBase entryA, TeamStandingsViewModelBase entryB)
        {
            return entryA.Points == entryB.Points
                && entryA.SetsRatio == entryB.SetsRatio
                && entryA.BallsRatio == entryB.BallsRatio;
        }
    }
}