namespace VolleyManagement.Domain.GameReportsAggregate
{
    using System.Collections.Generic;

    public class StandingsDto
    {
        public int DivisionId { get; set; }

        public string DivisionName { get; set; }

        public List<StandingsEntry> Standings { get; set; } = new List<StandingsEntry>();
    }
}