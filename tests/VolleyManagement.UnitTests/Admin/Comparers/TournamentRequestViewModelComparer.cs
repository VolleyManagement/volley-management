namespace VolleyManagement.UnitTests.Admin.Comparers
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using UI.Areas.Admin.Models;

    /// <summary>
    /// Compares Tournament requests instances
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class TournamentRequestViewModelComparer : IEqualityComparer<TournamentRequestViewModel>
    {
        private bool AreEqual(TournamentRequestViewModel x, TournamentRequestViewModel y)
        {
            return x.Id == y.Id &&
                   x.TeamId == y.TeamId &&
                   x.TournamentId == y.TournamentId &&
                   x.PersonName == y.PersonName &&
                   x.TeamTitle == y.TeamTitle &&
                   x.TournamentTitle == y.TournamentTitle;
        }

        public bool Equals(TournamentRequestViewModel x, TournamentRequestViewModel y)
        {
            return AreEqual(x, y);
        }

        public int GetHashCode(TournamentRequestViewModel obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}