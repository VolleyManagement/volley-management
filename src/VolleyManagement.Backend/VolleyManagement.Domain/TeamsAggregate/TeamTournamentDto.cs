namespace VolleyManagement.Domain.TeamsAggregate
{
    public class TeamTournamentDto
    {
        public int TeamId { get; set; }

        public string TeamName { get; set; }

        public int DivisionId { get; set; }

        public string DivisionName { get; set; }

        public int GroupId { get; set; }
    }
}