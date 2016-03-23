namespace VolleyManagement.Data.Queries.Team
{
    using System.Collections.Generic;
    using VolleyManagement.Data.Contracts;

    /// <summary>
    /// Represents criteria to retrieve teams from the game results.
    /// </summary>
    public class GameResultsTeamsCriteria : IQueryCriteria
    {
        /// <summary>
        /// Gets or sets the collection of team's identifiers.
        /// </summary>
        public IEnumerable<int> TeamIds { get; set; }
    }
}
