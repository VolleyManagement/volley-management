'use strict';

(function (This) {
    This.CreateEditView = Backbone.View.extend({
        tagName: 'div',

        template: editUserTpl,

        events: {
            'click .save': 'save',
            'click .cancel': 'cancel',
            'blur input': 'preValidate'
        },

        initialize: function () {
            this.model = this.model || new This.User();
            this.defaultModelJSON = this.model.toJSON();
            this.modelBinder = new Backbone.ModelBinder();

            Backbone.Validation.bind(this);
            
            vm.mediator.subscribe('ShowUserById', this.undoChanges, {}, this);
        },

        render: function () {
            this.$el.append(this.template(this.model.toJSON()));
            
            this.modelBinder.bind(this.model, this.el);

            return this;
        },

        preValidate: function (e) {
            if (e) {
                var attrName = e.target.name,
                    error;

                error = this.model.preValidate(
                    attrName, this.model.get(attrName)
                );

                if (error) {
                    vm.mediator.publish('Hint', error, this.$('[name=' + attrName + ']'));
                }

                return error;
            } else {
                var errors = this.model.preValidate({
                    UserName: this.model.get('UserName'),
                    Email: this.model.get('Email'),
                    CellPhone: this.model.get('CellPhone'),
                    FullName: this.model.get('FullName'),
                    Password: this.model.get('Password'),
                    ConfirmPassword: this.model.get('ConfirmPassword')
                }), 
                attrName;

                if (errors) {
                    for (attrName in errors) {
                        vm.mediator.publish('Hint', errors[attrName], this.$('[name=' + attrName + ']'));
                    }
                }

                return errors;
            }        
        },

        save: function () {

            if (!this.preValidate()) {
                this.model.save();

                if (this.model.isNew()) {
                    vm.mediator.publish('UserSaved', this.model);
                }
                
                vm.mediator.publish('UsersViewClosed',
                    this.model.isNew()? 'afterCreating': 'afterEditing', 
                    this.model.id
                );

                vm.mediator.publish('Notice', 'success', this.model.isNew()?
                    'Вы успешно зарегистрировались!':
                    'Информация успешно изменена!'
                );
            }
        },

        cancel: function () {
            this.undoChanges();
            vm.mediator.publish('UsersViewClosed',
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
})(App.Users);