namespace VolleyManagement.Domain.RolesAggregate
{
    /// <summary>
    /// Represents a role which user can have
    /// </summary>
    public class Role
    {
        /// <summary>
        /// Gets or sets unique identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets user-friendly name
        /// </summary>
        public string Name { get; set; }
    }
}