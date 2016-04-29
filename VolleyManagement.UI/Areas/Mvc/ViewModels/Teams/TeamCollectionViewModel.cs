namespace VolleyManagement.UI.Areas.Mvc.ViewModels.Teams
{
    using System.Collections.Generic;
    using VolleyManagement.Contracts.Authorization;

    /// <summary>
    /// Represents team collections
    /// </summary>
    public class TeamCollectionViewModel
    {
        /// <summary>
        /// Gets or sets current team collection
        /// </summary>
        public IEnumerable<TeamViewModel> Teams { get; set; }

        /// <summary>
        /// Gets or sets instance of <see cref="AllowedOperations"/> create object
        /// </summary>
        public AllowedOperations Create { get; set; }

        /// <summary>
        /// Gets or sets instance of <see cref="AllowedOperations"/> Edit object
        /// </summary>
        public AllowedOperations Edit { get; set; }

        /// <summary>
        /// Gets or sets instance of <see cref="AllowedOperations"/> Delete object
        /// </summary>
        public AllowedOperations Delete { get; set; }
    }
}