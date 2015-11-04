namespace VolleyManagement.Data.Queries.User
{
    using VolleyManagement.Data.Contracts;

    /// <summary>
    /// Criteria to find users assigned to specific role
    /// </summary>
    public class FindByRoleCriteria : IQueryCriteria
    {
        /// <summary>
        /// Id of the role to search
        /// </summary>
        public int RoleId { get; set; }
    }
}