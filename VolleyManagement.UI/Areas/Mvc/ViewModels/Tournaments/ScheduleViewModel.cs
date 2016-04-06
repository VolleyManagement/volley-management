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
        /// <summary>
        /// Gets or sets number of rounds in tournament
        /// </summary>
        public byte CountRound { get; set; }

        /// <summary>
        /// Gets or sets current rounds collection
        /// </summary>
        public Dictionary<byte, List<GameResultDto>> Rounds { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduleViewModel"/> class
        /// </summary>
        public ScheduleViewModel()
        {
            Rounds = new Dictionary<byte, List<GameResultDto>>();            
        }

        /// <summary>
        /// Gets or sets id of tournament
        /// </summary>
        public int tournamentId { get; set; }

        /// <summary>
        /// Gets or sets name of tournament
        /// </summary>
        public string tournamentName { get; set; }
    }
}