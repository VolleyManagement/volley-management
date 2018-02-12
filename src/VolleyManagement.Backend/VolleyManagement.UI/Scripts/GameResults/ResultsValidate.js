(function () {
    var resValidateNs = VM.addNamespace("GameResult.Validate"),
        privates = {};

    resValidateNs.privates = privates;

    privates.getResults = function () {
        return {
            "GameScore": {
                "Home": parseInt($("#GameScore_Home").val()),
                "Away": parseInt($("#GameScore_Away").val())
            },
            "SetScores":
                [
                   {
                       "Home": parseInt($("#SetScores_0__Home").val()),
                       "Away": parseInt($("#SetScores_0__Away").val())
                   },
                    {
                        "Home": parseInt($("#SetScores_1__Home").val()),
                        "Away": parseInt($("#SetScores_1__Away").val())
                    },
                    {
                        "Home": parseInt($("#SetScores_2__Home").val()),
                        "Away": parseInt($("#SetScores_2__Away").val())
                    },
                    {
                        "Home": parseInt($("#SetScores_3__Home").val()),
                        "Away": parseInt($("#SetScores_3__Away").val())
                    },
                    {
                        "Home": parseInt($("#SetScores_4__Home").val()),
                        "Away": parseInt($("#SetScores_4__Away").val())
                    }
                ],
            "Teams": {
                "Home": parseInt($("#HomeTeamId").val()),
                "Away": parseInt($("#AwayTeamId").val())
            },
            "IsTechnicalDefeat": $("#IsTechnicalDefeat")[0].checked,
            "HasPenalty": $("#HasPenalty")[0].checked,
            "IsHomeTeamPenalty": $("#IsHomeTeamPenalty")[0].checked,
            "PenaltyAmount": parseInt($("#PenaltyAmount").val()),
            "PenaltyDescrition": $("#PenaltyDescrition").val()
        };
    }

    // Methods provides units of rules.

    privates.AreTheSameTeams = function (HomeId, AwayId) {
        return HomeId === AwayId;
    }

    privates.IsGameScoreValid = function (setsScore, isTechnicalDefeat) {
        return isTechnicalDefeat ? privates.IsTechnicalDefeatGameScoreValid(setsScore) : privates.IsOrdinaryGameScoreValid(setsScore);
    }

    privates.AreSetScoresMatched = function (setsScore, setScores) {
        var score = {
            "Home": 0,
            "Away": 0
        }

        for (var i = 0; i < setScores.length; i++) {
            if (setScores[i].Home > setScores[i].Away) {
                score.Home++;
            }
            else if (setScores[i].Home < setScores[i].Away) {
                score.Away++;
            }
        }

        return score.Home === setsScore.Home && score.Away === setsScore.Away;
    }

    privates.IsRequiredSetScoreValid = function (setScore, isTechnicalDefeat) {
        return isTechnicalDefeat ? privates.IsTechnicalDefeatRequiredSetScoreValid(setScore) : privates.IsOrdinaryRequiredSetScoreValid(setScore);
    }

    privates.IsOptionalSetScoreValid = function (setScore, isTechnicalDefeat, setOrderNumber) {
        return isTechnicalDefeat ? privates.IsTechnicalDefeatOptionalSetScoreValid(setScore) :
            privates.IsOrdinaryOptionalSetScoreValid(setScore, setOrderNumber);
    }

    privates.IsSetUnplayed = function (setScore) {
        return setScore.Home === gameResultConstants.UNPLAYED_SET_HOME_SCORE
            && setScore.Away === gameResultConstants.UNPLAYED_SET_AWAY_SCORE;
    }

    privates.AreSetScoresOrdered = function (setScores) {
        var hasMatchEnded = false;
        var score = {
            "Home": 0,
            "Away": 0
        }

        for (i = 0; i < setScores.length; i++) {
            if (setScores[i].Home > setScores[i].Away) {
                if (hasMatchEnded) {
                    return false;
                }

                score.Home++;

                if (score.Home === gameResultConstants.SETS_COUNT_TO_WIN) {
                    hasMatchEnded = true;
                }
            }
            else if (setScores[i].Home < setScores[i].Away) {
                if (hasMatchEnded) {
                    return false;
                }

                score.Away++;

                if (score.Away === gameResultConstants.SETS_COUNT_TO_WIN) {
                    hasMatchEnded = true;
                }
            }
        }

        return true;
    }

    privates.IsTechnicalDefeatGameScoreValid = function (setsScore) {
        return (setsScore.Home === gameResultConstants.TECHNICAL_DEFEAT_SETS_WINNER_SCORE
            && setsScore.Away === gameResultConstants.TECHNICAL_DEFEAT_SETS_LOSER_SCORE)
            || (setsScore.Home === gameResultConstants.TECHNICAL_DEFEAT_SETS_LOSER_SCORE
            && setsScore.Away === gameResultConstants.TECHNICAL_DEFEAT_SETS_WINNER_SCORE);
    }

    privates.IsOrdinaryGameScoreValid = function (setsScore) {
        return (setsScore.Home === gameResultConstants.SETS_COUNT_TO_WIN
            && setsScore.Away < gameResultConstants.SETS_COUNT_TO_WIN)
            || (setsScore.Home < gameResultConstants.SETS_COUNT_TO_WIN
            && setsScore.Away === gameResultConstants.SETS_COUNT_TO_WIN);
    }

    privates.IsTechnicalDefeatRequiredSetScoreValid = function (setScore) {
        return (setScore.Home === gameResultConstants.TECHNICAL_DEFEAT_SET_WINNER_SCORE
            && setScore.Away === gameResultConstants.TECHNICAL_DEFEAT_SET_LOSER_SCORE)
            || (setScore.Home === gameResultConstants.TECHNICAL_DEFEAT_SET_LOSER_SCORE
            && setScore.Away === gameResultConstants.TECHNICAL_DEFEAT_SET_WINNER_SCORE);
    }

    privates.IsTechnicalDefeatOptionalSetScoreValid = function (setScore) {
        return setScore.Home === gameResultConstants.TECHNICAL_DEFEAT_SET_LOSER_SCORE
            && setScore.Away === gameResultConstants.TECHNICAL_DEFEAT_SET_LOSER_SCORE;
    }

    privates.IsOrdinaryRequiredSetScoreValid = function (setScore, setOrderNumber) {
        var isValid = false;
        setOrderNumber = setOrderNumber || 0;
        if (setOrderNumber === gameResultConstants.MAX_SETS_COUNT) {
            isValid = privates.IsSetScoreValid(setScore, gameResultConstants.FIFTH_SET_POINTS_MIN_VALUE_TO_WIN);
        }
        else {
            isValid = privates.IsSetScoreValid(setScore, gameResultConstants.SET_POINTS_MIN_VALUE_TO_WIN);
        }

        return isValid;
    }

    privates.IsSetScoreValid = function (setScore, minSetScore) {
        var isValid = false;
        if (privates.IsSetScoreGreaterThanMin(setScore, minSetScore)) {
            isValid = privates.IsPointsDifferenceEqualRequired(setScore);
        }
        else if (privates.IsSetScoreEqualToMin(setScore, minSetScore)) {
            isValid = privates.IsPointsDifferenceGreaterOrEqualRequired(setScore);
        }

        return isValid;
    }

    privates.IsPointsDifferenceEqualRequired = function (setScore) {
        return Math.abs(setScore.Home - setScore.Away) == gameResultConstants.SET_POINTS_MIN_DELTA_TO_WIN;
    }

    privates.IsPointsDifferenceGreaterOrEqualRequired = function (setScore) {
        return Math.abs(setScore.Home - setScore.Away) >= gameResultConstants.SET_POINTS_MIN_DELTA_TO_WIN;
    }

    privates.IsOrdinaryOptionalSetScoreValid = function (setScore, setOrderNumber) {
        return privates.IsOrdinaryRequiredSetScoreValid(setScore, setOrderNumber) || privates.IsSetUnplayed(setScore);
    }

    privates.IsSetScoreEqualToMin = function (setScore, minSetScore) {
        return setScore.Home === minSetScore
            || setScore.Away === minSetScore;
    }

    privates.IsSetScoreGreaterThanMin = function (setScore, minSetScore) {
        return setScore.Home > minSetScore
            || setScore.Away > minSetScore;
    }

    // Methods to validate.

    privates.ValidateGameResult = function (gameResult) {
        privates.ValidateTeams(gameResult.Teams.Home, gameResult.Teams.Away);
        privates.ValidateGameScore(gameResult.GameScore, gameResult.IsTechnicalDefeat);
        privates.ValidateGameScoreMatchesSetScores(gameResult.GameScore, gameResult.SetScores);
        privates.ValidateSetScoresValues(gameResult.SetScores, gameResult.IsTechnicalDefeat);
        privates.ValidateSetScoresOrder(gameResult.SetScores);
    }

    privates.ValidateTeams = function (homeTeamId, awayTeamId) {
        if (privates.AreTheSameTeams(homeTeamId, awayTeamId)) {
            throw resourceMessages.GameResultSameTeam;
        }
    }

    privates.ValidateGameScore = function (gameScore, isTechnicalDefeat) {
        if (!privates.IsGameScoreValid(gameScore, isTechnicalDefeat)) {
            var template = jQuery.validator.format(resourceMessages.GameResultSetsScoreInvalid);
            throw template(
                gameResultConstants.TECHNICAL_DEFEAT_SETS_WINNER_SCORE,
                gameResultConstants.TECHNICAL_DEFEAT_SETS_LOSER_SCORE);
        }
    }

    privates.ValidateGameScoreMatchesSetScores = function (setsScore, setScores) {
        if (!privates.AreSetScoresMatched(setsScore, setScores)) {
            throw resourceMessages.GameResultSetsScoreNoMatchSetScores;
        }
    }

    privates.ValidateSetScoresValues = function (setScores, isTechnicalDefeat) {
        var isPreviousOptionalSetUnplayed = false;

        for (i = 0, setOrderNumber = 1; i < setScores.length; i++, setOrderNumber++) {
            if (i < gameResultConstants.SETS_COUNT_TO_WIN) {
                if (!privates.IsRequiredSetScoreValid(setScores[i], isTechnicalDefeat)) {
                    var template = jQuery.validator.format(resourceMessages.GameResultRequiredSetScores);
                    throw template(
                        gameResultConstants.SET_POINTS_MIN_VALUE_TO_WIN,
                        gameResultConstants.SET_POINTS_MIN_DELTA_TO_WIN,
                        gameResultConstants.TECHNICAL_DEFEAT_SET_WINNER_SCORE,
                        gameResultConstants.TECHNICAL_DEFEAT_SET_LOSER_SCORE);
                }
            }
            else {
                if (!privates.IsOptionalSetScoreValid(setScores[i], isTechnicalDefeat, setOrderNumber)) {
                    if (setOrderNumber == gameResultConstants.MAX_SETS_COUNT) {
                        var template = jQuery.validator.format(resourceMessages.GameResultFifthSetScoreInvalid);
                        throw template(
                            gameResultConstants.FIFTH_SET_POINTS_MIN_VALUE_TO_WIN,
                            gameResultConstants.SET_POINTS_MIN_DELTA_TO_WIN);
                    }
                    var template = jQuery.validator.format(resourceMessages.GameResultOptionalSetScores);
                    throw template(
                        gameResultConstants.SET_POINTS_MIN_VALUE_TO_WIN,
                        gameResultConstants.SET_POINTS_MIN_DELTA_TO_WIN,
                        gameResultConstants.TECHNICAL_DEFEAT_SET_LOSER_SCORE,
                        gameResultConstants.TECHNICAL_DEFEAT_SET_LOSER_SCORE);
                }

                if (isPreviousOptionalSetUnplayed) {
                    if (!privates.IsSetUnplayed(setScores[i])) {
                        var template = jQuery.validator.format(resourceMessages.GameResultPreviousOptionalSetUnplayed);
                        throw resourceMessages.GameResultPreviousOptionalSetUnplayed;
                    }
                }

                isPreviousOptionalSetUnplayed = privates.IsSetUnplayed(setScores[i]);
            }
        }
    }

    privates.ValidateSetScoresOrder = function (setScores) {
        if (!privates.AreSetScoresOrdered(setScores)) {
            throw resourceMessages.GameResultSetScoresNotOrdered;
        }
    }

    $("#createForm").submit(function (e) {
        e.preventDefault();
    }).validate({
        submitHandler: function (form) {
            var gameResult;

            try {
                gameResult = privates.getResults();
                privates.ValidateGameResult(gameResult);
                form.submit();
            }
            catch (message) {
                errors = { ValidationMessage: message };
                this.showErrors(errors);
                return false;
            }
        }
    });

    $(document).ready(function () {
        privates.formLoaded = $("#createForm").serialize();
    });

    $("#backToSchedule").click(function () {
        var formChanged = $("#createForm").serialize();
        if (privates.formLoaded !== formChanged) {
            return confirm("You've made changes. Do you want to discard them?");
        }
    });
})();
