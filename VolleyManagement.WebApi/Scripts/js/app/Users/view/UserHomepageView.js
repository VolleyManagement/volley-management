"use strict";

(function (This) {
    This.UserHomepageView = Backbone.View.extend({
        tagName: 'div',
        className: 'homepage',

        template: userHomepageTpl,

        events: {
            'click .cancel': 'cancel',
            'click .edit': 'edit',
            'click .delete': 'confirmDelete'
        },

        render: function () {
            this.$el.append(this.template(this.model.toJSON()));

            return this;
        },

        cancel: function () {
            vm.mediator.publish('ShowUsers');
        },

        edit: function () {
            this.remove();
            
            vm.mediator.publish('EditUser', this.model);
        },

        confirmDelete: function () {
            vm.mediator.publish('Popup', 'Вы действительно хотите удалить свой профиль?', this.delete.bind(this));
        },

        delete: function () {
            this.model.destroy();

            vm.mediator.publish('ShowUsers');   

            vm.mediator.publish('Notice', 'success', 'Профиль успешно удален!');
        }
    });
})(App.Users);