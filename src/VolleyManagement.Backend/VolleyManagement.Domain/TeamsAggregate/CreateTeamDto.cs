namespace VolleyManagement.Domain.TeamsAggregate
{
    public class CreateTeamDto
    {
        public string Name { get; set; }
        public string Achievements { get; set; }
        public string Coach { get; set; }
        public PlayerId CaptainId { get; set; }
    }
}
