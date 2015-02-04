"use strict";

(function (This, scope)  {
    var mediator;

    function extractMediator () {
        mediator = scope.mediator;
    }

    This.TournamentRouter = Backbone.Router.extend({
        routes: {
            '': 'allTournaments',
            'Tournaments': 'allTournaments',
            'Tournaments/new': 'create',
            'Tournaments/:id/edit': 'edit',
            'Tournaments/:id': 'oneTournament'
        },

        initialize: function () {
            extractMediator();

            this.controller = new App.Tournaments.Controller(this);
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
})(App.Tournaments, vm.tournaments);