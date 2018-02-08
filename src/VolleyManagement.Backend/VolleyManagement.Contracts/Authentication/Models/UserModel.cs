namespace VolleyManagement.Contracts.Authentication.Models
{
    using System.Collections.Generic;

    using Microsoft.AspNet.Identity;

    /// <summary>
    /// Represents a user
    /// </summary>
    public class UserModel : IUser<int>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserModel"/> class.
        /// </summary>
        public UserModel()
        {
            Logins = new List<LoginProviderModel>();
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the person name.
        /// </summary>
        public string PersonName { get; set; }

        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the Login Provider info list.
        /// </summary>
        public List<LoginProviderModel> Logins { get; set; }
    }
}