(function(This) {
    This.TeamScore = function(firstTeamId, secondTeamId, firstScore, secondScore) {
        return {
            firstTeam: firstTeamId,
            secondTeam: secondTeamId,
            firstScore: firstScore,
            secondScore: secondScore
        };
    };
})(scores);
