namespace VolleyManagement.UI.Areas.WebAPI.ViewModels.Schedule
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents schedule grouped by weeks
    /// </summary>
    public class Week
    {
        /// <summary>
        /// Gets or sets collection of <see cref="ScheduleDay"/>containing schedule grouped by days
        /// </summary>
        public List<ScheduleDay> Days { get; set; }
    }
}
