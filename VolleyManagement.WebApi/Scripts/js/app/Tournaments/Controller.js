'use strict';

(function (This)  {
    This.Controller = function (router) {
        var tournaments = new This.TournamentCollectionView(),
            $tournaments = $('#main'),
            view;
        
        start();     
        
        function start () {
            setUpMediator();
            $tournaments.append(tournaments.render().el);
        }
        
        function setUpMediator () {
            vm.mediator.subscribe('ShowTournaments', showAll); 
            vm.mediator.subscribe('ShowTournamentInfo', showView);
            vm.mediator.subscribe('ShowTournamentById', showViewById);
            
            vm.mediator.subscribe('CreateTournament', createView);
            vm.mediator.subscribe('EditTournament', editView);
            vm.mediator.subscribe('EditTournamentById', editViewById);

            vm.mediator.subscribe('TournamentsViewClosed', viewClosed);
        }

        function showAll () {
            view && view.remove();
            
            tournaments.show();
            router.navigate('Tournaments');
        }

        function createView () {
            view && view.remove();
            view = new This.CreateEditView();
            
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

        function showView (tournament) {
            view && view.remove();
            view = new This.TournamentHomepageView({model: tournament});
            
            router.navigate('Tournaments/' + tournament.id);
            tournaments.hide();
            $tournaments.append(view.render().el);
        }

        function editViewById (id) {
            tournaments.getModelById(id, editView);
        }

        function viewClosed (reason, id) {
            if (reason === 'afterCreating') {
                showAll();
            } else {
                showViewById(id);
            }
        }

        function showViewById (id) {
            tournaments.getModelById(id, showView);
        }

        return this;
    }
})(App.Tournaments);