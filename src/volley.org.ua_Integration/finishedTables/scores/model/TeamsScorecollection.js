(function(This) {
    'use strict';
    This.TeamsScoreCollection = function() {
        var data = [new This.TeamScore(1, 2, 3, 1),
            new This.TeamScore(1, 3, 3, 1),
            new This.TeamScore(2, 3, 0, 3),
            new This.TeamScore(1, 4, 3, 0),
            new This.TeamScore(1, 5, 3, 2),
            new This.TeamScore(2, 4, 2, 3),
            new This.TeamScore(2, 5, 0, 3)
        ];

        this.getData = function() {
            return data;
        };

        return this;
    };
})(scores);
