namespace VolleyManagement.Data.MsSql.Entities
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a Role user can have
    /// </summary>
    public class RoleEntity
    {
        /// <summary>
        /// Gets or sets identifier of the role
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets user-friendly name of the role
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets returns all users assigned to role
        /// </summary>
        public virtual ICollection<UserEntity> Users { get; set; }
    }
}