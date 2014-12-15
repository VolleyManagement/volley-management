"use strict";

(function (This) {
    This.Controller = function () {
        var noticeView = new This.NoticeView(),
            hintView = new This.HintView(),
            popupView = new This.PopupView();

        mediator.subscribe("Notice", showNotice);
        mediator.subscribe("Hint", showHint);
        mediator.subscribe("Popup", showPopup);

        $('#messenger').html(noticeView.el);
        $('#popup').html(popupView.el);

        function showNotice (type, message) {
            noticeView.set(type, message);

            noticeView.render();
        }

        function showHint (message, $target) {
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