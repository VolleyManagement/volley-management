namespace VolleyManagement.Domain.TournamentsAggregate
{
    /// <summary>
    /// Describes scheduling info for a particular division
    /// </summary>
    public class DivisionScheduleDto
    {
        public int DivisionId { get; set; }

        public string DivisionName { get; set; }

        /// <summary>
        /// Number of teams playing in division
        /// </summary>
        public int TeamCount { get; set; }
    }
}