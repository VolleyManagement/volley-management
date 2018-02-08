namespace VolleyManagement.UI.Areas.Admin.Models
{
    using System.Collections.Generic;

    using Domain.Dto;
    using Domain.RolesAggregate;

    /// <summary>
    /// The role edit view model.
    /// </summary>
    public class RoleEditViewModel : RoleViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleEditViewModel"/> class.
        /// </summary>
        /// <param name="role"> The role. </param>
        public RoleEditViewModel(Role role)
            : base(role)
        {
            UsersInRole = new List<UserViewModel>();
            UsersOutsideRole = new List<UserViewModel>();
        }

        /// <summary>
        /// Gets or sets the users in role.
        /// </summary>
        public List<UserViewModel> UsersInRole { get; set; }

        /// <summary>
        /// Gets or sets the users outside role.
        /// </summary>
        public List<UserViewModel> UsersOutsideRole { get; set; }

        /// <summary>
        /// Fills current instance with data provided by users in role collection
        /// </summary>
        /// <param name="usersInRoles"> The users in roles. </param>
        public void SetFromUsers(IEnumerable<UserInRoleDto> usersInRoles)
        {
            foreach (var user in usersInRoles)
            {
                if (user.RoleIds.Contains(Id))
                {
                    UsersInRole.Add(UserViewModel.From(user));
                }
                else
                {
                    UsersOutsideRole.Add(UserViewModel.From(user));
                }
            }
        }
    }
}