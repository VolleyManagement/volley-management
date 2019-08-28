namespace VolleyManagement.Domain.PlayersAggregate
{
    /// <summary>
    /// Common attributes for player
    /// </summary>
    public class CreatePlayerDto
    {
        /// <summary>
        /// Gets or sets first name of player
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets last name of player
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets birth year of player
        /// </summary>
        public short? BirthYear { get; set; }

        /// <summary>
        /// Gets or sets height of player
        /// </summary>
        public short? Height { get; set; }

        /// <summary>
        /// Gets or sets weight of player
        /// </summary>
        public short? Weight { get; set; }
    }
}