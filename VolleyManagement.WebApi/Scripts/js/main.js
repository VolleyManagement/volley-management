"use strict";

var App = {
        Messenger: {},
        Tournaments: {}
    },
    vm  = {
    	mediator: new Mediator(),
    	messenger: {},
    	tournaments: {}
    };

//app - hash with Constructors
//vm - hash with objects like mediator

$(function () {
    vm.messenger = new App.Messenger.Controller();

    vm.tournaments.mediator = new Mediator();
    vm.tournaments.router = new App.Tournaments.TournamentRouter();

    Backbone.history.start({pushState: true});
});
