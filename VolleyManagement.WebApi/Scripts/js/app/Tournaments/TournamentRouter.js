"use strict";

(function (This)  {
    This.TournamentRouter = Backbone.Router.extend({
            routes: {
                '': 'allTournaments',
                'Tournaments': 'allTournaments',
                'Tournaments/new': 'create',
                'Tournaments/edit/:id': 'edit',
                'Tournaments/:id': 'oneTournament'
            },

            initialize: function () {
                this.controller = new App.Tournaments.Controller();
            },

            allTournaments: function () {
                mediator.publish('ShowTournaments');
            },

            oneTournament: function (id) {
                mediator.publish('ShowTournamentById', id);
            },

            create: function () {
                mediator.publish('CreateTournament');
            },

            edit: function (id) {
                mediator.publish('EditTournamentById', id);
            }
    });
})(App.Tournaments);