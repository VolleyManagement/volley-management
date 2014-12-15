"use strict";

(function (This)  {
    This.Controller = function () {
        var tournaments = new This.TournamentCollectionView(),
            $tournaments = $('#tournaments'),
            view;
        
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
            tournamentRouter.navigate('Tournaments');
        }

        function createView () {
			view && view.remove();
            view = new This.CreateEditView();
			view.collection = tournaments.collection;
			
            tournamentRouter.navigate('Tournaments/new');
            tournaments.hide();
            $tournaments.append(view.render().el);
        }

        function editView (tournament) {
			view && view.remove();
            view = new This.CreateEditView({model: tournament});
			
            tournamentRouter.navigate('Tournaments/edit/' + tournament.id);
            tournaments.hide();
            $tournaments.append(view.render().el);
        }

        function showView(tournament) {
            console.log(tournament);
			view && view.remove();
            view = new This.TournamentHomepageView({model: tournament});
			
            tournamentRouter.navigate('Tournaments/' + tournament.id);
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
})(App.Tournaments);