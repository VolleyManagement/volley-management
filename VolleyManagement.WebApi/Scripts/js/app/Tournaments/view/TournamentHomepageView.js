"use strict";

(function (This) {
    This.TournamentHomepageView = Backbone.View.extend({
        tagName: 'div',
        className: 'homepage',

        template: tournamentHomepageTpl,

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
            vm.mediator.publish('ShowTournaments');
        },

        edit: function () {
            this.remove();
            
            vm.mediator.publish('EditTournament', this.model);
        },

        confirmDelete: function () {
            vm.mediator.publish('Popup', 'Вы действительно хотите удалить турнир?', this.delete.bind(this));
        },

        delete: function () {
            this.model.destroy();

            vm.mediator.publish('ShowTournaments');   

            vm.mediator.publish('Notice', 'success', 'Турнир успешно удален!');
        }
    });
})(App.Tournaments);