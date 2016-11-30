namespace VolleyManagement.Domain.TournamentRequestAggregate
{
    using System;
    using VolleyManagement.Domain.Properties;

    /// <summary>
    /// Tournament request domain class.
    /// </summary>
    public class TournamentRequest
    {
        private int _userId;
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
        public int UserId
        {
            get
            {
                return _userId;
            }

            set
            {
                if (TournamentRequestValidation.ValidateUserId(value))
                {
                    throw new ArgumentException(Resources.ValidationUserId);
                }

                this._userId = value;
            }
        }

        /// <summary>
        /// Gets or sets tournament's id
        /// </summary>
        public int TournamentId
        {
            get
            {
                return _tournamentId;
            }

            set
            {
                if (TournamentRequestValidation.ValidateTournamentId(value))
                {
                    throw new ArgumentException(Resources.ValidationTournamentId);
                }

                this._tournamentId = value;
            }
        }

        /// <summary>
        /// Gets or sets team's id
        /// </summary>
        public int TeamId
        {
            get
            {
                return this._teamId;
            }

            set
            {
                if (TournamentRequestValidation.ValidateTeamId(value))
                {
                    throw new ArgumentException(Resources.ValidationTeamId);
                }

                this._teamId = value;
            }
        }
    }
}
