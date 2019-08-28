﻿namespace VolleyManagement.UI.Areas.Mvc.ViewModels.Tournaments
{
    using System.Collections.Generic;
    using Contracts.Authorization;
    using Domain.TournamentsAggregate;

    /// <summary>
    /// Represents tournaments collections
    /// </summary>
    public class TournamentsCollectionsViewModel
    {
        /// <summary>
        /// Gets or sets current tournaments collection
        /// </summary>
        public IEnumerable<Tournament> CurrentTournaments { get; set; }

        /// <summary>
        /// Gets or sets expected tournaments collection
        /// </summary>
        public IEnumerable<Tournament> UpcomingTournaments { get; set; }

        /// <summary>
        /// Gets or sets instance of <see cref="AllowedOperations"/> object
        /// </summary>
        public AllowedOperations Authorization { get; set; }
    }
}