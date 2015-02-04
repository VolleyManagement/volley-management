'use strict';

(function (This)  {
    This.Controller = function (router) {
        var $service = $('#main'),
            view;

        vm.mediator.subscribe('ShowAbout', showAbout);
        vm.mediator.subscribe('Show404', showErrorPage);  

        function showAbout () {
            view = new This.GroupCollectionView();

            router.navigate('About');
            $service.append(view.render().el);
        }

        function showErrorPage () {
            /*view = new This.GroupCollectionView();

            router.navigate('About');
            $service.append(view.render().el);    */
        }

        return this;
    }
})(App.ServicePages);