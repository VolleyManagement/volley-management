namespace VolleyManagement.UI.Areas.Mvc.ViewModels.Tournaments
{
    using System.Collections.Generic;
    using VolleyManagement.Domain.Tournaments;

    /// <summary>
    /// Represents tournaments collections
    /// </summary>
    public class TournamentsCollectionsViewModel
    {
        /// <summary>
        /// Gets or sets current tournaments collection
        /// </summary>
        public ICollection<Tournament> CurrentTournaments { get; set; }

        /// <summary>
        /// Gets or sets expected tournaments collection
        /// </summary>
        public ICollection<Tournament> ExpectedTournaments { get; set; }
    }
}