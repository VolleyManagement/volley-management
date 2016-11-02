namespace VolleyManagement.Data.MsSql.Entities
{
    using System.Collections.Generic;

    /// <summary>
    /// DAL player model
    /// </summary>
    public class PlayerEntity
    {
        /// <summary>
        /// Gets or sets id of player
        /// </summary>
        public int Id { get; set; }

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

        /// <summary>
        /// Gets or sets Team which this player is captain of
        /// </summary>
        public int? TeamId { get; set; }

        /// <summary>
        /// Gets or sets Team which player playing currently
        /// </summary>
        public virtual TeamEntity Team { get; set; }

        /// <summary>
        /// Gets or sets team which this player is captain of
        /// !!! SHOULD NOT BE USED !!!
        /// Created to address mapping issues for Team - Captain relationship
        /// </summary>
        public virtual ICollection<TeamEntity> LedTeam { get; set; }

        public virtual ICollection<UserEntity> PlayerUser { get; set; }
    }
}