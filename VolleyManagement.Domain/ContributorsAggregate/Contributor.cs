namespace VolleyManagement.Domain.ContributorsAggregate
{
    using System;
    using VolleyManagement.Domain.Properties;

    /// <summary>
    /// Contributor domain class.
    /// </summary>
    public class Contributor
    {
        /// <summary>
        /// Gets or sets a value indicating where Id.
        /// </summary>
        /// <value>Id of contributor.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating where FirstName.
        /// </summary>
        /// <value>First name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating where ContributorTeamId.
        /// </summary>
        /// <value>Contributor Team Id.</value>
        public int? ContributorTeamId { get; set; }
    }
}
