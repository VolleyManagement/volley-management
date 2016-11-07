namespace VolleyManagement.Domain.RequestsAggregate
{
    using System;
    using VolleyManagement.Domain.Properties;

    /// <summary>
    /// Request domain class.
    /// </summary>
    public class Request
    {
        /// <summary>
        /// Gets or sets a value indicating where Id.
        /// </summary>
        /// <value>Id of request.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets user's Id.
        /// </summary>
        /// <value>Id of user.</value>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets player's Id.
        /// </summary>
        /// <value>Id of player.</value>
        public int PlayerId { get; set; }
    }
}
