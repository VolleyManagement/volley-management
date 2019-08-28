using System.Collections.Generic;

namespace VolleyManagement.UI.Areas.WebAPI.ViewModels.GameReports
{
    using WebApi.ViewModels.GameReports;
    public class DivisionStandingsViewModel : DivisionStandingsViewModelBase
    {
        public IList<StandingsEntryViewModel> StandingsTable { get; set; } = new List<StandingsEntryViewModel>();
    }
}
