namespace VolleyManagement.Domain.TeamsAggregate
{
    using System.Collections.Generic;

    public class CreateTeamDto
    {
        public string Name { get; set; }
        public string Achievements { get; set; }
        public string Coach { get; set; }
        public PlayerId CaptainId { get; set; }
        public ICollection<PlayerId> Roster { get; set; }
    }
}
