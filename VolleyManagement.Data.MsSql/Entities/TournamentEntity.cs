namespace VolleyManagement.Data.MsSql.Entities
{
    using System;

    /// <summary>
    /// DAL tournament model
    /// </summary>
    public class TournamentEntity
    {
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
        public DateTime TransferStart { get; set; }

        /// <summary>
        /// Gets or sets end of a transfer period
        /// </summary>
        public DateTime TransferEnd { get; set; }

        /// <summary>
        /// Gets or sets start of a tournament
        /// </summary>
        public DateTime ApplyingPeriodStart { get; set; }

        /// <summary>
        /// Gets or sets end of a tournament registration
        /// </summary>
        public DateTime ApplyingPeriodEnd { get; set; }
    }
}
