(function(This) {
    'use strict';
    This.Team = function(id, place, name, score, avgScore) {
        return {
            id: id,
            place: place,
            name: name,
            score: score,
            avgScore: avgScore
        };
    };
})(scores);
