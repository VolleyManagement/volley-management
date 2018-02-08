(function(This) {
    'use strict';
    This.Team = function(options) {

        return {
            place: options.place,
            teamName: options.teamName,
            points: options.points,
            game: {
                all: options.gameAll,
                win: options.gameWin,
                lost: options.gameLost,
                game30: options.game30,
                game31: options.game31,
                game32: options.game32,
                game23: options.game23,
                game13: options.game13,
                game03: options.game03
            },
            sets: {
                win: options.setsWin,
                lost: options.setsLost,
                koef: options.setsKoef
            },
            balls: {
                win: options.ballsWin,
                lost: options.ballsLost,
                koef: options.ballsKoef
            }
        };
    };
})(summary);
