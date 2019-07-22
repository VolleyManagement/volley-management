namespace VolleyManagement.UI.Areas.Admin.Models
{
    using Domain.PlayersAggregate;
    using Domain.RequestsAggregate;
    using Domain.UsersAggregate;

    /// <summary>
    /// Represents request view model
    /// </summary>
    public class RequestViewModel
    {
        /// <summary>
        /// Gets or sets request id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets player last name.
        /// </summary>
        public string PlayerLastName { get; set; }

        /// <summary>
        /// Gets or sets user name
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets user's id.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets player's id.
        /// </summary>
        public int PlayerId { get; set; }

        /// <summary>
        /// Maps domain entity to presentation
        /// </summary>
        /// <param name="request"><see cref="Request"/> domain entity.</param>
        /// <param name="player"> player that binds.</param>
        /// <param name="user"> user who connects.</param>
        /// <returns> View model object</returns>
        public static RequestViewModel Map(Request request, Player player, User user)
        {
            return new RequestViewModel {
                Id = request.Id,
                UserId = user.Id,
                PlayerId = player.Id,
                PlayerLastName = player.LastName,
                UserName = user.UserName
            };
        }
    }
}