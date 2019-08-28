namespace VolleyManagement.UI.Areas.Mvc.ViewModels.Users
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Contracts.Authentication.Models;
    using Domain.UsersAggregate;
    using Resources.UI;

    /// <summary>
    /// UserViewModel for Create and Edit actions
    /// </summary>
    public class UserEditViewModel
    {
        /// <summary>
        /// Gets or sets a value indicating where Id.
        /// </summary>
        /// <value>Id of user.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating where Full Name.
        /// </summary>
        /// <value>Full name of user.</value>
        [Display(Name = "UserFullName", ResourceType = typeof(ViewModelResources))]
        [StringLength(60)]
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating where Telephone.
        /// </summary>
        /// <value>Telephone of user.</value>
        [Display(Name = "UserCellPhone", ResourceType = typeof(ViewModelResources))]
        [StringLength(20)]
        public string CellPhone { get; set; }

        /// <summary>
        /// Gets or sets a value indicating email of user.
        /// </summary>
        /// <value>Email of user.</value>
        [Display(Name = "UserEmail", ResourceType = typeof(ViewModelResources))]
        [Required(
            ErrorMessageResourceName = "FieldRequired",
            ErrorMessageResourceType = typeof(ViewModelResources))]
        [StringLength(80)]
        public string Email { get; set; }

        #region Factory Methods

        /// <summary>
        /// Maps domain entity to presentation
        /// </summary>
        /// <param name="user"> Domain object </param>
        /// <returns> View model object </returns>
        public static UserEditViewModel Map(User user)
        {
            return new UserEditViewModel {
                Id = user.Id,
                FullName = user.PersonName,
                CellPhone = user.PhoneNumber,
                Email = user.Email
            };
        }

        /// <summary>
        /// Maps contract entity to presentation
        /// </summary>
        /// <param name="user"> Contract object </param>
        /// <returns> View model object </returns>
        public static UserEditViewModel Map(UserModel user)
        {
            return new UserEditViewModel {
                Id = user.Id,
                FullName = user.PersonName,
                CellPhone = user.PhoneNumber,
                Email = user.Email
            };
        }
        #endregion
    }
}