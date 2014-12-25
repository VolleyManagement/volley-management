'use strict';

(function (This) {
    This.TournamentView = Backbone.View.extend({
        tagName: 'li',
        className: 'tournament list-group-item',

        template: tournamentTpl,

        events: {
            'click': 'showInfo'
        },

        initialize: function () {
            this.model.on('change', this.render, this);
            this.model.on('remove', this.remove, this);
        },

        render: function () {
            this.$el.html(this.template(this.model.toJSON()));
            
            return this;
        },

        showInfo: function () {
            vm.mediator.publish('ShowTournamentInfo', this.model);
        }
    });
})(App.Tournaments);