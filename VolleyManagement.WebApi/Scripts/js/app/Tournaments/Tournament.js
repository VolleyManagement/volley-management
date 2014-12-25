'use strict';

(function (This)  {
    This.Tournament = Backbone.Model.extend({
        urlRoot: '/OData/Tournaments',
        idAttribute: "Id",

        defaults: function () {
            var year = (new Date()).getFullYear();

            return {
                Name: '',
                Description: '',
                Season: (year + "/" + (year+1)),
                Scheme: '1',
                RegulationsLink: ''
            };
        },

        validation: {
            Name: [{
                required: true,
                msg: 'Поле не может быть пустым'
            }, {
                maxLength: 60,
                msg: 'Поле не может содержать более 60 символов'
            }],
            Description: {
                maxLength: 300,
                msg: 'Поле не может содержать более 300 символов',
            },
            Season: {
                required: true,
                msg: 'Поле не может быть пустым'
            },
            Scheme: {
                required: true,
                msg: 'Поле не может быть пустым'
            }
        }
    });
})(App.Tournaments);