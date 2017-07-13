namespace VolleyManagement.UI.Areas.Mvc.ViewModels.Tournaments
{
    using Generic;
    using Teams;

    /// <summary>
    /// Represents TournamentTeamsListViewModel and referrer link.
    /// </summary>
    public class TournamentTeamsListReferrerViewModel : ReferrersViewModel<TournamentTeamsListViewModel, TournamentDivisionsListViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentTeamsListReferrerViewModel" /> class.
        /// </summary>
        /// <param name="teams">Tournament teams list view model.</param>
        /// <param name="divisions">Tournament divisions list view model.</param>
        /// <param name="referrer">Referrer controller name.</param>
        public TournamentTeamsListReferrerViewModel(TournamentTeamsListViewModel teams, TournamentDivisionsListViewModel divisions, string referrer)
            : base(teams, divisions, referrer)
        {
        }
    }
}