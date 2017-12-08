namespace VolleyManagement.UI.Areas.WebAPI.ViewModels.Schedule
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents schedule view model
    /// </summary>
    public class ScheduleViewModel
    {
        /// <summary>
        /// Gets or sets collection of <see cref="Week"/>containing schedule grouped by weeks
        /// </summary>
        public List<Week> Schedule { get; set; }
    }
}
