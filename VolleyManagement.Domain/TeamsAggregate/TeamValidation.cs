namespace VolleyManagement.Domain.TeamsAggregate
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Team validation class.
    /// </summary>
    public class TeamValidation
    {
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
        /// Validates team name with other team names.
        /// </summary>
        /// <param name="existTeams">List of all Teams in DB</param>
        /// <param name="name">Name of Team, that is creating</param>
        /// <returns>Validity of achievements</returns>
        public static bool ValidateTwoTeamsWithTheSameName(List<Team> existTeams, string name)
        {
            return existTeams.Where(t => t.Name.ToLower().Equals(name.ToLower())).Any();
        }
    }
}