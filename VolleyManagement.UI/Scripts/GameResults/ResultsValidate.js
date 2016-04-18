(function () {
    var resValidateNs = VM.addNamespace("GameResult.Validate"),
        priv = {},
        formLoaded;

    resValidateNs.priv = priv;
    priv.formLoaded = formLoaded;

    priv.getResults = function () {
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

    priv.AreTheSameTeams = function (HomeId, AwayId) {
        return HomeId == AwayId;
    }

    priv.IsSetsScoreValid = function (setsScore, isTechnicalDefeat) {
        return isTechnicalDefeat ? priv.IsTechnicalDefeatSetsScoreValid(setsScore) : priv.IsOrdinarySetsScoreValid(setsScore);
    }

    priv.AreSetScoresMatched = function (setsScore, setScores) {
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

    priv.IsRequiredSetScoreValid = function (setScore, isTechnicalDefeat) {
        return isTechnicalDefeat ? priv.IsTechnicalDefeatRequiredSetScoreValid(setScore) : priv.IsOrdinaryRequiredSetScoreValid(setScore);
    }

    priv.IsOptionalSetScoreValid = function (setScore, isTechnicalDefeat) {
        return isTechnicalDefeat ? priv.IsTechnicalDefeatOptionalSetScoreValid(setScore) : priv.IsOrdinaryOptionalSetScoreValid(setScore);
    }

    priv.IsSetUnplayed = function (setScore) {
        return setScore.Home == gameResultConstants.UNPLAYED_SET_HOME_SCORE
            && setScore.Away == gameResultConstants.UNPLAYED_SET_AWAY_SCORE;
    }

    priv.AreSetScoresOrdered = function (setScores) {
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

                if (score.Home == gameResultConstants.SETS_COUNT_TO_WIN) {
                    hasMatchEnded = true;
                }
            }
            else if (setScores[i].Home < setScores[i].Away) {
                if (hasMatchEnded) {
                    return false;
                }

                score.Away++;

                if (score.Away == gameResultConstants.SETS_COUNT_TO_WIN) {
                    hasMatchEnded = true;
                }
            }
        }

        return true;
    }

    priv.IsTechnicalDefeatSetsScoreValid = function (setsScore) {
        return (setsScore.Home == gameResultConstants.TECHNICAL_DEFEAT_SETS_WINNER_SCORE
            && setsScore.Away == gameResultConstants.TECHNICAL_DEFEAT_SETS_LOSER_SCORE)
            || (setsScore.Home == gameResultConstants.TECHNICAL_DEFEAT_SETS_LOSER_SCORE
            && setsScore.Away == gameResultConstants.TECHNICAL_DEFEAT_SETS_WINNER_SCORE);
    }

    priv.IsOrdinarySetsScoreValid = function (setsScore) {
        return (setsScore.Home == gameResultConstants.SETS_COUNT_TO_WIN
            && setsScore.Away < gameResultConstants.SETS_COUNT_TO_WIN)
            || (setsScore.Home < gameResultConstants.SETS_COUNT_TO_WIN
            && setsScore.Away == gameResultConstants.SETS_COUNT_TO_WIN);
    }

    priv.IsTechnicalDefeatRequiredSetScoreValid = function (setScore) {
        return (setScore.Home == gameResultConstants.TECHNICAL_DEFEAT_SET_WINNER_SCORE
            && setScore.Away == gameResultConstants.TECHNICAL_DEFEAT_SET_LOSER_SCORE)
            || (setScore.Home == gameResultConstants.TECHNICAL_DEFEAT_SET_LOSER_SCORE
            && setScore.Away == gameResultConstants.TECHNICAL_DEFEAT_SET_WINNER_SCORE);
    }

    priv.IsTechnicalDefeatOptionalSetScoreValid = function (setScore) {
        return setScore.Home == gameResultConstants.TECHNICAL_DEFEAT_SET_LOSER_SCORE
            && setScore.Away == gameResultConstants.TECHNICAL_DEFEAT_SET_LOSER_SCORE;
    }

    priv.IsOrdinaryRequiredSetScoreValid = function (setScore) {
        var isValid = false;

        if (priv.IsSetScoreGreaterThanMin(setScore)) {
            isValid = Math.abs(setScore.Home - setScore.Away) == gameResultConstants.SET_POINTS_MIN_DELTA_TO_WIN;
        }
        else if (priv.IsSetScoreEqualToMin(setScore)) {
            isValid = Math.abs(setScore.Home - setScore.Away) >= gameResultConstants.SET_POINTS_MIN_DELTA_TO_WIN;
        }

        return isValid;
    }

    priv.IsOrdinaryOptionalSetScoreValid = function (setScore) {
        return priv.IsOrdinaryRequiredSetScoreValid(setScore) || priv.IsSetUnplayed(setScore);
    }

    priv.IsSetScoreEqualToMin = function (setScore) {
        return setScore.Home == gameResultConstants.SET_POINTS_MIN_VALUE_TO_WIN
            || setScore.Away == gameResultConstants.SET_POINTS_MIN_VALUE_TO_WIN;
    }

    priv.IsSetScoreGreaterThanMin = function (setScore) {
        return setScore.Home > gameResultConstants.SET_POINTS_MIN_VALUE_TO_WIN
            || setScore.Away > gameResultConstants.SET_POINTS_MIN_VALUE_TO_WIN;
    }

    // Methods to validate.

    priv.ValidateGameResult = function (gameResult) {
        priv.ValidateTeams(gameResult.Teams.Home, gameResult.Teams.Away);
        priv.ValidateSetsScore(gameResult.SetsScore, gameResult.IsTechnicalDefeat);
        priv.ValidateSetsScoreMatchesSetScores(gameResult.SetsScore, gameResult.SetScores);
        priv.ValidateSetScoresValues(gameResult.SetScores, gameResult.IsTechnicalDefeat);
        priv.ValidateSetScoresOrder(gameResult.SetScores);
    }

    priv.ValidateTeams = function (homeTeamId, awayTeamId) {
        if (priv.AreTheSameTeams(homeTeamId, awayTeamId)) {
            throw resourceMessages.GameResultSameTeam;
        }
    }

    priv.ValidateSetsScore = function (setsScore, isTechnicalDefeat) {
        if (!priv.IsSetsScoreValid(setsScore, isTechnicalDefeat)) {
            var template = jQuery.validator.format(resourceMessages.GameResultSetsScoreInvalid);
            throw template(
                gameResultConstants.TECHNICAL_DEFEAT_SETS_WINNER_SCORE,
                gameResultConstants.TECHNICAL_DEFEAT_SETS_LOSER_SCORE);
        }
    }

    priv.ValidateSetsScoreMatchesSetScores = function (setsScore, setScores) {
        if (!priv.AreSetScoresMatched(setsScore, setScores)) {
            throw resourceMessages.GameResultSetsScoreNoMatchSetScores;
        }
    }

    priv.ValidateSetScoresValues = function (setScores, isTechnicalDefeat) {
        var isPreviousOptionalSetUnplayed = false;

        for (i = 0; i < setScores.length; i++) {
            if (i < gameResultConstants.SETS_COUNT_TO_WIN) {
                if (!priv.IsRequiredSetScoreValid(setScores[i], isTechnicalDefeat)) {
                    var template = jQuery.validator.format(resourceMessages.GameResultRequiredSetScores);
                    throw template(
                        gameResultConstants.SET_POINTS_MIN_VALUE_TO_WIN,
                        gameResultConstants.SET_POINTS_MIN_DELTA_TO_WIN,
                        gameResultConstants.TECHNICAL_DEFEAT_SET_WINNER_SCORE,
                        gameResultConstants.TECHNICAL_DEFEAT_SET_LOSER_SCORE);
                }
            }
            else {
                if (!priv.IsOptionalSetScoreValid(setScores[i], isTechnicalDefeat)) {
                    var template = jQuery.validator.format(resourceMessages.GameResultOptionalSetScores);
                    throw template(
                        gameResultConstants.SET_POINTS_MIN_VALUE_TO_WIN,
                        gameResultConstants.SET_POINTS_MIN_DELTA_TO_WIN,
                        gameResultConstants.TECHNICAL_DEFEAT_SET_LOSER_SCORE,
                        gameResultConstants.TECHNICAL_DEFEAT_SET_LOSER_SCORE);
                }

                if (isPreviousOptionalSetUnplayed) {
                    if (!priv.IsSetUnplayed(setScores[i])) {
                        var template = jQuery.validator.format(resourceMessages.GameResultPreviousOptionalSetUnplayed);
                        throw resourceMessages.GameResultPreviousOptionalSetUnplayed;
                    }
                }

                isPreviousOptionalSetUnplayed = priv.IsSetUnplayed(setScores[i]);
            }
        }
    }

    priv.ValidateSetScoresOrder = function (setScores) {
        if (!priv.AreSetScoresOrdered(setScores)) {
            throw resourceMessages.GameResultSetScoresNotOrdered;
        }
    }

    $("#createForm").submit(function (e) {
        e.preventDefault();
    }).validate({
        submitHandler: function (form) {
            var gameResult;

            try {
                gameResult = priv.getResults();
                priv.ValidateGameResult(gameResult);
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
        priv.formLoaded = $("#createForm").serialize();
    });

    $("#backToSchedule").click(function () {
        var formChanged = $("#createForm").serialize();
        if (priv.formLoaded != formChanged) {
            return confirm("You've made changes. Do you want to discard them?");
        }
    });

})();