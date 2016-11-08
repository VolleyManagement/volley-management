namespace VolleyManagement.Domain.RequestsAggregate
{
    using System.Text.RegularExpressions;

    /// <summary>
    /// Request validation class.
    /// </summary>
    public static class RequestValidation
    {
        /// <summary>
        /// Validates user id.
        /// </summary>
        /// <param name="userId">User for validation</param>
        /// <returns> Validity of User</returns>
        public static bool ValidateUserId(int userId)
        {
            return userId == 0;
        }

        /// <summary>
        /// Validates player id.
        /// </summary>
        /// <param name="playerId">PLayer for validation</param>
        /// <returns>Validity of player</returns>
        public static bool ValidatePlayerId(int playerId)
        {
            return playerId == 0;
        }
    }
}
