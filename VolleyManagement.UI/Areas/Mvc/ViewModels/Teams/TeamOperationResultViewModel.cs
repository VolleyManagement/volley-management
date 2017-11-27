namespace VolleyManagement.UI.Areas.Mvc.ViewModels.Teams
{
    /// <summary>
    /// Represents the information which will use for the team delete dialog
    /// </summary>
    public class TeamOperationResultViewModel
    {
        /// <summary>
        /// Gets or sets message to user
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets the result of delete
        /// </summary>
        public bool OperationSuccessful { get; set; }

        /// <summary>
        /// Gets or sets the information about internal errors during the operation
        /// </summary>
        public string InternalErrorInformation { get; set; }
    }
}