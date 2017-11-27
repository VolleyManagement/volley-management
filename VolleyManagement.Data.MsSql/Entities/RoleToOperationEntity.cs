namespace VolleyManagement.Data.MsSql.Entities
{
    /// <summary>
    /// Represents a Operation allowed to role
    /// </summary>
    public class RoleToOperationEntity
    {
        /// <summary>
        /// Gets or sets identifier of the operation to role entry
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets identifier of the Role
        /// </summary>
        public virtual int RoleId { get; set; }

        /// <summary>
        /// Gets or sets identifier of the operation
        /// </summary>
        public short OperationId { get; set; }
    }
}
