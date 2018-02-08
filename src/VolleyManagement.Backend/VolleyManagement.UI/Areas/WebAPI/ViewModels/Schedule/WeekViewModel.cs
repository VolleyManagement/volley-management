namespace VolleyManagement.UI.Areas.WebAPI.ViewModels.Schedule
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents schedule grouped by weeks
    /// </summary>
    public class WeekViewModel
    {
        /// <summary>
        /// Gets or sets collection of <see cref="ScheduleDayViewModel"/>containing schedule grouped by days
        /// </summary>
        public List<ScheduleDayViewModel> Days { get; set; }
    }
}
