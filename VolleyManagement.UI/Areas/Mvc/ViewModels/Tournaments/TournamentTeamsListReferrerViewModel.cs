namespace VolleyManagement.UI.Areas.Mvc.ViewModels.Tournaments
{
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Generic;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Teams;

    /// <summary>
    /// Represents TournamentTeamsListViewModel and referrer link.
    /// </summary>
    public class TournamentTeamsListReferrerViewModel : RefererViewModel<TournamentTeamsListViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentTeamsListReferrerViewModel" /> class.
        /// </summary>
        /// <param name="teams">Tournament teams list view model.</param>
        /// <param name="referrer">Referrer controller name.</param>
        public TournamentTeamsListReferrerViewModel(TournamentTeamsListViewModel teams, string referrer)
            : base(teams, referrer)
        {
        }
    }
}