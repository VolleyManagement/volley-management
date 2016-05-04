namespace VolleyManagement.UnitTests.Services.GameReportService
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using VolleyManagement.Domain.GameReportsAggregate;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.GameReports;

    [ExcludeFromCodeCoverage]
    internal class ShortGameResultDtoTetsFixture
    {
        private List<ShortGameResultDto> _shortResults;

        public ShortGameResultDtoTetsFixture GetShortResults()
        {
            _shortResults = new List<ShortGameResultDto>();

            _shortResults.Add(
                new ShortGameResultDto
                {
                    HomeTeamId = 1,
                    AwayTeamId = 3,
                    HomeSetsScore = 3,
                    AwaySetsScore = 2,
                    IsTechnicalDefeat = false
                });

            _shortResults.Add(
                new ShortGameResultDto
                {
                    HomeTeamId = 3,
                    AwayTeamId = 1,
                    HomeSetsScore = 3,
                    AwaySetsScore = 0,
                    IsTechnicalDefeat = true
                });

            _shortResults.Add(
                new ShortGameResultDto
                {
                    HomeTeamId = 2,
                    AwayTeamId = 1,
                    HomeSetsScore = 1,
                    AwaySetsScore = 3,
                    IsTechnicalDefeat = false
                });

            return this;
        }

        public List<ShortGameResultDto> Build()
        {
            return _shortResults;
        }
    }
}