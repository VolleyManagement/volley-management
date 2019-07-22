﻿namespace VolleyManagement.Domain.TournamentRequestAggregate
{
    /// <summary>
    /// Tournament request validation class
    /// </summary>
    public static class TournamentRequestValidation
    {
        /// <summary>
        /// Validates user id.
        /// </summary>
        /// <param name="userId">User for validation</param>
        /// <returns>Validity of User</returns>
        public static bool ValidateUserId(int userId)
        {
            return userId <= 0;
        }

        /// <summary>
        /// Validates team id.
        /// </summary>
        /// <param name="teamId">Team for validation</param>
        /// <returns>Validity of Team</returns>
        public static bool ValidateTeamId(int teamId)
        {
            return teamId <= 0;
        }

        /// <summary>
        /// Validates tournament id.
        /// </summary>
        /// <param name="tournamentId">Tournament for validation</param>
        /// <returns>Validity of Tournament</returns>
        public static bool ValidateTournamentId(int tournamentId)
        {
            return tournamentId <= 0;
        }

        /// <summary>
        /// Validates group id.
        /// </summary>
        /// <param name="groupId">Group for validation</param>
        /// <returns>Validity of Group</returns>
        public static bool ValidateGroupId(int groupId)
        {
            return groupId <= 0;
        }

        /// <summary>
        /// Validates division id.
        /// </summary>
        /// <param name="divisionId">Group for validation</param>
        /// <returns>Validity of Division</returns>
        public static bool ValidateDivisionId(int divisionId)
        {
            return divisionId <= 0;
        }
    }
}
