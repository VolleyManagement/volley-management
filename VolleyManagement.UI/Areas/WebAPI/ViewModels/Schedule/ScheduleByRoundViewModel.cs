namespace VolleyManagement.UI.Areas.WebAPI.ViewModels.Schedule
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using VolleyManagement.UI.Areas.WebApi.ViewModels.Games;

    public class ScheduleByRoundViewModel
    {
        public int Round { get; set; }

        public List<GameViewModel> GameResults { get; set; }
    }
}
