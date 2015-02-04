"use strict";

(function (This, scope) {
    var mediator;

    function extractMediator () {
        mediator = scope.mediator;
    }

    This.TournamentView = Backbone.View.extend({
        tagName: 'li',
        className: 'list-group-item',

        template: tournamentTpl,

        events: {
			'click h4': 'showInfo'
        },

        initialize: function () {
            extractMediator();

            this.model.on('change', this.render, this);
            this.model.on('remove', this.remove, this);
        },

        render: function () {
            this.$el.html(this.template(this.model.toJSON()));
            
            return this;
        },

        showInfo: function () {
            mediator.publish('ShowTournamentInfo', this.model);
        }
    });
})(App.Tournaments, vm.tournaments);