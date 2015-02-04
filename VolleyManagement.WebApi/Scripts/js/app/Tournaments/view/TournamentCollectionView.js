'use strict';

(function (This) {
    This.TournamentCollectionView = Backbone.View.extend({
        tagName: 'div',
        className: 'tournament-collection',

        template: tournamentCollectionTpl,

        events: {
            'click button.create': 'create'
        },

        initialize: function () {
            this.collection = new This.TournamentCollection();
            this.collection.on('add', this.addOne, this);
            
            vm.mediator.subscribe('TournamentSaved', this.saveModel, {}, this);

            this.update();
        },

        saveModel: function (model) {
            this.collection.add(model);
        },

        update: function () {
            this.collection.fetch();
        },

        render: function () {
            this.$el.html(this.template());

            return this;
        },

        addOne: function (tournament) {
            var view = new This.TournamentView({model: tournament});
            console.log(tournament);
            this.$('ul').append(view.render().el);
        },

        create: function () {
            vm.mediator.publish('CreateTournament');
        },

        show: function () {
            this.$el.removeClass('hidden');
        },

        hide: function () {
            this.$el.addClass('hidden');
        },

        getModelById: function (id, callback) {
            if (this.collection.get(id)) {
                callback(this.collection.get(id));
            } else {
                this.collection.once('sync', function () {
                    callback(this.collection.get(id));
                }, this);
            }
        }
    });
})(App.Tournaments);