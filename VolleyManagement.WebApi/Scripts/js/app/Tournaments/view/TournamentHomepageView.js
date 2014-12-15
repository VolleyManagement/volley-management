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
		
		initialize: function () {
            this.modelBinder = new Backbone.ModelBinder();
		},

        render: function () {
            this.$el.append(this.template(this.model.toJSON()));
			
            this.modelBinder.bind(this.model, this.el);

            return this;
        },

        cancel: function () {
            this.modelBinder.unbind();
			
            mediator.publish('TournamentViewClosed');
        },

        edit: function () {
            this.remove();
            
            mediator.publish('EditTournament', this.model);
        },

        confirmDelete: function () {
            mediator.publish('Popup', 'Вы действительно хотите удалить турнир?', this.delete.bind(this));
        },

        delete: function () {
            this.model.destroy();

            mediator.publish('TournamentViewClosed');   
        }
    });
})(App.Tournaments);