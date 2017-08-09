namespace VolleyManagement.Domain.TournamentsAggregate
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Properties;

    /// <summary>
    /// Division domain class.
    /// </summary>
    public class Division
    {
        private string _name;

        /// <summary>
        /// Initializes a new instance of the <see cref="Division"/> class.
        /// </summary>
        public Division()
        {
            Groups = new List<Group>();
        }

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
                return _name;
            }

            set
            {
                if (DivisionValidation.ValidateName(value))
                {
                    throw new ArgumentException(Resources.ValidationDivisionName, "Name");
                }

                _name = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether division is empty.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return Groups.All(g => g.IsEmpty);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating where Tournament.
        /// </summary>
        /// <value>The division tournament.</value>
        public int TournamentId { get; set; }

        /// <summary>
        /// Gets or sets groups of the division.
        /// </summary>
        public List<Group> Groups { get; set; }
    }
}