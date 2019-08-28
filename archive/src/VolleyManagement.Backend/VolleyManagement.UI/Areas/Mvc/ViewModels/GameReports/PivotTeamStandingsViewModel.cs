namespace VolleyManagement.UI.Areas.Mvc.ViewModels.GameReports
{
    using System;
    using System.Globalization;
    using Domain.GameReportsAggregate;

    /// <summary>
    /// Represents a data transfer object of team with total score and statistics values.
    /// </summary>
    public class PivotTeamStandingsViewModel : TeamStandingsViewModelBase
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
        /// Gets the sets ratio of the team.
        /// </summary>
        public string SetsRatioText
        {
            get
            {
                return !Single.IsPositiveInfinity(SetsRatio.Value)
                    ? string.Format(
                        CultureInfo.CurrentUICulture,
                        "{0:0.###}",
                        SetsRatio)
                    : "MAX";
            }
        }

        /// <summary>
        /// Maps domain model of team standings to view model of team standings.
        /// </summary>
        /// <param name="team">Domain model of team standings.</param>
        /// <returns>View model of game result.</returns>
        internal static PivotTeamStandingsViewModel Map(TeamStandingsDto team)
        {
            return new PivotTeamStandingsViewModel {
                TeamId = team.TeamId,
                TeamName = team.TeamName,
                Points = team.Points,
                SetsRatio = team.SetsRatio,
                BallsRatio = team.BallsRatio
            };
        }
    }
}