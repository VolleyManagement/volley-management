namespace VolleyManagement.UI.Areas.Admin.Models
{
    using Domain.TeamsAggregate;
    using Domain.TournamentRequestAggregate;
    using Domain.TournamentsAggregate;
    using Domain.UsersAggregate;

    /// <summary>
    /// The tournament's request view model.
    /// </summary>
    public class TournamentRequestViewModel
    {
        /// <summary>
        /// Gets or sets request Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets team Id.
        /// </summary>
        public int TeamId { get; set; }

        /// <summary>
        /// Gets or sets tournament Id.
        /// </summary>
        public int TournamentId { get; set; }

        /// <summary>
        /// Gets or sets person Id.
        /// </summary>
        public int PersonId { get; set; }

        /// <summary>
        /// Gets or sets tournament title.
        /// </summary>
        public string TournamentTitle { get; set; }

        /// <summary>
        /// Gets or sets team title.
        /// </summary>
        public string TeamTitle { get; set; }

        /// <summary>
        /// Gets or sets person name.
        /// </summary>
        public string PersonName { get; set; }

        /// <summary>
        /// Maps domain entity to presentation
        /// </summary>
        /// <param name="request"><see cref="TournamentRequest"/> domain entity.</param>
        /// <param name="team">Team to bind</param>
        /// <param name="user"> User who connects.</param>
        /// <param name="tournament">Tournament to bind</param>
        /// <returns> View model object</returns>
        public static TournamentRequestViewModel Map(
                                                    TournamentRequest request,
                                                    Team team,
                                                    User user,
                                                    Tournament tournament)
        {
            return new TournamentRequestViewModel()
            {
                Id = request.Id,
                PersonId = request.UserId,
                TournamentId = request.TournamentId,
                TeamId = request.TeamId,
                PersonName = user.PersonName,
                TeamTitle = team.Name
            };
        }

        /// <summary>
        /// Maps tournament entity to domain.
        /// </summary>
        /// <returns>Domain object.</returns>
        public TournamentRequest ToDomain()
        {
            return new TournamentRequest
            {
                Id = Id,
                TeamId = TeamId,
                TournamentId = TournamentId
            };
        }
    }
}