namespace VolleyManagement.UI.Areas.Mvc.ViewModels.Tournaments
{
    using System;
    using System.Collections.Generic;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.GameResults;

    /// <summary>
    /// Represents tournaments schedule
    /// </summary>
    public class ScheduleViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduleViewModel"/> class
        /// </summary>
        public ScheduleViewModel()
        {
            Rounds = new Dictionary<byte, List<GameResultViewModel>>();
        }

        /// <summary>
        /// Gets or sets number of rounds in tournament
        /// </summary>
        public byte NumberOfRounds { get; set; }

        /// <summary>
        /// Gets or sets current rounds collection
        /// </summary>
        public Dictionary<byte, List<GameResultViewModel>> Rounds { get; set; }

        /// <summary>
        /// Gets or sets id of tournament
        /// </summary>
        public int TournamentId { get; set; }

        /// <summary>
        /// Gets or sets name of tournament
        /// </summary>
        public string TournamentName { get; set; }

        /// <summary>
        /// Gets or sets names of rounds for playoff scheme
        /// </summary>
        public IEnumerable<string> RoundNames { get; set; }
    }
}