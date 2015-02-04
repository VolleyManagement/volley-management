'use strict';

var App = {
        Messenger: {},
        Tournaments: {},
        Users: {},
        ServicePages: {}
    },
    vm  = {
        mediator: new Mediator(),
        messenger: {},
        tournaments: {},
        users: {},
        servicePages: {},
        subRouter: {},
        router: {}
    };

$(function () {
    vm.messenger = new App.Messenger.Controller();
    vm.router = new App.Router();
    
    Backbone.history.start({pushState: true});
});