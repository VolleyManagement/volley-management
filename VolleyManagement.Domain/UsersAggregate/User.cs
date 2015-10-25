namespace VolleyManagement.Domain.UsersAggregate
{
    using System.Collections.Generic;

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
        /// External login information for the user
        /// </summary>
        public IEnumerable<LoginProviderInfo> LoginProviders { get; set; }
    }
}
