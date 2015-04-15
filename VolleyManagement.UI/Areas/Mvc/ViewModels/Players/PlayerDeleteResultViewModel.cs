namespace VolleyManagement.UI.Areas.Mvc.ViewModels.Players
{
    /// <summary>
    /// Represents the information which will use the player delete dialog
    /// </summary>
    public class PlayerDeleteResultViewModel
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