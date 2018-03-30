namespace VolleyManagement.Domain.PlayersAggregate
{
    using System;
    using Properties;

    using static PlayerValidation;

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
        private int? _teamId;

        /// <summary>
        /// Initializes a new instance of the Player
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="firstName">Fisrt Name</param>
        /// <param name="lastName">Last Name</param>
        public Player(int id, string firstName, string lastName) : this(id, firstName, lastName, null, null, null, null)
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
        /// <param name="teamId">Id of the team, which roster this player is member of.</param>
        public Player(int id, string firstName, string lastName, short? birthYear, short? height, short? weight, int? teamId)
        {
            if (ValidateId(id))
            {
                throw new ArgumentException(Resources.ValidationPlayerId,
                    nameof(id));
            }

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
        public int Id { get; }

        /// <summary>
        /// Gets or sets a value indicating where FirstName.
        /// </summary>
        /// <value>First name.</value>
        public string FirstName
        {
            get => _firstName;

            set
            {
                if (ValidateFirstName(value))
                {
                    throw new ArgumentException(Resources.ValidationPlayerFirstName,
                        nameof(value));
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

            set
            {
                if (ValidateLastName(value))
                {
                    throw new ArgumentException(Resources.ValidationPlayerLastName,
                        nameof(value));
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

            set
            {
                if (ValidateBirthYear(value))
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

            set
            {
                if (ValidateHeight(value))
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

            set
            {
                if (ValidateWeight(value))
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
        public int? TeamId
        {
            get => _teamId;
            set
            {
                if (ValidateTeamId(value))
                {
                    throw new ArgumentException(Resources.ValidationTeamId,
                        nameof(value));
                }

                _teamId = value;
            }
        }
    }
}
