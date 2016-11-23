using System;
using VolleyManagement.Domain.PlayersAggregate;
using VolleyManagement.Domain.Properties;

namespace VolleyManagement.Domain.TournamentRequestAggregate
{
    /// <summary>
    /// Tournament request domain class.
    /// </summary>
    public class TournamentRequest
    {
        private int _teamId;
        private int _tournamentId;

        /// <summary>
        /// Gets or sets Id.
        /// </summary>
        /// <value>Id of tournament's request.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets user's id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets tournament's id
        /// </summary>
        public int TournamentId
        {
            get { return this._tournamentId; }

            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException(Resources.InvalidArgumentException);
                }

                this._tournamentId = value;
            }
        }

        /// <summary>
        /// Gets or sets team's id
        /// </summary>
        public int TeamId
        {
            get { return this._teamId; }

            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException(Resources.InvalidArgumentException);
                }

                this._teamId = value;
            }
        }
    }
}
