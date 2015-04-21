namespace VolleyManagement.Domain.Tournaments
{
    using System;
    using VolleyManagement.Domain.Properties;

    public enum TournamentState { finished, current, upcoming, notStarted };

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

        public TournamentState State
        {
            get
            {
                if (StartDate > Constants.APPLICATION_DATE.AddMonths(Constants.Tournament.UPCOMING_TOURNAMENTS_MONTH_LIMIT))
                {
                    return TournamentState.notStarted;
                }
                else if(StartDate > Constants.APPLICATION_DATE
                    && StartDate <= Constants.APPLICATION_DATE.AddMonths(Constants.Tournament.UPCOMING_TOURNAMENTS_MONTH_LIMIT))
                {
                    return TournamentState.upcoming;
                }
                else if (StartDate <= Constants.APPLICATION_DATE && EndDate >= Constants.APPLICATION_DATE)
                {
                    return TournamentState.current;
                }
                else if (EndDate < Constants.APPLICATION_DATE)
                {
                    return TournamentState.finished;
                }
                else
                {
                    throw new ArgumentException("The state haven't been set");
                }
            }
        }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
