using System.Collections.Generic;

namespace VolleyManagement.UI.Areas.WebAPI.ViewModels.Schedule
{
    /// <summary>
    /// Represents info about divisions
    /// </summary>
    public class DivisionTitleViewModel
    {
        /// <summary>
        /// Gets or sets division id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets division name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets collection of round numbers
        /// </summary>
        public List<string> Rounds { get; set; }
    }
}
