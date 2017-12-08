namespace VolleyManagement.UI.Areas.WebAPI.ViewModels.Schedule
{
    using System;
    using System.Collections.Generic;
    using VolleyManagement.UI.Areas.WebApi.ViewModels.Games;

    /// <summary>
    /// Represents schedule by day
    /// </summary>
    public class ScheduleDay
    {
        /// <summary>
        /// Gets or sets <see cref="DateTime"/>containing date
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets collection of <see cref="GameViewModel"/>containing games played in this day
        /// </summary>
        public List<GameViewModel> Games { get; set; }

        /// <summary>
        /// Gets or sets collection of <see cref="DivisionTitle"/>containing divisions played in this day info
        /// </summary>
        public List<DivisionTitle> Divisions{ get; set; }
    }
}
