'use strict';

(function (This) {
    This.NoticeView = Backbone.View.extend({
        tagName: 'div',
        className: 'hidden notice',

        template: noticeTpl,

        set: function (type, message) {
            this.type = type;
            this.message = message;
        },

        events: {
            'click .close': 'hide'
        },

        render: function () {
            this.$el.html(this.template({type: this.type, message: this.message}))
                .removeClass('hidden');

            this.timeout = setTimeout(this.hide.bind(this), 5000);

            return this;
        },

        hide: function () {
            this.$el.addClass('hidden');
            clearTimeout(this.timeout);
        }
    });
})(App.Messenger);