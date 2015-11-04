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
        /// Id of user
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// User name
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Role Id user belongs to
        /// </summary>
        public List<int> RoleIds { get; set; }
    }
}