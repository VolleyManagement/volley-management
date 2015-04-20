namespace VolleyManagement.Domain.Contributors
{
    using System;
    using VolleyManagement.Domain.Properties;

    /// <summary>
    /// Contributor domain class.
    /// </summary>
    public class Contributor
    {
        private string _firstName;
        private string _lastName;
        private int? _contributorTeamId;

        /// <summary>
        /// Gets or sets a value indicating where Id.
        /// </summary>
        /// <value>Id of player.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating where FirstName.
        /// </summary>
        /// <value>First name.</value>
        public string FirstName
        {
            get
            {
                return _firstName;
            }

            set
            {
                _firstName = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating where LastName.
        /// </summary>
        /// <value>Last name.</value>
        public string LastName
        {
            get
            {
                return _lastName;
            }

            set
            {
                _lastName = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating where ContributorTeamId.
        /// </summary>
        /// <value>Contributor Team Id.</value>
        public int? ContributorTeamId
        {
            get
            {
                return _contributorTeamId;
            }

            set
            {
                _contributorTeamId = value;
            }
        }
    }
}
