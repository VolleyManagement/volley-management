namespace VolleyManagement.UI.Areas.Mvc.ViewModels.GameResults
{
    /// <summary>
    /// Represents the information which will use the game delete dialog
    /// </summary>
    public class GameDeleteResultViewModel
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