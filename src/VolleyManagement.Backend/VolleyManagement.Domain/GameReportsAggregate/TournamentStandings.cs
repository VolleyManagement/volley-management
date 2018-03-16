namespace VolleyManagement.Domain.GameReportsAggregate
{
    using System.Collections.Generic;

    public class TournamentStandings<T>
    {
        public ICollection<T> Divisions { get; set; } = new List<T>();
    }
}