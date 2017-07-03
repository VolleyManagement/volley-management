namespace VolleyManagement.UI.Areas.Mvc.ViewModels.Tournaments
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    /// <summary>
    /// Represents the information which will use the team delete dialog
    /// </summary>
    public class TeamDeleteFromTournamentViewModel
    {
        /// <summary>
        /// Gets or sets message to user
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets the result of delete
        /// </summary>
        public bool HasDeleted { get; set; }
    }
}