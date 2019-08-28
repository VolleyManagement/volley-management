namespace VolleyManagement.UI.Areas.Admin.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents request collection.
    /// </summary>
    public class RequestCollectionViewModel
    {
        /// <summary>
        /// Gets or sets request collection.
        /// </summary>
        public IEnumerable<RequestViewModel> Requests { get; set; }
    }
}