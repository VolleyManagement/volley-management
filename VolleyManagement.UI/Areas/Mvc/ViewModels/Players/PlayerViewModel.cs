namespace VolleyManagement.UI.Areas.Mvc.ViewModels.Players
{
    using System.ComponentModel.DataAnnotations;

    using VolleyManagement.Domain;
    using VolleyManagement.Domain.PlayersAggregate;
    using VolleyManagement.UI.App_GlobalResources;

    /// <summary>
    /// Represents player view model
    /// </summary>
    public class PlayerViewModel
    {
        /// <summary>
        /// Gets or sets the player Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the player first name
        /// </summary>
        [Display(Name = "PlayerFirstName", ResourceType = typeof(ViewModelResources))]
        [Required(ErrorMessageResourceName = "PlayerFirstNameRequired",
            ErrorMessageResourceType = typeof(ViewModelResources))]
        [StringLength(Constants.Player.MAX_FIRST_NAME_LENGTH, ErrorMessageResourceName = "PlayerMaxLengthErrorMessage",
            ErrorMessageResourceType = typeof(ViewModelResources))]
        [RegularExpression(ViewModelConstants.NAME_VALIDATION_REGEX, ErrorMessageResourceName = "PlayerFirstNameInvalidEntriesError",
            ErrorMessageResourceType = typeof(ViewModelResources))]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the player last name
        /// </summary>
        [Display(Name = "PlayerLastName", ResourceType = typeof(ViewModelResources))]
        [Required(ErrorMessageResourceName = "PlayerLastNameRequired",
            ErrorMessageResourceType = typeof(ViewModelResources))]
        [StringLength(Constants.Player.MAX_LAST_NAME_LENGTH, ErrorMessageResourceName = "PlayerMaxLengthErrorMessage",
            ErrorMessageResourceType = typeof(ViewModelResources))]
        [RegularExpression(ViewModelConstants.NAME_VALIDATION_REGEX, ErrorMessageResourceName = "PlayerLastNameInvalidEntriesError",
            ErrorMessageResourceType = typeof(ViewModelResources))]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the player birth year
        /// </summary>
        [Display(Name = "PlayerBirthYear", ResourceType = typeof(ViewModelResources))]
        [Range(Constants.Player.MIN_BIRTH_YEAR, Constants.Player.MAX_BIRTH_YEAR,
            ErrorMessageResourceName = "PlayerBirthFieldRange", ErrorMessageResourceType = typeof(ViewModelResources))]
        public short? BirthYear { get; set; }

        /// <summary>
        /// Gets or sets the player height
        /// </summary>
        [Display(Name = "PlayerHeight", ResourceType = typeof(ViewModelResources))]
        [Range(Constants.Player.MIN_HEIGHT, Constants.Player.MAX_HEIGHT,
            ErrorMessageResourceName = "PlayerHeightFieldRange", ErrorMessageResourceType = typeof(ViewModelResources))]
        public short? Height { get; set; }

        /// <summary>
        /// Gets or sets the player weight
        /// </summary>
        [Display(Name = "PlayerWeight", ResourceType = typeof(ViewModelResources))]
        [Range(Constants.Player.MIN_WEIGHT, Constants.Player.MAX_WEIGHT,
            ErrorMessageResourceName = "PlayerWeightFieldRange", ErrorMessageResourceType = typeof(ViewModelResources))]
        public short? Weight { get; set; }

        /// <summary>
        /// Gets or sets the player team
        /// </summary>
        public int? TeamId { get; set; }

        #region Factory Methods

        /// <summary>
        /// Maps domain entity to presentation
        /// </summary>
        /// <param name="player"> Domain object </param>
        /// <returns> View model object </returns>
        public static PlayerViewModel Map(Player player)
        {
            var playerViewModel = new PlayerViewModel
            {
                Id = player.Id,
                FirstName = player.FirstName,
                LastName = player.LastName,
                BirthYear = player.BirthYear,
                Height = player.Height,
                Weight = player.Weight
            };

            return playerViewModel;
        }

        /// <summary>
        /// Maps presentation entity to domain
        /// </summary>
        /// <returns> Domain object </returns>
        public Player ToDomain()
        {
            return new Player
            {
                Id = this.Id,
                FirstName = this.FirstName,
                LastName = this.LastName,
                BirthYear = this.BirthYear,
                Height = this.Height,
                Weight = this.Weight,
                TeamId = this.TeamId
            };
        }
        #endregion
    }
}