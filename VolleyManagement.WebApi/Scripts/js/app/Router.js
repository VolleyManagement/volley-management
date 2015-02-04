'use strict';

(function (This)  {
    This.Router = Backbone.Router.extend({
        routes: {
            '': 'index',
            'Tournaments*path': 'tournaments',
            'Users*path': 'users',
            'About*path': 'servicePages',
            '*path': 'notFound'
        },

        tournaments: function (path) {
            vm.subRouter = new App.Tournaments.Router(path);
        },

        users: function (path) {
            vm.subRouter = new App.Users.Router(path);
        },

        servicePages: function (path) {
            vm.subRouter = new App.ServicePages.Router(path);
        },

        notFound: function (path) {
            console.log('404 Not Found %s', path);
        }
    });
})(App);