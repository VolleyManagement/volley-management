namespace VolleyManagement.Domain.TournamentRequestAggregate
{
    using System;
    using Properties;

    /// <summary>
    /// Tournament request domain class.
    /// </summary>
    public class TournamentRequest
    {
        private int _userId;
        private int _teamId;
        private int _tournamentId;
        private int _groupId;
        private int _divisionId;

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

                _userId = value;
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

                _tournamentId = value;
            }
        }

        /// <summary>
        /// Gets or sets division's id
        /// </summary>
        public int DivisionId
        {
            get
            {
                return _divisionId;
            }

            set
            {
                if (TournamentRequestValidation.ValidateDivisionId(value))
                {
                    throw new ArgumentException(Resources.ValidationDivisionId);
                }

                _divisionId = value;
            }
        }

        /// <summary>
        /// Gets or sets team's id
        /// </summary>
        public int TeamId
        {
            get
            {
                return _teamId;
            }

            set
            {
                if (TournamentRequestValidation.ValidateTeamId(value))
                {
                    throw new ArgumentException(Resources.ValidationTeamId);
                }

                _teamId = value;
            }
        }

        /// <summary>
        /// Gets or sets group's id
        /// </summary>
        public int GroupId
        {
            get
            {
                return _groupId;
            }

            set
            {
                if (TournamentRequestValidation.ValidateTeamId(value))
                {
                    throw new ArgumentException(Resources.ValidationGroupId);
                }

                _groupId = value;
            }
        }
    }
}
