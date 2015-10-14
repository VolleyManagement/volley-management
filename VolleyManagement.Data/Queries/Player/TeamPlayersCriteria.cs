namespace VolleyManagement.Data.Queries.Player
{
    using VolleyManagement.Data.Contracts;

    /// <summary>
    /// Provides parameters to retrieve Team roster
    /// </summary>
    public class TeamPlayersCriteria : IQueryCriteria
    {
        public int TeamId { get; set; }
    }
}