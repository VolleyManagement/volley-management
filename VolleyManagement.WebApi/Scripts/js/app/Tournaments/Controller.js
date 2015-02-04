"use strict";

(function (This, scope)  {
    var mediator;

    function extractMediator () {
        mediator = scope.mediator;        
    }

    This.Controller = function (router) {
        var tournaments = new This.TournamentCollectionView(),
            $tournaments = $('#tournaments'),
            view;
        
        extractMediator();

        mediator.subscribe('CreateTournament', createView);
        mediator.subscribe('EditTournament', editView);
        mediator.subscribe('ShowTournamentInfo', showView);
        mediator.subscribe('ShowTournaments', showAll);
        mediator.subscribe('TournamentSaved', showAll);
        mediator.subscribe('TournamentViewClosed', showAll);
        mediator.subscribe('EditTournamentById', editViewById, {}, this);
        mediator.subscribe('ShowTournamentById', showViewById, {}, this);

        start();
        
        function start () {
            $tournaments.append(tournaments.render().el);
        }
        
        function showAll () {
            view && view.remove();
			
            tournaments.show();
            router.navigate('Tournaments');
        }

        function createView () {
			view && view.remove();
            view = new This.CreateEditView();
			view.collection = tournaments.collection;
			
            router.navigate('Tournaments/new');
            tournaments.hide();
            $tournaments.append(view.render().el);
        }

        function editView (tournament) {
			view && view.remove();
            view = new This.CreateEditView({model: tournament});
			
            router.navigate('Tournaments/' + tournament.id + '/edit');
            tournaments.hide();
            $tournaments.append(view.render().el);
        }

        function showView(tournament) {
			view && view.remove();
            view = new This.TournamentHomepageView({model: tournament});
			
            router.navigate('Tournaments/' + tournament.id);
            tournaments.hide();
            $tournaments.append(view.render().el);
        }

        function editViewById (id) {
            tournaments.getModelById(id, editView);
        }

        function showViewById (id) {
            tournaments.getModelById(id, showView);
        }
		
        return this;
    }
})(App.Tournaments, vm.tournaments);