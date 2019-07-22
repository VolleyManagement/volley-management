namespace VolleyManagement.Data.MsSql.Entities
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// DAL tournament model
    /// </summary>
    public class TournamentEntity
    {
        private ICollection<DivisionEntity> _divisions;

        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentEntity"/> class.
        /// </summary>
        public TournamentEntity()
        {
            _divisions = new List<DivisionEntity>();
        }

        /// <summary>
        /// Gets or sets the tournament id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the tournament name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the tournament scheme
        /// </summary>
        public byte Scheme { get; set; }

        /// <summary>
        /// Gets or sets the tournament season as a byte offset from the 1900
        /// </summary>
        public byte Season { get; set; }

        /// <summary>
        /// Gets or sets the tournament description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the tournament location
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets regulations of the tournament
        /// </summary>
        public string RegulationsLink { get; set; }

        /// <summary>
        /// Gets or sets start of a tournament
        /// </summary>
        public DateTime GamesStart { get; set; }

        /// <summary>
        /// Gets or sets end of a tournament
        /// </summary>
        public DateTime GamesEnd { get; set; }

        /// <summary>
        /// Gets or sets start of a transfer period
        /// </summary>
        public DateTime? TransferStart { get; set; }

        /// <summary>
        /// Gets or sets end of a transfer period
        /// </summary>
        public DateTime? TransferEnd { get; set; }

        /// <summary>
        /// Gets or sets start of a tournament
        /// </summary>
        public DateTime ApplyingPeriodStart { get; set; }

        /// <summary>
        /// Gets or sets end of a tournament registration
        /// </summary>
        public DateTime ApplyingPeriodEnd { get; set; }

        /// <summary>
        /// Gets or sets collection of tournaments divisions
        /// </summary>
        public virtual ICollection<DivisionEntity> Divisions
        {
            get => _divisions;
            set => _divisions = value;
        }

        /// <summary>
        /// Gets or sets game results of the tournament.
        /// </summary>
        public virtual ICollection<GameResultEntity> GameResults { get; set; }

        /// <summary>
        /// Gets or sets last time, when tournament was updated
        /// </summary>
        public DateTime? LastTimeUpdated { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether tournament is archived
        /// </summary>
        public bool IsArchived { get; set; }
    }
}