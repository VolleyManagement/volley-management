namespace VolleyManagement.Domain.TournamentsAggregate
{
    using System;
    using Properties;

    /// <summary>
    /// Group domain class.
    /// </summary>
    public class Group
    {
        private string _name;

        /// <summary>
        /// Gets or sets the identifier of the group.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the group.
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                if (!GroupValidation.ValidateName(value))
                {
                    throw new ArgumentException(Resources.ValidationGroupName, "Name");
                }

                _name = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether group is empty.
        /// </summary>
        public bool IsEmpty { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the division where group belongs.
        /// </summary>
        public int DivisionId { get; set; }
    }
}
