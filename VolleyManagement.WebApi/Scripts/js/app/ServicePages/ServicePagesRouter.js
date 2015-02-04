'use strict';

(function (This)  {
    This.Router = Backbone.Router.extend({
        routes: {
            'About': 'showAbout'
        },

        initialize: function (path) {
            this.controller = new App.ServicePages.Controller(this);

            Backbone.history.loadUrl(Backbone.history.fragment);
        },

        showAbout: function () {
            vm.mediator.publish('ShowAbout');
        }
    });
})(App.ServicePages);