namespace VolleyManagement.Domain.RolesAggregate
{
    /// <summary>
    /// Represents a role which user can have
    /// </summary>
    public class Role
    {
        /// <summary>
        /// Unique identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// User-friendly name
        /// </summary>
        public string Name { get; set; }
    }
}