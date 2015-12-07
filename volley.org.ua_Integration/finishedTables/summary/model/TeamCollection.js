(function (This) {
    'use strict';
    This.TeamCollection = function() {
    var teams = [new This.Team({
        place: 1,
        teamName: 'Союз',
        points: 25,
        gameAll: 10,
        gameWin: 8,
        gameLost: 2,
        game30: 4,
        game31: 3,
        game32: 1,
        game23: 2,
        game13: 0,
        game03: 0,
        setsWin: 28,
        setsLost: 11,
        setsKoef: 2.545,
        ballsWin: 785,
        ballsLost: 685,
        ballsKoef: 1.146
    }), new This.Team({
        place: 2,
        teamName: 'РБ',
        points: 24,
        gameAll: 10,
        gameWin: 8,
        gameLost: 2,
        game30: 6,
        game31: 2,
        game32: 0,
        game23: 0,
        game13: 0,
        game03: 2,
        setsWin: 24,
        setsLost: 8,
        setsKoef: 3,
        ballsWin: 760,
        ballsLost: 626,
        ballsKoef: 1.214
    }), new This.Team({
        place: 3,
        teamName: 'Люди в черном',
        points: 23,
        gameAll: 10,
        gameWin: 8,
        gameLost: 2,
        game30: 3,
        game31: 3,
        game32: 2,
        game23: 1,
        game13: 1,
        game03: 0,
        setsWin: 27,
        setsLost: 13,
        setsKoef: 2.077,
        ballsWin: 806,
        ballsLost: 732,
        ballsKoef: 1.101
    }) ];

    this.getData = function() {
        return teams;
    };

    return this;
};

})(summary);