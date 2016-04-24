namespace VolleyManagement.Data.MsSql.Entities
{
    /// <summary>
    /// Represents a Operation allowed to role
    /// </summary>
    public class RoleToOperationEntity
    {
        /// <summary>
        /// Identifier of the operation to role entry
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Identifier of the Role
        /// </summary>
        public virtual int RoleId { get; set; }

        /// <summary>
        /// Identifier of the operation
        /// </summary>
        public short OperationId { get; set; }
    }
}
