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

        /// <summary>
        /// Gets or sets role Id user belongs to
        /// </summary>
        public ICollection<int> RoleIds { get; set; }
    }
}