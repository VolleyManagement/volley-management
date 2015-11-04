namespace VolleyManagement.UI.Areas.Admin.Models
{
    using VolleyManagement.Domain.Dto;

    /// <summary>
    /// The user view model.
    /// </summary>
    public class UserViewModel
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Creates <see cref="UserViewModel"/> instance based on <see cref="UserInRoleDto"/>
        /// </summary>
        /// <param name="user"> The source instance. </param>
        /// <returns> The <see cref="UserViewModel"/>. </returns>
        public static UserViewModel From(UserInRoleDto user)
        {
            return new UserViewModel { Id = user.UserId, Name = user.UserName };
        }
    }
}