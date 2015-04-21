namespace VolleyManagement.Domain.Tournaments
{
    using System;
    using VolleyManagement.Domain.Properties;

    /// <summary>
    /// Tournament domain class.
    /// </summary>
    public class Tournament
    {
        private string _name;
        private string _description;
        private short _season;
        private TournamentSchemeEnum _scheme;
        private string _regulationsLink;
        private DateTime _startDate;
        private DateTime _endDate;
        private DateTime _registrationStart;
        private DateTime _registrationEnd;
        private DateTime _transferStart;
        private DateTime _transferEnd;

        /// <summary>
        /// Gets or sets tournament start
        /// </summary>
        public DateTime StartDate
        {
            get
            {
                return _startDate;
            }

            set
            {
                // TODO: add validation
                _startDate = value;
            }
        }

        /// <summary>
        /// Gets or sets the tournament end 
        /// </summary>
        public DateTime EndDate
        {
            get
            {
                return _endDate;
            }

            set
            {
                _endDate = value;
            }
        }

        /// <summary>
        /// Gets or sets start registration date of a tournament
        /// </summary>
        public DateTime RegistrationStart
        {
            get
            {
                return _registrationStart;
            }
            
            set
            {
                _registrationStart = value;
            }
        }

        /// <summary>
        /// Gets or sets end registration date of a tournament 
        /// </summary>
        public DateTime RegistrationEnd 
        {
            get
            {
                return _registrationEnd;
            }

            set
            {
                _registrationEnd = value;
            }

        }

        /// <summary>
        /// Gets or sets transfer end of a tournament
        /// </summary>
        public DateTime TransferEnd
        {
            get
            {
                return _transferEnd;
            }

            set
            {
                _transferEnd = value;
            }
        }

        /// <summary>
        /// Gets or sets transfer start of a tournament
        /// </summary>
        public DateTime TransferStart
        {
            get
            {
                return _transferStart;
            }

            set
            {
                _transferStart = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating where Id.
        /// </summary>
        /// <value>Id of tournament.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating where Name.
        /// </summary>
        /// <value>Name of tournament.</value>
        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                if (string.IsNullOrEmpty(value) || value.Length > Constants.Tournament.MAX_NAME_LENGTH)
                {
                    throw new ArgumentException(Resources.ValidationResultName);
                }

                _name = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating where Description.
        /// </summary>
        /// <value>Description of tournament.</value>
        public string Description
        {
            get
            {
                return _description;
            }

            set
            {
                if (!string.IsNullOrEmpty(value) && value.Length > Constants.Tournament.MAX_DESCRIPTION_LENGTH)
                {
                    throw new ArgumentException(Resources.ValidationResultDescription);
                }

                _description = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating where Season.
        /// </summary>
        /// <value>Season of tournament.</value>
        public short Season
        {
            get
            {
                return _season;
            }

            set
            {
                if (value < Constants.Tournament.MINIMAL_SEASON_YEAR || value > Constants.Tournament.MAXIMAL_SEASON_YEAR)
                {
                    throw new ArgumentException(Resources.ValidationTournamentSeason);
                }

                _season = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating where Scheme.
        /// </summary>
        /// <value>Scheme of tournament.</value>
        public TournamentSchemeEnum Scheme
        {
            get
            {
                return _scheme;
            }

            set
            {
                if (!Enum.IsDefined(typeof(TournamentSchemeEnum), value))
                {
                    throw new ArgumentException(Resources.ValidationResultScheme, "Scheme");
                }

                _scheme = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating regulations of tournament.
        /// </summary>
        /// <value>regulations of tournament.</value>
        public string RegulationsLink
        {
            get
            {
                return _regulationsLink;
            }

            set
            {
                if (!string.IsNullOrEmpty(value) && value.Length > Constants.Tournament.MAX_REGULATION_LENGTH)
                {
                    throw new ArgumentException(Resources.ValidationResultRegLink);
                }

                _regulationsLink = value;
            }
        }
    }
}
