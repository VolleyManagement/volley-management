using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolleyManagement.Domain.TeamsAggregate;

namespace VolleyManagement.UnitTests.Services.TeamService
{
    class CreateTeamDtoComparer : IComparer<CreateTeamDto>, IComparer
    {
        public int Compare(object x, object y)
        {
            var firstTeam = x as CreateTeamDto;
            var secondTeam = y as CreateTeamDto;

            if (firstTeam == null)
            {
                return -1;
            }
            else if (secondTeam == null)
            {
                return 1;
            }

            return Compare(firstTeam, secondTeam);
        }

        public int Compare(CreateTeamDto x, CreateTeamDto y)
        {
            return AreEqual(x, y) ? 0 : 1;
        }

        private bool AreEqual(CreateTeamDto x, CreateTeamDto y)
        {
            var simpleDataIsEquialent = x.Name.Equals(y.Name) &&
                x.Coach.Equals(y.Coach) &&
                x.Achievements.Equals(y.Achievements) &&
                x.Captain.Id == y.Captain.Id;

            if (simpleDataIsEquialent)
            {
                var xRosterIds = x.Roster.Select(p => p.Id);
                var yRosterIds = y.Roster.Select(p => p.Id);

                return xRosterIds.SequenceEqual(yRosterIds);
            }
            else
            {
                return false;
            }
        }
    }
}
