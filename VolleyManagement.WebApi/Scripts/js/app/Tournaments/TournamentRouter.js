'use strict';

(function (This)  {
    This.Router = Backbone.Router.extend({
        routes: {
            'Tournaments': 'getTournaments',
            'Tournaments/new': 'createTournament',
            'Tournaments/:id/edit': 'editTournament',
            'Tournaments/:id': 'getTournament',
            'Tournaments*path': 'notFound'
        },

        initialize: function () {
            this.controller = new App.Tournaments.Controller(this);

            Backbone.history.loadUrl(Backbone.history.fragment);
        },

        getTournaments: function () {
            vm.mediator.publish('ShowTournaments');
        },

        createTournament: function () {
            vm.mediator.publish('CreateTournament');
        },

        editTournament: function (id) {
            vm.mediator.publish('EditTournamentById', id);
        },
        getTournament: function (id) {
            vm.mediator.publish('ShowTournamentById', id);
        },
        notFound: function (path) {
            console.log('404 not found $s', path);
        }
    });
})(App.Tournaments);




