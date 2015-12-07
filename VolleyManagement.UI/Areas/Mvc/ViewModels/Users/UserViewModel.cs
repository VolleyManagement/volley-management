namespace VolleyManagement.UI.Areas.Mvc.ViewModels.Users
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Contracts.Authentication.Models;
    using VolleyManagement.Domain.UsersAggregate;
    using VolleyManagement.UI.App_GlobalResources;

    /// <summary>
    /// UserViewModel for Create and Edit actions
    /// </summary>
    public class UserViewModel
    {
        /// <summary>
        /// Gets or sets a value indicating where Id.
        /// </summary>
        /// <value>Id of user.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating where UserName.
        /// </summary>
        /// <value>User login.</value>
        [Display(Name = "UserName", ResourceType = typeof(ViewModelResources))]
        [Required(ErrorMessageResourceName = "FieldRequired",
            ErrorMessageResourceType = typeof(ViewModelResources))]
        [StringLength(20)]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating where Password.
        /// </summary>
        /// <value>Password of user.</value>
        [Display(Name = "UserPassword", ResourceType = typeof(ViewModelResources))]
        [Required(ErrorMessageResourceName = "FieldRequired",
            ErrorMessageResourceType = typeof(ViewModelResources))]
        [StringLength(20, MinimumLength = 6)]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets a value indicating where ConfirmPassword.
        /// </summary>
        /// <value>ConfirmPassword of user.</value>
        [Display(Name = "ConfirmUserPassword", ResourceType = typeof(ViewModelResources))]
        [Required(ErrorMessageResourceName = "FieldRequired",
            ErrorMessageResourceType = typeof(ViewModelResources))]
        [Compare("Password", ErrorMessageResourceName = "PasswordDidNotMatch",
            ErrorMessageResourceType = typeof(ViewModelResources))]
        [StringLength(20, MinimumLength = 6)]
        public string ConfirmPassword { get; set; }

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
        [Required(ErrorMessageResourceName = "FieldRequired",
            ErrorMessageResourceType = typeof(ViewModelResources))]
        [StringLength(80)]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the Login Provider info list.
        /// </summary>
        [Display(Name = "LoginProviders", ResourceType = typeof(ViewModelResources))]
        public List<string> LoginProviders { get; set; }

        #region Factory Methods

        /// <summary>
        /// Maps domain entity to presentation
        /// </summary>
        /// <param name="user"> Domain object </param>
        /// <returns> View model object </returns>
        public static UserViewModel Map(User user)
        {
            return new UserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Password = string.Empty,
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
        public static UserViewModel Map(VolleyManagement.Contracts.Authentication.Models.UserModel user)
        {
            return new UserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Password = string.Empty,
                FullName = user.PersonName,
                CellPhone = user.PhoneNumber,
                Email = user.Email,
                LoginProviders = user.Logins.Select(p => p.LoginProvider).ToList()
            };
        }

        /// <summary>
        /// Maps presentation entity to domain
        /// </summary>
        /// <returns> Domain object </returns>
        public User ToDomain()
        {
            return new User
            {
                Id = this.Id,
                UserName = this.UserName,
                PhoneNumber = this.CellPhone,
                Email = this.Email
            };
        }

        #endregion
    }
}