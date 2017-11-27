namespace VolleyManagement.UnitTests.Services.GameReportService
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.GameReportsAggregate;

    internal class PivotStandingsComparer : IComparer<PivotStandingsDto>
    {
        public int Compare(PivotStandingsDto x, PivotStandingsDto y)
        {
            if (x.Teams.Count == y.Teams.Count)
            {
                var teamsComparer = new TeamStandingsDtoComparer();
                var xTeams = x.Teams.OrderBy(t => t.TeamId).ToList();
                var yTeams = y.Teams.OrderBy(t => t.TeamId).ToList();
                for (var i = 0; i < xTeams.Count; i++)
                {
                    if (teamsComparer.Compare(xTeams[i], yTeams[i]) != 0)
                    {
                        return 1;
                    }
                }
            }
            else
            {
                return 1;
            }

            if (x.GameResults.Count == y.GameResults.Count)
            {
                var gameResultComparer = new ShortGameResultDtoComparer();
                var xGames = x.GameResults.OrderBy(g => g.HomeTeamId).ThenBy(g => g.AwayTeamId).ToList();
                var yGames = y.GameResults.OrderBy(g => g.HomeTeamId).ThenBy(g => g.AwayTeamId).ToList();
                for (var i = 0; i < xGames.Count; i++)
                {
                    if (gameResultComparer.Compare(xGames[i], yGames[i]) != 0)
                    {
                        return 1;
                    }
                }
            }
            else
            {
                return 1;
            }

            return 0;
        }
    }
}