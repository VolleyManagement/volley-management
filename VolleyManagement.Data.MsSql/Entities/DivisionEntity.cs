namespace VolleyManagement.Data.MsSql.Entities
{
    /// <summary>
    /// DAL Division model
    /// </summary>
    public class DivisionEntity
    {
        /// <summary>
        /// Gets or sets division Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets division name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets division's tournament id
        /// </summary>
        public int TournamentId { get; set; }

        /// <summary>
        /// Gets or sets division's tournament
        /// </summary>
        public virtual TournamentEntity Tournament { get; set; }
    }
}
