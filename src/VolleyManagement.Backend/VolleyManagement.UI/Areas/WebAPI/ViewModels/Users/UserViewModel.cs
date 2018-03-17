namespace VolleyManagement.UI.Areas.WebApi.ViewModels.Users
{
    using System.ComponentModel.DataAnnotations;

    using Domain.UsersAggregate;

    /// <summary>
    /// TournamentViewModel class.
    /// </summary>
    public class UserViewModel
    {
        /// <summary>
        /// Gets or sets value indicating where Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating where UserName.
        /// </summary>
        /// <value>User Login.</value>
        [Required]
        [StringLength(60, MinimumLength = 5)]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating where FullName.
        /// </summary>
        /// <value>User Full Name.</value>
        [StringLength(60)]
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating where Email.
        /// </summary>
        /// <value>User Email.</value>
        [Required]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets a value indicating where Password.
        /// </summary>
        /// <value>User Password.</value>
        [Required]
        [StringLength(20, MinimumLength = 6)]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets a value indicating where CellPhone.
        /// </summary>
        /// <value>User Cell Phone.</value>
        public string CellPhone { get; set; }

        #region Factory Methods

        /// <summary>
        /// Maps domain entity to presentation
        /// </summary>
        /// <param name="user"> Domain object </param>
        /// <returns> View model object </returns>
        public static UserViewModel Map(User user)
        {
            return new UserViewModel {
                Id = user.Id,
                UserName = user.UserName,
                Password = string.Empty,
                FullName = user.PersonName,
                CellPhone = user.PhoneNumber,
                Email = user.Email
            };
        }

        /// <summary>
        /// Maps presentation entity to domain
        /// </summary>
        /// <returns> Domain object </returns>
        public User ToDomain()
        {
            return new User {
                Id = Id,
                UserName = UserName,
                PhoneNumber = CellPhone,
                Email = Email
            };
        }

        #endregion
    }
}