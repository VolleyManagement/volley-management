'use strict';

(function (This) {
    This.CreateEditView = Backbone.View.extend({
        tagName: 'div',

        template: editTournamentTpl,

        events: {
            'click .save': 'save',
            'click .cancel': 'cancel'
        },

        initialize: function () {
            this.model = this.model || new This.Tournament();
            this.model.on('change', this.preValidate, this);
            this.defaultModelJSON = this.model.toJSON();
            
            this.modelBinder = new Backbone.ModelBinder();
            
            Backbone.Validation.bind(this);
            
            vm.mediator.subscribe('ShowTournamentById', this.undoChanges, {}, this);
        },

        render: function () {
            this.$el.append(this.template(this.model.toJSON()));

            this.modelBinder.bind(this.model, this.el);

            return this;
        },

        preValidate: function () {
            var errors = this.model.preValidate({
                    Name: this.model.get('Name'),
                    Description: this.model.get('Description')
                }), 
                attrName;

            if (errors) {
                for (attrName in errors) {
                    vm.mediator.publish('Hint', errors[attrName], this.$('[name=' + attrName + ']'));
                }
            }
            return errors;
        },

        save: function () {

            if (!this.preValidate()) {
                this.model.save();
                
                if (this.model.isNew()) {
                    vm.mediator.publish('TournamentSaved', this.model);
                }
                
                vm.mediator.publish('TournamentsViewClosed',
                    this.model.isNew()? 'afterCreating': 'afterEditing', 
                    this.model.id
                );
                
                vm.mediator.publish('Notice', 'success', 
                    this.model.isNew()? 'Турнир успешно создан': 'Турнир успешно изменён'
                );
            }  
        },

        cancel: function () {
            this.undoChanges();

            vm.mediator.publish('TournamentsViewClosed',
                this.model.isNew()? 'afterCreating': 'afterEditing', 
                this.model.id
            );
        },

        undoChanges: function () {
            this.modelBinder.unbind();
            this.model.off('change', this.preValidate);
            this.model.set(this.defaultModelJSON);
        }
    });
})(App.Tournaments);