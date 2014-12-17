"use strict";

(function (This, scope) {
	var mediator;

    function extractMediator () {
        mediator = scope.mediator;
    }

    This.CreateEditView = Backbone.View.extend({
        tagName: 'div',

        template: editTournamentTpl,

        events: {
            'click .save': 'save',
            'click .cancel': 'cancel'
        },

        initialize: function () {
        	extractMediator();

            this.model = this.model || new This.Tournament();
			this.defaultModelJSON = this.model.toJSON();
            this.modelBinder = new Backbone.ModelBinder();
			
			mediator.subscribe("ShowTournamentById", this.undoChanges, {}, this);
        },

        render: function () {
            this.$el.append(this.template(this.model.toJSON()));

            this.modelBinder.bind(this.model, this.el);

            return this;
        },

        save: function () {
			var validation = this.modelValidate();
			
            if (validation.isValid) {
                this.model.save();

                mediator.publish('TournamentSaved', this.model);
                vm.messenger.notice('success', this.model.isNew() ?
                                                      'Турнир успешно создан' :
                                                      'Турнир успешно изменён'
                );
            } else {
			    if (validation.errorText['name']) {
					vm.messenger.hint(validation.errorText['name'], this.$('input:eq(0)'));
				} else if (validation.errorText['description']) {
					vm.messenger.hint(validation.errorText['description'], this.$('textarea'));
				} else if (validation.errorText['link']) {
					vm.messenger.hint(validation.errorText['link'], this.$('input:eq(1)'));
				}
            }
        },

        cancel: function () {
            this.undoChanges();

            mediator.publish("TournamentViewClosed");
        },

        undoChanges: function () {
            this.modelBinder.unbind();
			
            this.model.set(this.defaultModelJSON);
        },
		
		modelValidate: function () {
			var errorText = {},
			    isValid = true,
				printSymbolCheck = /^[\u0020-\u007E\u00A1-\uFFFF]*$/;
			
			if (this.model.get('Name').length !== 0) {
				if (this.model.get('Name').length <= 60) {
					if (printSymbolCheck.test(this.model.get('Name'))) {
						var restNames,
							names;
							
						if (!this.model.isNew()) {
							restNames = _.without(this.model.collection.models, this.model)
						                 .map(function (model) {
											 return model.get('Name');
										 });
						}
										 
						names = this.model.isNew()? 
								this.collection.pluck('Name'):
								restNames;
						
						_.each(names, function (name) {
							if (name === this.model.get('Name')) {
								errorText['name'] = 'Турнир с таким именем уже существует в системе';
								isValid = false;
							}
						}.bind(this));
					} else {
						errorText['name'] = 'Поле должно содержать только печатаемые символы';
						isValid = false;
					}
				} else {
				    errorText['name'] = 'Поле не может содержать более 60 символов';
					isValid = false;
				}
			} else {
				errorText['name'] = 'Поле не может быть пустым';
				isValid = false;
			}
			
			if (this.model.get('Description').length <= 300) {
				if (!printSymbolCheck.test(this.model.get('Description'))) {
					errorText['description'] = 'Поле должно содержать только печатаемые символы';
					isValid = false;
				}
			} else {
				errorText['description'] = 'Поле не может содержать более 300 символов';
				isValid = false;
			}
			
			if (this.model.get('RegulationsLink').length > 255) {
				errorText['link'] = 'Поле не может содержать более 255 символов';
				isValid = false;
			} 
			
			return {
				errorText: errorText,
				isValid: isValid
			};
		}
    });
})(App.Tournaments, vm.tournaments);