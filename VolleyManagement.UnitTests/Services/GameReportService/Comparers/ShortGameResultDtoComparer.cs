namespace VolleyManagement.UnitTests.Services.GameReportService
{
    using System.Collections.Generic;
    using Domain.GameReportsAggregate;

    internal class ShortGameResultDtoComparer : IComparer<ShortGameResultDto>
    {
        public int Compare(ShortGameResultDto x, ShortGameResultDto y)
        {
            if (x.HomeTeamId != y.HomeTeamId)
            {
                return 1;
            }

            if (x.HomeSetsScore != y.HomeSetsScore)
            {
                return 1;
            }

            if (x.AwayTeamId != y.AwayTeamId)
            {
                return 1;
            }

            if (x.AwaySetsScore != y.AwaySetsScore)
            {
                return 1;
            }

            if (x.IsTechnicalDefeat != y.IsTechnicalDefeat)
            {
                return 1;
            }

            return 0;
        }
    }
}