'use strict';

(function (This)  {
    This.Router = Backbone.Router.extend({
        routes: {
            'Users': 'getUsers',
            'Users/new': 'createUser',
            'Users/:id/edit': 'editUser',
            'Users/:id': 'getUser'
        },
        
        initialize: function (path) {
            this.controller = new App.Users.Controller(this);

            Backbone.history.loadUrl(Backbone.history.fragment);
        },

        getUsers: function () {
            vm.mediator.publish('ShowUsers');
        },

        getUser: function (id) {
            vm.mediator.publish('ShowUserById', id);
        },

        createUser: function () {
            vm.mediator.publish('CreateUser');
        },

        editUser: function (id) {
            vm.mediator.publish('EditUserById', id);
        }
    });
})(App.Users);