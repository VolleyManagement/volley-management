"use strict";

var mediator, messenger, tournamentRouter,
    App = {
        Messenger: {},
        Tournaments: {}
    },
    vm  = {};

//app - hash with Constructors
//vm - hash with objects like mediator

$(function () {
    mediator = new Mediator();
    messenger = new App.Messenger.Controller();
    tournamentRouter = new App.Tournaments.TournamentRouter();

    Backbone.history.start({pushState: true});
});
