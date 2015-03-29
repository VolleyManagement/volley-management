
namespace VolleyManagement.UI.Areas.Mvc.ViewModels.Players
{
    using System.ComponentModel.DataAnnotations;
    using VolleyManagement.UI.App_GlobalResources;

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
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ViewModelResources))]
        [StringLength(60, ErrorMessageResourceName = "MaxLengthErrorMessage", ErrorMessageResourceType = typeof(ViewModelResources))]
        [RegularExpression(@"\p{L}+$", ErrorMessageResourceName = "InvalidEntriesError", ErrorMessageResourceType = typeof(ViewModelResources))]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the player last name
        /// </summary>
        [Display(Name = "PlayerLastName", ResourceType = typeof(ViewModelResources))]
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ViewModelResources))]
        [StringLength(60, ErrorMessageResourceName = "MaxLengthErrorMessage", ErrorMessageResourceType = typeof(ViewModelResources))]
        [RegularExpression(@"\p{L}+$", ErrorMessageResourceName = "InvalidEntriesError", ErrorMessageResourceType = typeof(ViewModelResources))]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the player photo path
        /// </summary>
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ViewModelResources))]
        public string Photo { get; set; }

        /// <summary>
        /// Gets or sets the player birth year
        /// </summary>
        [Range(1900, 2100, ErrorMessageResourceName = "FieldRange", ErrorMessageResourceType = typeof(ViewModelResources))]
        public int BirthYear { get; set; }

        /// <summary>
        /// Gets or sets the player height
        /// </summary>
        [Range(10, 250, ErrorMessageResourceName = "FieldRange", ErrorMessageResourceType = typeof(ViewModelResources))]
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the player weight
        /// </summary>
        [Range(10, 250, ErrorMessageResourceName = "FieldRange", ErrorMessageResourceType = typeof(ViewModelResources))]
        public int Weight { get; set; }

        /*
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
                Id = ,
                FirstName = ,
                LastName = ,
                Photo = ,
                BirthYear = ,
                Height = ,
                Weight = 
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
            };
        }
        #endregion
         */
        
    }
}