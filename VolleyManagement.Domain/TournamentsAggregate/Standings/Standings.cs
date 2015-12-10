namespace VolleyManagement.Domain.TournamentsAggregate.Standings
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents tournament's standings.
    /// </summary>
    public class Standings
    {
        private List<StandingsEntry> _entries;

        /// <summary>
        /// Initializes a new instance of the <see cref="Standings"/> class.
        /// </summary>
        public Standings()
        {
            _entries = new List<StandingsEntry>();
        }

        /// <summary>
        /// Gets all entries of the standings.
        /// </summary>
        public List<StandingsEntry> Entries
        {
            get
            {
                return _entries;
            }
        }

        /// <summary>
        /// Adds an entry to the standings.
        /// </summary>
        /// <param name="entry">Entry to add to the standings.</param>
        public void AddEntry(StandingsEntry entry)
        {
            _entries.Add(entry);
        }

        /// <summary>
        /// Rebuilds tournament's standings, ordering all entries by points, by sets ratio and by balls ratio in descending order.
        /// </summary>
        /// <returns>Instance of <see cref="Standings"/> class.</returns>
        public Standings Rebuild()
        {
            _entries = _entries.OrderByDescending(ts => ts.Points)
                .ThenByDescending(ts => ts.SetsRatio)
                .ThenByDescending(ts => ts.BallsRatio)
                .ToList();

            return this;
        }
    }
}
