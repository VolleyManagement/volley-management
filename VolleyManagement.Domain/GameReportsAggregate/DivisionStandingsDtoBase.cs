namespace VolleyManagement.Domain.GameReportsAggregate
{
    using System;

    /// <summary>
    /// Common attributes for standings
    /// </summary>
    public abstract class DivisionStandingsDtoBase
    {
        /// <summary>
        /// Id of the division this standings belongs to
        /// </summary>
        public int DivisionId { get; set; }

        /// <summary>
        /// Name of the division this standings belongs to
        /// </summary>
        public string DivisionName { get; set; }

        /// <summary>
        /// Time when statistics has been updated last time
        /// </summary>
        public DateTime? LastUpdateTime { get; set; }
    }
}