namespace VolleyManagement.UI.Areas.WebAPI.ViewModels.Schedule
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents schedule grouped by rounds view model
    /// </summary>
    public class ScheduleByRoundViewModel
    {
        /// <summary>
        /// Gets or sets round in tournamnet
        /// </summary>
        public int Round { get; set; }

        /// <summary>
        /// Gets or sets collection of <see cref="ScheduleByDateInRoundViewModel"/>containing game results grouped by date
        /// </summary>
        public List<ScheduleByDateInRoundViewModel> ScheduleByDate { get; set; }
    }
}
