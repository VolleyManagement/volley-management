namespace VolleyManagement.UI.Areas.Admin.Models
{
    /// <summary>
    /// Represents modified data
    /// </summary>
    public class ModifiedRoleViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModifiedRoleViewModel"/> class.
        /// </summary>
        public ModifiedRoleViewModel()
        {
            IdsToAdd = new int[0];
            IdsToDelete = new int[0];
        }

        /// <summary>
        /// Gets or sets the role id.
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// Gets or sets the ids to add.
        /// </summary>
        public int[] IdsToAdd { get; set; }

        /// <summary>
        /// Gets or sets the ids to delete.
        /// </summary>
        public int[] IdsToDelete { get; set; }
    }
}