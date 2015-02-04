'use strict';

(function (This) {
    This.GroupView = Backbone.View.extend({
        tagName: 'div',
        className: 'group-container',

        template: groupTpl,

        initialize: function () {
            this.model.on('change', this.render, this);
            this.model.on('remove', this.remove, this);
        },

        render: function () {
            this.$el.html(this.template(this.model.toJSON()));

            return this;
        }
    });
})(App.ServicePages);