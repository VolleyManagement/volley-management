namespace VolleyManagement.Domain.GamesAggregate
{
    /// <summary>
    /// Penalty for violating regulations
    /// </summary>
    public class Penalty
    {
        /// <summary>
        /// Gets or sets a value indicating whether Home or Away team got penalty
        /// </summary>
        public bool IsHomeTeam { get; set; }

        /// <summary>
        /// Gets or sets amount of points penalized
        /// </summary>
        public byte Amount { get; set; }

        /// <summary>
        /// Gets or sets short reason for the penalty
        /// </summary>
        public string Description { get; set; }
    }
}