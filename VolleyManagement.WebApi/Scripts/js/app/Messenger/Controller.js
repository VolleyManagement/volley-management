'use strict';

(function (This) {
    This.Controller = function () {
        var noticeView = new This.NoticeView(),
            popupView = new This.PopupView();

        vm.mediator.subscribe("Notice", showNotice);
        vm.mediator.subscribe("Hint", showHint);
        vm.mediator.subscribe("Popup", showPopup);

        $('#messenger').html(noticeView.el);
        $('#popup').html(popupView.el);

        function showNotice (type, message) {
            noticeView.set(type, message);

            noticeView.render();
        }

        function showHint (message, $target) {
            var hintView = new This.HintView();
            
            hintView.set(message, $target);

            $target.parent().prepend(hintView.render().el);
        }

        function showPopup (message, callback) {
            popupView.set(message, callback);

            popupView.render();
        }

        return this;
    }
})(App.Messenger);