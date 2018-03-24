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
        /// Initializes a new instance of the Player
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="firstName">Fisrt Name</param>
        /// <param name="lastName">Last Name</param>
        public Player(int id, string firstName, string lastName) : this(id, firstName, lastName, null, null, null)
        {

        }

        /// <summary>
        /// Initializes a new instance of the Player
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="firstName">First Name</param>
        /// <param name="lastName">Last Name</param>
        /// <param name="birthYear">BirthYear</param>
        /// <param name="height">Height</param>
        /// <param name="weight">Weight</param>
        public Player(int id, string firstName, string lastName, short? birthYear, short? height, short? weight)
            : this(id, firstName, lastName, birthYear, height, weight, null)
        {

        }

        /// <summary>
        /// Initializes a new instance of the Player
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="firstName">First Name</param>
        /// <param name="lastName">Last Name</param>
        /// <param name="birthYear">BirthYear</param>
        /// <param name="height">Height</param>
        /// <param name="weight">Weight</param>
        public Player(int id, string firstName, string lastName, short? birthYear, short? height, short? weight, int? teamId)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            BirthYear = birthYear;
            Height = height;
            Weight = weight;
            TeamId = teamId;
        }

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
            get => _firstName;

            private set
            {
                if (PlayerValidation.ValidateFirstName(value))
                {
                    throw new ArgumentException(Resources.ValidationPlayerFirstName, nameof(value));
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
            get => _lastName;

            private set
            {
                if (PlayerValidation.ValidateLastName(value))
                {
                    throw new ArgumentException(Resources.ValidationPlayerLastName, nameof(value));
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
            get => _birthYear;

            private set
            {
                if (PlayerValidation.ValidateBirthYear(value))
                {
                    throw new ArgumentException(Resources.ValidationPlayerBirthYear, nameof(value));
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
            get => _height;

            private set
            {
                if (PlayerValidation.ValidateHeight(value))
                {
                    throw new ArgumentException(Resources.ValidationPlayerHeight, nameof(value));
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
            get => _weight;

            private set
            {
                if (PlayerValidation.ValidateWeight(value))
                {
                    throw new ArgumentException(Resources.ValidationPlayerWeight, nameof(value));
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
