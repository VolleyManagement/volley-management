namespace VolleyManagement.Domain.TournamentRequestAggregate
{
    /// <summary>
    /// Tournament request domain class.
    /// </summary>
    public class TournamentRequest
    {
        /// <summary>
        /// Gets or sets Id.
        /// </summary>
        /// <value>Id of tournament's request.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets user's id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets tournament's id
        /// </summary>
        public int TournamentId { get; set; }

        /// <summary>
        /// Gets or sets team's id
        /// </summary>
        public int TeamId { get; set; }
    }
}
