namespace VolleyManagement.UI.Areas.Mvc.ViewModels.GameReports
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.GameReportsAggregate;

    /// <summary>
    /// View model for standings table of single division
    /// </summary>
    public class DivisionStandingsViewModel : DivisionStandingsBase
    {
        public IList<StandingsEntryViewModel> StandingsEntries { get; set; } = new List<StandingsEntryViewModel>();

        public static DivisionStandingsViewModel Map(StandingsDto standings)
        {
            return new DivisionStandingsViewModel {
                StandingsEntries = standings.Standings.Select(StandingsEntryViewModel.Map).ToList(),
                LastUpdateTime = standings.LastUpdateTime
            };
        }
    }
}
