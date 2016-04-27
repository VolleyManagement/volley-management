namespace VolleyManagement.Domain.TournamentsAggregate
{
    using System;

    /// <summary>
    /// Represents tournament data transfer object
    /// </summary>
    public class TournamentScheduleDto
    {
        /// <summary>
        /// Gets or sets id of the tournament data transfer object
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the tournament name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets start date of the tournament data transfer object
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets date of the tournament data transfer object
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets the number of the teams in tournament
        /// </summary>
        public byte TeamCount { get; set; }

        /// <summary>
        /// Gets or sets the tournament scheme
        /// </summary>
        public TournamentSchemeEnum Scheme { get; set; }
    }
}
