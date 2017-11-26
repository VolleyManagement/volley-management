namespace VolleyManagement.UI.Areas.WebAPI.ViewModels.Schedule
{
    using System;
    using System.Collections.Generic;
    using VolleyManagement.UI.Areas.WebApi.ViewModels.Games;

    /// <summary>
    /// Represents schedule grouped by date view model
    /// </summary>
    public class ScheduleByDateInRoundViewModel
    {
        /// <summary>
        /// Gets or sets game date in tournamnet
        /// </summary>
        public DateTime GameDate { get; set; }

        /// <summary>
        /// Gets or sets collection of <see cref="GameViewModel"/>containing game results
        /// </summary>
        public List<GameViewModel> GameResults { get; set; }
    }
}
