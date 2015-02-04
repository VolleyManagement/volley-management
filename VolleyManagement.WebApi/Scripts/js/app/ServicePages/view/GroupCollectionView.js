'use strict';

(function (This) {
    This.GroupCollectionView = Backbone.View.extend({
        tagName: 'div',

        template: groupCollectionTpl,

        initialize: function () {
            this.collection = new This.GroupCollection();
            this.collection.on('add', this.addOne, this);
            this.collection.fetch();
        },

        render: function () {
            this.$el.html(this.template());

            return this;
        },

        addOne: function (group) {
            var view = new This.GroupView({model: group});
              
            this.$('.list-group-container').append(view.render().el);
        }
    });
})(App.ServicePages);