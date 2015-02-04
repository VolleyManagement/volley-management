'use strict';

(function (This) {
    This.Controller = function (router) {
        var users = new This.UserCollectionView(),
            $users = $('#main'),
            view;

        start();
        
        function start () {
            setUpMediator();
            $users.append(users.render().el);
        }

        function setUpMediator () {
            vm.mediator.subscribe('CreateUser', createView);
            
            vm.mediator.subscribe('EditUser', editView);
            vm.mediator.subscribe('EditUserById', editViewById);
            
            vm.mediator.subscribe('ShowUserInfo', showView);
            vm.mediator.subscribe('ShowUsers', showAll);
            vm.mediator.subscribe('ShowUserById', showViewById);

            vm.mediator.subscribe('UsersViewClosed', viewClosed);
        }


        function showAll () {
            view && view.remove();

            users.show();
            router.navigate('Users');
        }

        function createView () {
            view && view.remove();
            view = new This.CreateEditView();
            
            router.navigate('Users/new');
            
            users.hide();
            $users.append(view.render().el);
        }

        function editView (user) {
            view && view.remove();
            view = new This.CreateEditView({model: user});    
            router.navigate('Users/' + user.id + '/edit');
            users.hide();
            $users.append(view.render().el);
        }

        function showView (user) {
            view && view.remove();
            view = new This.UserHomepageView({model: user});    
            router.navigate('Users/' + user.id);
            users.hide();
            $users.append(view.render().el);
        }

        function editViewById (id) {
            users.getModelById(id, editView);
        }

        function viewClosed (reason, id) {
            if (reason === 'afterCreating') {
                showAll();
            } else {
                showViewById(id);
            }
        }

        function showViewById (id) {
            users.getModelById(id, showView);
         }

        return this;
    }
})(App.Users);