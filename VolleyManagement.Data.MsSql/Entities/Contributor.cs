namespace VolleyManagement.Data.MsSql.Entities
{
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// DAL user model
    /// </summary>
    [Table("Contributors")]
    public partial class Contributor
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
        [ForeignKey("ContributorTeamId")]
        public virtual ContributorTeam ContributorTeam { get; set; }

        /// <summary>
        /// Gets or sets Contributor Team Id of contributor
        /// </summary>
        public int? ContributorTeamId { get; set; }
    }
}