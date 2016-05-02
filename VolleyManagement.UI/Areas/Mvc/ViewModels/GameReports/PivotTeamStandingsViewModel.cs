﻿namespace VolleyManagement.UI.Areas.Mvc.ViewModels.GameReports
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using VolleyManagement.Domain.GameReportsAggregate;

    /// <summary>
    /// Represents a data transfer object of team with total score and statistics values.
    /// </summary>
    public class PivotTeamStandingsViewModel
    {
        /// <summary>
        /// Gets or sets the team's identifier.
        /// </summary>
        public int TeamId { get; set; }

        /// <summary>
        /// Gets or sets the team's position.
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// Gets or sets the team's name.
        /// </summary>
        public string TeamName { get; set; }

        /// <summary>
        /// Gets or sets the number of point for the team.
        /// </summary>
        public int Points { get; set; }

        /// <summary>
        /// Gets or sets the ratio of number of sets the team won to number of sets the team lost.
        /// </summary>
        public float? SetsRatio { get; set; }

        /// <summary>
        /// Gets the sets ratio of the team.
        /// </summary>
        public string SetsRatioText
        {
            get
            {
                return SetsRatio != float.PositiveInfinity ?
                    string.Format(
                    CultureInfo.CurrentUICulture,
                    "{0:0.###}",
                    SetsRatio) : "MAX";
            }
        }

        /// <summary>
        /// Gets or sets the ratio of number of balls the team won to number of balls the team lost.
        /// </summary>
        public float? BallsRatio { get; set; }

        /// <summary>
        /// Maps domain model of team standings to view model of team standings.
        /// </summary>
        /// <param name="team">Domain model of team standings.</param>
        /// <returns>View model of game result.</returns>
        internal static PivotTeamStandingsViewModel Map(TeamStandingsDto team)
        {
            return new PivotTeamStandingsViewModel
            {
                TeamId = team.TeamId,
                TeamName = team.TeamName,
                Points = team.Points,
                SetsRatio = team.SetsRatio,
                BallsRatio = team.BallsRatio
            };
        }

        /// <summary>
        /// Set positions for teams in tournament according their results
        /// </summary>
        /// <param name="entries">Collection of entries with team's tournament data</param>
        /// <returns>Collection of entries with team's tournament data with set position for every team</returns>
        internal static List<PivotTeamStandingsViewModel> SetPositions(List<PivotTeamStandingsViewModel> entries)
        {
            for (int i = 0; i < entries.Count; i++)
            {
                if (i != 0)
                {
                    if (entries[i].Points == entries[i - 1].Points
                        && entries[i].SetsRatio == entries[i - 1].SetsRatio
                        && entries[i].BallsRatio == entries[i - 1].BallsRatio)
                    {
                        entries[i].Position = entries[i - 1].Position;
                    }
                    else
                    {
                        entries[i].Position = i + 1;
                    }
                }
                else
                {
                    entries[i].Position = i + 1;
                }
            }

            return entries;
        }
    }
}