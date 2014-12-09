namespace VolleyManagement.Domain.Tournaments
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Enumeration for tournament scheme
    /// </summary>
    public enum TournamentSchemeEnum
    {
        /// <summary>
        /// Scheme 1
        /// </summary>
        [Description("1")]
        One = 1,

        /// <summary>
        /// Scheme 2
        /// </summary>
        [Description("2")]
        Two,

        /// <summary>
        /// Scheme 2.5
        /// </summary>
        [Description("2.5")]
        TwoAndHalf
    }
}
