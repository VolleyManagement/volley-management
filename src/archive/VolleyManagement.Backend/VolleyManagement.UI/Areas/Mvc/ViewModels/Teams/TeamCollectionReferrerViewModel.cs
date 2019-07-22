namespace VolleyManagement.UI.Areas.Mvc.ViewModels.Teams
{
    using Generic;

    /// <summary>
    /// Represents TeamCollectionViewModel and referrer link.
    /// </summary>
    public class TeamCollectionReferrerViewModel : RefererViewModel<TeamCollectionViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TeamCollectionReferrerViewModel" /> class.
        /// </summary>
        /// <param name="teams">Team collection view model.</param>
        /// <param name="referrer">Referrer controller name.</param>
        public TeamCollectionReferrerViewModel(TeamCollectionViewModel teams, string referrer)
            : base(teams, referrer)
        {
        }
    }
}