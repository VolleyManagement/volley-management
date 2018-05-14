using VolleyManagement.Domain.TeamsAggregate;

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
        {
            if (ValidateId(id))
            {
                throw new EntityInvariantViolationException(Resources.ValidationPlayerId);
            }

            Id = id;
            FirstName = firstName;
            LastName = lastName;
            BirthYear = birthYear;
            Height = height;
            Weight = weight;
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
                    throw new EntityInvariantViolationException(Resources.ValidationPlayerFirstName);
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
                    throw new EntityInvariantViolationException(Resources.ValidationPlayerLastName);
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
                    throw new EntityInvariantViolationException(Resources.ValidationPlayerBirthYear);
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
                    throw new EntityInvariantViolationException(Resources.ValidationPlayerHeight);
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
                    throw new EntityInvariantViolationException(Resources.ValidationPlayerWeight);
                }

                _weight = value;
            }
        }

    }
}
