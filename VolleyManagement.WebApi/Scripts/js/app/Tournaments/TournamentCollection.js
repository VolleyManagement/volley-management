"use strict";

(function (This) {
    This.TournamentCollection = Backbone.Collection.extend({
        url: '/OData/Tournaments',

        model: This.Tournament
    });

})(App.Tournaments);
