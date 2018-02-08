(function(This) {
    'use strict';
    This.TeamCollection = function() {
        var data = [new This.Team(1, 1, 'Союз', 25, 2.545),
            new This.Team(2, 2, 'РБ', 24, 3.00),
            new This.Team(3, 3, 'Люди в черном', 23, 2.077),
            new This.Team(4, 4, 'Синергия', 18, 1.294),
            new This.Team(5, 5, 'Red Stone', 13, 0.900)
        ];

        this.getData = function() {
            return data;
        };

        return this;
    };
})(scores);
