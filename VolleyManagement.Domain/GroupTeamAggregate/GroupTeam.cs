namespace VolleyManagement.Domain.GroupTeamAggregate
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class GroupTeam
    {
        /// <summary>
        /// Gets or sets a value of TeamId.
        /// </summary>
        /// <value>TeamId of team.</value>
        public int TeamId { get; set; }

        /// <summary>
        /// Gets or sets a value of GroupId.
        /// </summary>
        /// <value>GroupId of group.</value>
        public int GroupId { get; set; }

        /// <summary>
        /// Gets or sets a value of TournamentId.
        /// </summary>
        /// <value>TournamentId of current tournament.</value>
        public int TournamentId { get; set; }
    }
}
