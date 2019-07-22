﻿namespace VolleyManagement.UI.Areas.Admin.Models
{
    using System.Collections.Generic;
    using Domain.RolesAggregate;

    /// <summary>
    /// The role details view model.
    /// </summary>
    public class RoleDetailsViewModel : RoleViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleDetailsViewModel"/> class.
        /// </summary>
        /// <param name="role">
        /// The role.
        /// </param>
        public RoleDetailsViewModel(Role role)
            : base(role)
        {
            Users = new List<string>();
        }

        /// <summary>
        /// Gets or sets user names in current role
        /// </summary>
        public ICollection<string> Users { get; set; }
    }
}