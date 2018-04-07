using System.Collections.Generic;

namespace VolleyManagement.Domain.TeamsAggregate
{
    using System.Text.RegularExpressions;
    using System.Linq;

    /// <summary>
    /// Team validation class.
    /// </summary>
    public static class TeamValidation
    {
        /// <summary>
        /// Validates team id.
        /// </summary>
        /// <param name="id">Team id for validation</param>
        /// <returns>Validity of team id</returns>
        public static bool ValidateTeamId(int id)
        {
            return id < Constants.Team.MIN_ID;
        }

        /// <summary>
        /// Validates team name.
        /// </summary>
        /// <param name="teamName">Team name for validation</param>
        /// <returns>Validity of team name</returns>
        public static bool ValidateTeamName(string teamName)
        {
            return string.IsNullOrEmpty(teamName) || teamName.Length > Constants.Team.MAX_NAME_LENGTH;
        }

        /// <summary>
        /// Validates coach name.
        /// </summary>
        /// <param name="coachName">Coach name for validation</param>
        /// <returns>Validity of coach name</returns>
        public static bool ValidateCoachName(string coachName)
        {
            return !Regex.IsMatch(coachName, Constants.Team.COACH_NAME_VALIDATION_REGEX)
                || coachName.Length > Constants.Team.MAX_COACH_NAME_LENGTH;
        }

        /// <summary>
        /// Validates achievements.
        /// </summary>
        /// <param name="achievements">Achievements for validation</param>
        /// <returns>Validity of achievements</returns>
        public static bool ValidateAchievements(string achievements)
        {
            return achievements.Length > Constants.Team.MAX_ACHIEVEMENTS_LENGTH;
        }

        /// <summary>
        /// Validates captain id.
        /// </summary>
        /// <param name="captainId">Captain id for validation</param>
        /// <returns>Validity of captain id</returns>
        public static bool ValidateCaptainId(PlayerId captainId)
        {
            return captainId == null || captainId.Id < Constants.Team.MIN_ID;
        }

        /// <summary>
        /// Validates team roster if it null, 
        /// if elements ids are less, than required,
        /// if not all values are unique.
        /// </summary>
        /// <param name="roster">List of team members ids.</param>
        /// <returns>true if roster is invalid</returns>
        public static bool ValidateTeamRoster(IEnumerable<PlayerId> roster)
        {
            return roster == null ||
                roster.Any(x => x == null) ||
                roster.Any(x => x.Id < Constants.Team.MIN_ID) ||
                roster.Count() != roster.Select(x => x.Id).Distinct().Count();
        }
    }
}
