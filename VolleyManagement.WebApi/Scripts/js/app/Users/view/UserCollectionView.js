'use strict';

(function (This) {
    This.UserCollectionView = Backbone.View.extend({
        tagName: 'div',
        className: 'userCollection',

        template: userCollectionTpl,

        initialize: function () {
            this.collection = new This.UserCollection();
            this.collection.on('add', this.addOne, this);

            vm.mediator.subscribe('UserSaved', this.saveModel, {}, this);

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

        addOne: function (user) {
            var view = new This.UserView({model: user});
            this.$('ul').append(view.render().el);
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
})(App.Users);