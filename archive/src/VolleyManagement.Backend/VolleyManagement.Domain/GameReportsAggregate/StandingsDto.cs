﻿namespace VolleyManagement.Domain.GameReportsAggregate
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a regular standings table for single division
    /// </summary>
    public class StandingsDto : DivisionStandingsDtoBase
    {
        public ICollection<StandingsEntry> Standings { get; set; } = new List<StandingsEntry>();
    }
}