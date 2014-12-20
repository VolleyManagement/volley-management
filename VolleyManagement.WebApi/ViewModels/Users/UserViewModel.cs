namespace VolleyManagement.WebApi.ViewModels.Users
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using VolleyManagement.Domain.Users;

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
    }
}