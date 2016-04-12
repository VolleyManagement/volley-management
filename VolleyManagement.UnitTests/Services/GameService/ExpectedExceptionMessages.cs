namespace VolleyManagement.UnitTests.Services.GameService
{
    /// <summary>
    /// Container for expected exception messages.
    /// </summary>
    public static class ExpectedExceptionMessages
    {
        public const string GAME = "Value cannot be null.\r\nParameter name: game";

        public const string GAME_SAME_TEAM = "The team can not play with itself";

        public const string GAME_SETS_SCORE_INVALID =
            "The set score can be one of the following: 3:0, 3:1, 3:2, 2:3, 1:3, 0:3." +
            " In case of a technical defeat set score must be 3:0 or 0:3";

        public const string GAME_SETS_SCORE_NOMATCH_SET_SCORES =
            "Game score does not match set scores";

        public const string GAME_REQUIRED_SET_SCORES_0_0 =
            "In the set, the number of points for one team must be at least 25 "
            + "and the points difference should be at least 2. "
            + "If the score exceeds 25, the points difference must be equal to 2. "
            + "In case of a technical defeat score of an optional game should be 0: 0";

        public const string GAME_REQUIRED_SET_SCORES_25_0 =
            "In the set, the number of points for one team must be at least 25 "
            + "and the points difference should be at least 2. "
            + "If the score exceeds 25, the points difference must be equal to 2. "
            + "In case of a technical defeat score of an optional game should be 25: 0 or 0: 25";

        public const string GAME_PREVIOUS_OPTIONAL_SET_UNPLAYED =
        "Enter a score of the previous optional game";

        public const string CONCURRENCY_EXCEPTION = "Игра с указаным идентификатором не была найдена";

        public const string GAME_SET_SCORES_NOT_ORDERED = "The set scores are listed in the wrong order";
    }
}