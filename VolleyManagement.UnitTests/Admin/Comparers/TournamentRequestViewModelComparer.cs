namespace VolleyManagement.UnitTests.Admin.Comparers
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using VolleyManagement.UI.Areas.Admin.Models;

    /// <summary>
    /// Compares Tournament requests instances
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class TournamentRequestViewModelComparer : IComparer<TournamentRequestViewModel>, IComparer
    {
        public int Compare(TournamentRequestViewModel x, TournamentRequestViewModel y)
        {
            return AreEqual(x, y) ? 0 : 1;
        }

        public int Compare(object x, object y)
        {
            TournamentRequestViewModel firstRequest = x as TournamentRequestViewModel;
            TournamentRequestViewModel secondRequest = y as TournamentRequestViewModel;

            if (firstRequest == null)
            {
                return -1;
            }
            else if (secondRequest == null)
            {
                return 1;
            }

            return Compare(firstRequest, secondRequest);
        }

        private bool AreEqual(TournamentRequestViewModel x, TournamentRequestViewModel y)
        {
           return x.Id == y.Id &&
                  x.TeamId == y.TeamId &&
                  x.TournamentId == y.TournamentId &&
                  x.PersonName == y.PersonName &&
                  x.TeamTitle == y.TeamTitle &&
                  x.TournamentTitle == y.TournamentTitle;
        }
    }
}