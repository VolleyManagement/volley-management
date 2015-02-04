'use strict';

(function (This) {
    This.UserView = Backbone.View.extend({
        tagName: 'li',
        className: 'user list-group-item',

        template: userTpl,

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
            vm.mediator.publish('ShowUserInfo', this.model);
        }
    });
})(App.Users);