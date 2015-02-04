'use strict';

(function (This) {
    This.User = Backbone.Model.extend({
        urlRoot: '/OData/Users',
        idAttribute: "Id",

        defaults: function () {
            return {
                UserName: '',
                Email: '',
                Password: '',
                FullName: '',
                CellPhone: ''
            };
        }, 

        validation: {
            Name: [{
                required: true,
                msg: 'Поле не может быть пустым'
            }, {
                maxLength: 60,
                msg: 'Поле не может содержать более 60 символов'
            }, {
                pattern: 'lettersOnly',
                msg: 'Поле должно содержать только буквы'
            }],
            Email: {
                required: true,
                msg: 'Неправильный формат почтового ящика',
                pattern: 'email'
            },
            CellPhone: {
                maxLength: 10,
                msg: 'Указан недействительный номер телефона',
                pattern: 'digits'
            },
            Password: {
                required: true,
                msg: 'Пароль и подтверждение пароля не совпадают'
            },
            FullName: {
                maxLength: 60,
                pattern: 'lettersOnly'
            }
        }
    });
})(App.Users);