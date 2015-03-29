
namespace VolleyManagement.UI.Areas.Mvc.ViewModels.Players
{
    using System.ComponentModel.DataAnnotations;

    using VolleyManagement.Domain.Players;
    using VolleyManagement.UI.App_GlobalResources;
    using playerConst = VolleyManagement.Domain.Constants.Player;

    /// <summary>
    /// Represents player view model
    /// </summary>
    public class PlayerViewModel
    {
        /// <summary>
        /// Gets or sets the player Id
        /// </summary>
        public int Id {get; set;}

        /// <summary>
        /// Gets or sets the player first name
        /// </summary>
        [Display(Name = "PlayerFirstName", ResourceType = typeof(ViewModelResources))]
        [Required(ErrorMessageResourceName = "FieldRequired"
            , ErrorMessageResourceType = typeof(ViewModelResources))]
        [StringLength(playerConst.MAX_FIRST_NAME_LENGTH, ErrorMessageResourceName = "MaxLengthErrorMessage"
            , ErrorMessageResourceType = typeof(ViewModelResources))]
        [RegularExpression(@"\p{L}+$", ErrorMessageResourceName = "InvalidEntriesError"
            , ErrorMessageResourceType = typeof(ViewModelResources))]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the player last name
        /// </summary>
        [Display(Name = "PlayerLastName", ResourceType = typeof(ViewModelResources))]
        [Required(ErrorMessageResourceName = "FieldRequired"
            , ErrorMessageResourceType = typeof(ViewModelResources))]
        [StringLength(playerConst.MAX_LAST_NAME_LENGTH, ErrorMessageResourceName = "MaxLengthErrorMessage"
            , ErrorMessageResourceType = typeof(ViewModelResources))]
        [RegularExpression(@"\p{L}+$", ErrorMessageResourceName = "InvalidEntriesError"
            , ErrorMessageResourceType = typeof(ViewModelResources))]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the player photo path
        /// </summary>
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ViewModelResources))]
        public string Photo { get; set; }

        /// <summary>
        /// Gets or sets the player birth year
        /// </summary>
        [Range(playerConst.MIN_BIRTH_YEAR, playerConst.MAX_BIRTH_YEAR
            , ErrorMessageResourceName = "FieldRange", ErrorMessageResourceType = typeof(ViewModelResources))]
        public int BirthYear { get; set; }

        /// <summary>
        /// Gets or sets the player height
        /// </summary>
        [Range(playerConst.MIN_HEIGHT, playerConst.MAX_HEIGHT
            , ErrorMessageResourceName = "FieldRange", ErrorMessageResourceType = typeof(ViewModelResources))]
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the player weight
        /// </summary>
        [Range(playerConst.MIN_WEIGHT, playerConst.MAX_WEIGHT
            , ErrorMessageResourceName = "FieldRange", ErrorMessageResourceType = typeof(ViewModelResources))]
        public int Weight { get; set; }

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
                // TO DO: Photo = player.,
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
                // TO DO: Photo
                BirthYear = this.BirthYear,
                Height = this.Height,
                Weight = this.Weight
            };
        }
        #endregion
        
    }
}