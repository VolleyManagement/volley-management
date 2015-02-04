"use strict";

(function (This) {
    This.Controller = function () {
        var noticeView = new This.NoticeView(),
            hintView = new This.HintView(),
            popupView = new This.PopupView();

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

        this.notice = showNotice;
        this.hint = showHint;
        this.popup = showPopup;

        return this;
    }
})(App.Messenger);