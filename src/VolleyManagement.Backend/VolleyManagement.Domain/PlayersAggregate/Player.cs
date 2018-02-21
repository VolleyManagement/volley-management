namespace VolleyManagement.Domain.PlayersAggregate
{
    using System;

    using Properties;

    /// <summary>
    /// Player domain class.
    /// </summary>
    public class Player
    {
        private string _firstName;
        private string _lastName;
        private short? _birthYear;
        private short? _height;
        private short? _weight;

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
                if (PlayerValidation.ValidateFirstName(value))
                {
                    throw new ArgumentException(Resources.ValidationPlayerFirstName, nameof(Player.FirstName));
                }

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
                if (PlayerValidation.ValidateLastName(value))
                {
                    throw new ArgumentException(Resources.ValidationPlayerLastName, nameof(Player.LastName));
                }

                _lastName = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating where BirthYear.
        /// </summary>
        /// <value>Birth year.</value>
        public short? BirthYear
        {
            get
            {
                return _birthYear;
            }

            set
            {
                if (PlayerValidation.ValidateBirthYear(value))
                {
                    throw new ArgumentException(Resources.ValidationPlayerBirthYear, nameof(Player.BirthYear));
                }

                _birthYear = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating where Height.
        /// </summary>
        /// <value>The height.</value>
        public short? Height
        {
            get
            {
                return _height;
            }

            set
            {
                if (PlayerValidation.ValidateHeight(value))
                {
                    throw new ArgumentException(Resources.ValidationPlayerHeight, nameof(Player.Height));
                }

                _height = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating where Weight.
        /// </summary>
        /// <value>The weight.</value>
        public short? Weight
        {
            get
            {
                return _weight;
            }

            set
            {
                if (PlayerValidation.ValidateWeight(value))
                {
                    throw new ArgumentException(Resources.ValidationPlayerWeight, nameof(Player.Weight));
                }

                _weight = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating where Team.
        /// </summary>
        /// <value>The player team.</value>
        public int? TeamId { get; set; }
    }
}
