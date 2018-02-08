namespace VolleyManagement.Data.MsSql.Entities
{
    /// <summary>
    /// DAL user model
    /// </summary>
    public class ContributorEntity
    {
        /// <summary>
        /// Gets or sets id of contributor
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets first name of contributor
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Contributor Team of contributor
        /// </summary>
        public virtual ContributorTeamEntity Team { get; set; }
    }
}