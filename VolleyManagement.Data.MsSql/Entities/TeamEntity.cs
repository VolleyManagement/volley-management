namespace VolleyManagement.Data.MsSql.Entities
{
    /// <summary>
    /// DAL team model
    /// </summary>
    public class TeamEntity
    {
        /// <summary>
        /// Gets or sets id of team
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets name of team
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets coach of team
        /// </summary>
        public string Coach { get; set; }

        /// <summary>
        /// Gets or sets achievements of team
        /// </summary>
        public string Achievements { get; set; }

        /// <summary>
        /// Gets or sets Captain of the team
        /// </summary>
        public PlayerEntity Captain { get; set; }
    }
}