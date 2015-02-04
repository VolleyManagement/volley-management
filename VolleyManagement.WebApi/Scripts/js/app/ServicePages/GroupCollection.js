'use strict';

(function (This) {
    This.GroupCollection = Backbone.Collection.extend({
        url: '/OData/About',

        model: This.Group
    });

})(App.ServicePages);