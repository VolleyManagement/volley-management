namespace VolleyManagement.UI.Areas.Mvc.ViewModels.Teams
{
    /// <summary>
    /// Represents the information which will use for the team delete dialog
    /// </summary>
    public class TeamDeleteResultViewModel
    {
        /// <summary>
        /// Gets or sets message to user
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the result of delete
        /// </summary>
        public bool HasDeleted { get; set; }
    }
}