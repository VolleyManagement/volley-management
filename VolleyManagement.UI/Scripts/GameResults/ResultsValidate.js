﻿function getResults() {
    return {
        "SetsScore": {
            "Home": $("#SetsScore_Home").val(),
            "Away": $("#SetsScore_Away").val()
        },
        "SetScores":
            [
               {
                   "Home": $("#SetScores_0__Home").val(),
                   "Away": $("#SetScores_0__Away").val()
               },
                {
                    "Home": $("#SetScores_1__Home").val(),
                    "Away": $("#SetScores_1__Away").val()
                },
                {
                    "Home": $("#SetScores_2__Home").val(),
                    "Away": $("#SetScores_2__Away").val()
                },
                {
                    "Home": $("#SetScores_3__Home").val(),
                    "Away": $("#SetScores_3__Away").val()
                },
                {
                    "Home": $("#SetScores_4__Home").val(),
                    "Away": $("#SetScores_4__Away").val()
                }
            ],
        "Teams": {
            "Home": $("#HomeTeamId").val(),
            "Away": $("#AwayTeamId").val()
        },
        "IsTechnicalDefeat": $("#IsTechnicalDefeat")[0].checked
    };
}


// Methods provides units of rules.

function AreTheSameTeams(HomeId, AwayId) {
    return HomeId == AwayId;
}

function AreSetScoresMatched(setsScore, setScores) {
    var score = {
        "Home": 0,
        "Away": 0
    }

    for (i = 0; i < setScores.length; i++) {
        if (setScores[i].Home > setScores[i].Away) {
            score.Home++;
        }
        else if (setScores[i].Home < setScores[i].Away) {
            score.Away++;
        }
    }

    return score.Home == setsScore.Home && score.Away == setsScore.Away;
}

function IsSetsScoreValid(setsScore, isTechnicalDefeat) {
    return isTechnicalDefeat ? IsTechnicalDefeatSetsScoreValid(setsScore) : IsOrdinarySetsScoreValid(setsScore);
}

function IsRequiredSetScoreValid(setScore, isTechnicalDefeat) {
    return isTechnicalDefeat ? IsTechnicalDefeatRequiredSetScoreValid(setScore) : IsOrdinaryRequiredSetScoreValid(setScore);
}

function IsOptionalSetScoreValid(setScore, isTechnicalDefeat) {
    return isTechnicalDefeat ? IsTechnicalDefeatOptionalSetScoreValid(setScore) : IsOrdinaryOptionalSetScoreValid(setScore);
}

function IsTechnicalDefeatSetsScoreValid(setsScore) {
    return (setsScore.Home == gameResultConstants.TECHNICAL_DEFEAT_SETS_WINNER_SCORE
        && setsScore.Away == gameResultConstants.TECHNICAL_DEFEAT_SETS_LOSER_SCORE)
        || (setsScore.Home == gameResultConstants.TECHNICAL_DEFEAT_SETS_LOSER_SCORE
        && setsScore.Away == gameResultConstants.TECHNICAL_DEFEAT_SETS_WINNER_SCORE);
}

function IsOrdinarySetsScoreValid(setsScore) {
    return +setsScore.Home + +setsScore.Away >= gameResultConstants.MIN_SETS_COUNT
        && +setsScore.Home + +setsScore.Away <= gameResultConstants.MAX_SETS_COUNT;
}

function IsTechnicalDefeatRequiredSetScoreValid(setScore) {
    return (setScore.Home == gameResultConstants.TECHNICAL_DEFEAT_SET_WINNER_SCORE
        && setScore.Away == gameResultConstants.TECHNICAL_DEFEAT_SET_LOSER_SCORE)
        || (setScore.Home == gameResultConstants.TECHNICAL_DEFEAT_SET_LOSER_SCORE
        && setScore.Away == gameResultConstants.TECHNICAL_DEFEAT_SET_WINNER_SCORE);
}

function IsTechnicalDefeatOptionalSetScoreValid(setScore) {
    return setScore.Home == gameResultConstants.TECHNICAL_DEFEAT_SET_LOSER_SCORE
        && setScore.Away == gameResultConstants.TECHNICAL_DEFEAT_SET_LOSER_SCORE;
}

