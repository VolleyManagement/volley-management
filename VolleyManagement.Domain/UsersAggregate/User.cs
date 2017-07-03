namespace VolleyManagement.Domain.UsersAggregate
{
    using System.Collections.Generic;
    using PlayersAggregate;
    using RolesAggregate;

    /// <summary>
    /// Represents User of the Volley Management system
    /// </summary>
    public class User
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        public User()
        {
            LoginProviders = new List<LoginProviderInfo>();
        }

        /// <summary>
        /// Gets or sets user Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets user Name
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets user Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets user's full name
        /// </summary>
        public string PersonName { get; set; }

        /// <summary>
        /// Gets or sets phone number for the user
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether it is true if user account is blocked.
        /// </summary>
        public bool IsBlocked { get; set; }

        /// <summary>
        /// Gets or sets player Id
        /// </summary>
        public int? PlayerId { get; set; }

        /// <summary>
        /// Gets or sets player instance
        /// </summary>
        public Player Player { get; set; }

        /// <summary>
        /// Gets or sets external login information for the user
        /// </summary>
        public IEnumerable<LoginProviderInfo> LoginProviders { get; set; }

        /// <summary>
        /// Gets or sets external information of the user roles
        /// </summary>
        public IEnumerable<Role> Roles { get; set; }
    }
}
