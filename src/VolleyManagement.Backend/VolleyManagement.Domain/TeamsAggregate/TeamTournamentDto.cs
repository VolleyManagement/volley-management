namespace VolleyManagement.Domain.TeamsAggregate
{
    using System.Collections.Generic;

    public class TeamTournamentDto
    {
        public int TeamId { get; set; }

        public string TeamName { get; set; }

        public int DivisionId { get; set; }

        public string DivisionName { get; set; }

        public int GroupId { get; set; }

        public string GroupName { get; set; }

        public ICollection<PlayerId> Roster { get; set; }
    }
}
