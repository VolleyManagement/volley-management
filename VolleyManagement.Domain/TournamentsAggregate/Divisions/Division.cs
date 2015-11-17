namespace VolleyManagement.Domain.TournamentsAggregate
{
    using System;
    using VolleyManagement.Domain.Properties;

    /// <summary>
    /// Division domain class.
    /// </summary>
    public class Division
    {
        private string _name;

        /// <summary>
        /// Gets or sets a value indicating where Id.
        /// </summary>
        /// <value>Id of division.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating where Name.
        /// </summary>
        /// <value>First name.</value>
        public string Name
        {
            get
            {
                return this._name;
            }

            set
            {
                if (DivisionValidation.ValidateName(value))
                {
                    throw new ArgumentException(Resources.ValidationDivisionName, "Name");
                }

                this._name = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating where Tournament.
        /// </summary>
        /// <value>The division tournament.</value>
        public int TournamentId { get; set; }
    }
}