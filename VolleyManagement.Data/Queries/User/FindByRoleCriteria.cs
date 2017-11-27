namespace VolleyManagement.Data.Queries.User
{
    using Contracts;

    /// <summary>
    /// Criteria to find users assigned to specific role
    /// </summary>
    public class FindByRoleCriteria : IQueryCriteria
    {
        /// <summary>
        /// Gets or sets id of the role to search
        /// </summary>
        public int RoleId { get; set; }
    }
}