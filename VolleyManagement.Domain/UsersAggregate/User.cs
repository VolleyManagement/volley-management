namespace VolleyManagement.Domain.UsersAggregate
{
    using System.Collections.Generic;
    using VolleyManagement.Domain.PlayersAggregate;
    using VolleyManagement.Domain.RolesAggregate;

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
        /// User Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// User Name
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// User Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// User's full name
        /// </summary>
        public string PersonName { get; set; }

        /// <summary>
        /// Phone number for the user
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Player Id
        /// </summary>
        public int PlayerId { get; set; }

        /// <summary>
        /// Player instance
        /// </summary>
        public Player Player { get; set; }

        /// <summary>
        /// External login information for the user
        /// </summary>
        public IEnumerable<LoginProviderInfo> LoginProviders { get; set; }

        /// <summary>
        /// External information of the user roles
        /// </summary>
        public IEnumerable<Role> Roles { get; set; }
    }
}
