namespace VolleyManagement.UI.Areas.Mvc.ViewModels.Tournaments
{
    using System.Collections.Generic;
    using VolleyManagement.Domain.GamesAggregate;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.GameResults;

    /// <summary>
    /// Represents tournaments schedule
    /// </summary>
    public class ScheduleViewModel
    {
        public ScheduleViewModel()
        {
            Rounds = new Dictionary<int, List<GameResultDto>>();            
        }

        /// <summary>
        /// Gets or sets id of tournament
        /// </summary>
        public int tournamentId { get; set; }

        /// <summary>
        /// Gets or sets name of tournament
        /// </summary>
        public string tournamentName { get; set; }

        /// <summary>
        /// Gets or sets number of rounds in tournament
        /// </summary>
        public int CountRound { get; set; }
        
        /// <summary>
        /// Gets or sets current rounds collection
        /// </summary>
        public Dictionary<int, List<GameResultDto>> Rounds { get; set; }
    }
}