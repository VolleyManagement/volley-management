namespace VolleyManagement.Domain.Dto
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents User view in regards to Role
    /// </summary>
    public class UserInRoleDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserInRoleDto"/> class.
        /// </summary>
        public UserInRoleDto()
        {
            RoleIds = new List<int>();
        }

        /// <summary>
        /// Gets or sets id of user
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets user name
        /// </summary>
        public string UserName { get; set; }

#pragma warning disable S3956 // see more https://stackoverflow.com/questions/25528649/exception-on-inner-linq-query-when-calling-tolist?rq=1
        /// <summary>
        /// Gets or sets role Id user belongs to
        /// </summary>
        public List<int> RoleIds { get; set; }
#pragma warning restore S3956 // Use more generic collection, LINQ convertation fails, EF bug
    }
}