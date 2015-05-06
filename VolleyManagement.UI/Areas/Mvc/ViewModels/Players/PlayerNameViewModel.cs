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

        #region Factory Methods
        
        /// <summary>
        /// Maps domain entity to presentation
        /// </summary>
        /// <param name="player"> Domain object </param>
        /// <returns> View model object </returns>
        public static PlayerNameViewModel Map(Player player)
        {
            PlayerNameViewModel playerNameViewModel = new PlayerNameViewModel()
            {
                Id = player.Id,
                FullName = GetFullName(player)
            };
            return playerNameViewModel;
        }

        /// <summary>
        /// Maps PlayerViewModel to PlayerNameViewModel
        /// </summary>
        /// <param name="player"> Domain object </param>
        /// <returns> View model object </returns>
        public static PlayerNameViewModel Map(PlayerViewModel player)
        {
            PlayerNameViewModel playerNameViewModel = new PlayerNameViewModel()
            {
                Id = player.Id,
                FullName = GetFullName(player)
            };
            return playerNameViewModel;
        }

        /// <summary>
        /// Maps presentation entity to domain
        /// </summary>
        /// <returns> Domain object </returns>
        public Player ToDomain()
        {
            return new Player { Id = this.Id };
        }
        
        #endregion
        
        private static string GetFullName(Player player)
        {
            return player.LastName + " " + player.FirstName;
        }

        private static string GetFullName(PlayerViewModel player)
        {
            return player.LastName + " " + player.FirstName;
        }
    }
}