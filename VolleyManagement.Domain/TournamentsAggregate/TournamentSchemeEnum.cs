namespace VolleyManagement.Domain.TournamentsAggregate
{
    using System.ComponentModel;

    /// <summary>
    /// Enumeration for tournament scheme
    /// </summary>
    public enum TournamentSchemeEnum
    {
        [Description("1")]
        One = 1,
        [Description("2")]
        Two,
        [Description("2.5")]
        TwoAndHalf
    }
}
