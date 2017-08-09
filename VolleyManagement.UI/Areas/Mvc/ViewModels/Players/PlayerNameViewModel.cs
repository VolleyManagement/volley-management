namespace VolleyManagement.UI.Areas.Mvc.ViewModels.Players
{
    using System.ComponentModel.DataAnnotations;

    using Domain.PlayersAggregate;
    using Resources.UI;

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
        [RegularExpression(@"(^(([a-zA-Zа-яА-ЯёЁіІїЇєЄ]+[\s\-\'][a-zA-Zа-яА-ЯёЁіІїЇєЄ]{2,}))+)$", ErrorMessage = "Player must have First and Second Name")]
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
            var splitPlayerName = FullName.Split(' ');
            return new Player { Id = Id, FirstName = splitPlayerName[0], LastName = splitPlayerName[1] };
        }

        #endregion

        private static string GetFullName(Player player)
        {
            return player.FirstName + " " + player.LastName;
        }

        private static string GetFullName(PlayerViewModel player)
        {
            return player.FirstName + " " + player.LastName;
        }
    }
}