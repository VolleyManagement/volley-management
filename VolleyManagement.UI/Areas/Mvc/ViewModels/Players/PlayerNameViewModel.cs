namespace VolleyManagement.UI.Areas.Mvc.ViewModels.Players
{
    using System.ComponentModel.DataAnnotations;

    using VolleyManagement.Domain;
    using VolleyManagement.Domain.Players;
    using VolleyManagement.UI.App_GlobalResources;

    /// <summary>
    /// Represents player view model
    /// </summary>
    public class PlayerNameViewModel
    {
        /// <summary>
        /// Gets or sets the player Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the player name
        /// </summary>
        [Display(Name = "PlayerFullName", ResourceType = typeof(ViewModelResources))]
        public string FullName { get; set; }
    }
}