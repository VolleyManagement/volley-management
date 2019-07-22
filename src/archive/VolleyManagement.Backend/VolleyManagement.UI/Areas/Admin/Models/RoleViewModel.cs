namespace VolleyManagement.UI.Areas.Admin.Models
{
    using Domain.RolesAggregate;

    /// <summary>
    /// The role view model.
    /// </summary>
    public class RoleViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleViewModel"/> class.
        /// </summary>
        public RoleViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleViewModel"/> class.
        /// </summary>
        /// <param name="role">
        /// The role.
        /// </param>
        public RoleViewModel(Role role)
        {
            Id = role.Id;
            Name = role.Name;
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }
    }
}