namespace VolleyManagement.Domain.TournamentsAggregate
{
    using System;
    using System.Collections.Generic;
    using VolleyManagement.Crosscutting.Contracts.Providers;
    using VolleyManagement.Domain.Properties;

    /// <summary>
    /// Tournament domain class.
    /// </summary>
    public class Tournament
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Tournament"/> class.
        /// </summary>
        public Tournament()
        {
            this.Divisions = new List<Division>();
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
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating where Description.
        /// </summary>
        /// <value>Description of tournament.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating where Season.
        /// </summary>
        /// <value>Season of tournament.</value>
        public short Season { get; set; }

        /// <summary>
        /// Gets or sets a value indicating where Scheme.
        /// </summary>
        /// <value>Scheme of tournament.</value>
        public TournamentSchemeEnum Scheme { get; set; }

        /// <summary>
        /// Gets or sets a value indicating regulations of tournament.
        /// </summary>
        /// <value>regulations of tournament.</value>
        public string RegulationsLink { get; set; }

        /// <summary>
        /// Gets tournament state
        /// </summary>
        public TournamentStateEnum State
        {
            get
            {
                DateTime now = TimeProvider.Current.UtcNow;
                DateTime limitUpcomingTournamentsStartDate
                    = now.AddMonths(Constants.Tournament.UPCOMING_TOURNAMENTS_MONTH_LIMIT);

                if (GamesStart > limitUpcomingTournamentsStartDate)
                {
                    return TournamentStateEnum.NotStarted;
                }
                else if (GamesStart > now
                    && GamesStart <= limitUpcomingTournamentsStartDate)
                {
                    return TournamentStateEnum.Upcoming;
                }
                else if (GamesStart <= now && GamesEnd >= now)
                {
                    return TournamentStateEnum.Current;
                }
                else
                {
                    return TournamentStateEnum.Finished;
                }
            }
        }

        /// <summary>
        /// Gets or sets tournament start
        /// </summary>
        public DateTime GamesStart { get; set; }

        /// <summary>
        /// Gets or sets the tournament end
        /// </summary>
        public DateTime GamesEnd { get; set; }

        /// <summary>
        /// Gets or sets start registration date of a tournament
        /// </summary>
        public DateTime ApplyingPeriodStart { get; set; }

        /// <summary>
        /// Gets or sets end registration date of a tournament
        /// </summary>
        public DateTime ApplyingPeriodEnd { get; set; }

        /// <summary>
        /// Gets or sets transfer end of a tournament
        /// </summary>
        public DateTime? TransferEnd { get; set; }

        /// <summary>
        /// Gets or sets transfer start of a tournament
        /// </summary>
        public DateTime? TransferStart { get; set; }

        /// <summary>
        /// Divisions of the tournament
        /// </summary>
        public List<Division> Divisions { get; set; }

        public virtual DateTime? LastTimeUpdated { get; set; }
    }
}