function IsOrdinaryRequiredSetScoreValid(setScore) {
    var isValid = false;

    if (IsSetScoreGreaterThanMin(setScore)) {
        isValid = Math.abs(setScore.Home - setScore.Away) == gameResultConstants.SET_POINTS_MIN_DELTA_TO_WIN;
    }
    else if (IsSetScoreEqualToMin(setScore)) {
        isValid = Math.abs(setScore.Home - setScore.Away) >= gameResultConstants.SET_POINTS_MIN_DELTA_TO_WIN;
    }

    return isValid;
}

function IsOrdinaryOptionalSetScoreValid(setScore) {
    return IsOrdinaryRequiredSetScoreValid(setScore) || IsSetUnplayed(setScore);
}

function IsSetScoreEqualToMin(setScore) {
    return setScore.Home == gameResultConstants.SET_POINTS_MIN_VALUE_TO_WIN
        || setScore.Away == gameResultConstants.SET_POINTS_MIN_VALUE_TO_WIN;
}

function IsSetScoreGreaterThanMin(setScore) {
    return setScore.Home > gameResultConstants.SET_POINTS_MIN_VALUE_TO_WIN
        || setScore.Away > gameResultConstants.SET_POINTS_MIN_VALUE_TO_WIN;
}

function IsSetUnplayed(setScore) {
    return setScore.Home == gameResultConstants.UNPLAYED_SET_HOME_SCORE
        && setScore.Away == gameResultConstants.UNPLAYED_SET_AWAY_SCORE;
}

// Methods to validate.

function ValidateGameResult(gameResult) {
    ValidateTeams(gameResult.Teams.Home, gameResult.Teams.Away);
    ValidateSetsScoreMatchesSetScores(gameResult.SetsScore, gameResult.SetScores);
    ValidateSetsScore(gameResult.SetsScore, gameResult.IsTechnicalDefeat);
    ValidateSetScores(gameResult.SetScores, gameResult.IsTechnicalDefeat);
}

function ValidateTeams(homeTeamId, awayTeamId) {
    if (AreTheSameTeams(homeTeamId, awayTeamId)) {
        throw resourceMessages.GameResultSameTeam;
    }
}

function ValidateSetsScoreMatchesSetScores(setsScore, setScores) {
    if (!AreSetScoresMatched(setsScore, setScores)) {
        throw resourceMessages.GameResultSetsScoreNoMatchSetScores;
    }
}

function ValidateSetsScore(setsScore, isTechnicalDefeat) {
    if (!IsSetsScoreValid(setsScore, isTechnicalDefeat)) {
        var template = jQuery.validator.format(resourceMessages.GameResultSetsScoreInvalid);
        throw template(
        gameResultConstants.MIN_SETS_COUNT,
        gameResultConstants.MAX_SETS_COUNT,
        gameResultConstants.TECHNICAL_DEFEAT_SETS_WINNER_SCORE,
        gameResultConstants.TECHNICAL_DEFEAT_SETS_LOSER_SCORE);
    }
}

function ValidateSetScores(setScores, isTechnicalDefeat) {
    for (i = 0; i < setScores.length; i++) {
        if (i < gameResultConstants.MIN_SETS_COUNT) {
            if (!IsRequiredSetScoreValid(setScores[i], isTechnicalDefeat)) {
                var template = jQuery.validator.format(resourceMessages.GameResultRequiredSetScores);
                throw template(
                    gameResultConstants.SET_POINTS_MIN_VALUE_TO_WIN,
                    gameResultConstants.SET_POINTS_MIN_DELTA_TO_WIN,
                    gameResultConstants.TECHNICAL_DEFEAT_SET_WINNER_SCORE,
                    gameResultConstants.TECHNICAL_DEFEAT_SET_LOSER_SCORE);
            }
        }
        else {
            if (!IsOptionalSetScoreValid(setScores[i], isTechnicalDefeat)) {
                var template = jQuery.validator.format(resourceMessages.GameResultOptionalSetScores);
                throw template(
                    gameResultConstants.SET_POINTS_MIN_VALUE_TO_WIN,
                    gameResultConstants.SET_POINTS_MIN_DELTA_TO_WIN,
                    gameResultConstants.TECHNICAL_DEFEAT_SET_LOSER_SCORE,
                    gameResultConstants.TECHNICAL_DEFEAT_SET_LOSER_SCORE);
            }
        }
    }
}

$("#createForm").submit(function (e) {
   e.preventDefault();
}).validate({
    submitHandler: function (form) {
        try {
            var gameResult = getResults();
            ValidateGameResult(gameResult);
            form.submit();
        }
        catch (message) {
            errors = { ValidationMessage: message };
            this.showErrors(errors);
            return false;
        }
    }
});