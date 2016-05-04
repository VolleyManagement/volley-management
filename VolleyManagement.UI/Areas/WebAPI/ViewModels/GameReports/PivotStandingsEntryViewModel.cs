namespace VolleyManagement.UI.Areas.WebApi.ViewModels.GameReports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using VolleyManagement.Domain.GameReportsAggregate;

    /// <summary>
    /// Represents a data transfer object of team with total score and statistics values.
    /// </summary>
    public class PivotStandingsEntryViewModel
    {
        /// <summary>
        /// Gets or sets the team's identifier.
        /// </summary>
        public int TeamId { get; set; }

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
        /// Maps domain model of team standings to view model of team standings.
        /// </summary>
        /// <param name="team">Domain model of team standings.</param>
        /// <returns>View model of game result.</returns>
        internal static PivotStandingsEntryViewModel Map(TeamStandingsDto team)
        {
            return new PivotStandingsEntryViewModel
            {
                TeamId = team.TeamId,
                TeamName = team.TeamName,
                Points = team.Points,
                SetsRatio = team.SetsRatio
            };
        }
    }
}