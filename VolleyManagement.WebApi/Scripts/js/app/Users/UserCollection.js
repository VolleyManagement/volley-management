'use strict';

(function (This) {
    This.UserCollection = Backbone.Collection.extend({
        url: '/OData/Users',

        model: This.User
    });
})(App.Users);