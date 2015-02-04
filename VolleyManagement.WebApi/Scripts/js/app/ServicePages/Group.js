'use strict';

(function (This)  {
    This.Group = Backbone.Model.extend({
        urlRoot: '/OData/about',
        
        defaults: {
            name: '',
            contributors: [],
            itaName: ''
        }
    });
})(App.ServicePages);