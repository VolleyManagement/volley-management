namespace VolleyManagement.Data.MsSql.Entities
{
    /// <summary>
    /// DAL tournament's request model
    /// </summary>
    public class TournamentRequestEntity
    {
        /// <summary>
        /// Gets or sets id of request
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets user's id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets tournament's id
        /// </summary>
        public int GroupId { get; set; }

        /// <summary>
        /// Gets or sets team's id
        /// </summary>
        public int TeamId { get; set; }
    }
}
