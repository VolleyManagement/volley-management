namespace VolleyManagement.Domain.Tournaments
{
    using System;
    using VolleyManagement.Domain.Properties;

    /// <summary>
    /// Tournament domain class.
    /// </summary>
    public class Tournament
    {
        /// <summary>
        /// Name of tournament
        /// </summary>
        private string _name;

        /// <summary>
        /// Description of tournament
        /// </summary>
        private string _description;

        /// <summary>
        /// Season of tournament
        /// </summary>
        private string _season;

        /// <summary>
        /// Scheme of tournament
        /// </summary>
        private TournamentSchemeEnum _scheme;

        /// <summary>
        /// Season of tournament
        /// </summary>
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
                if (string.IsNullOrEmpty(value) || value.Length > 60)
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
                if (value.Length > 300)
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
        public string Season
        {
            get
            {
                return _season;
            }

            set
            {
                if (string.IsNullOrEmpty(value) || value.Length > 9)
                {
                    throw new ArgumentException(Resources.ValidationResultSeason);
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
                if (!Enum.IsDefined(typeof(TournamentSchemeEnum), Scheme))
                {
                    throw new ArgumentException(Resources.ValidationResultScheme);
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
                if (value.Length > 255)
                {
                    throw new ArgumentException(Resources.ValidationResultRegLink);
                }
                _regulationsLink = value;
            }
        }
    }
}
