namespace VolleyManagement.UI.Areas.Admin.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents tournament request collection.
    /// </summary>
    public class TournamentRequestCollectionViewModel
    {
        /// <summary>
        /// Gets or sets tournament request collection.
        /// </summary>
        public IEnumerable<TournamentRequestViewModel> Requests { get; set; }
    }
}