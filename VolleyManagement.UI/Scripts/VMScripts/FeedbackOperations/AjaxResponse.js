'use strict';
$(document).ready(function () {
    (function () {
        $('#send-feedback').click(function (event) {

            event.preventDefault();

            var capthaResponsePlace = $('#capthaResponsePlace');
            var feedbackResponsePlace = $('#responsePlace');
            var feedbackForm = $("#feedbackForm");
            var waitPlace = $('#waitPlace');

            capthaResponsePlace.html('');
            feedbackResponsePlace.html('');

            feedbackForm.validate();
            if (feedbackForm.valid() === true) {

                $.ajax({

                    url: '/Feedbacks/Create',

                    type: 'POST',

                    data: {
                        captchaResponse: grecaptcha.getResponse(),
                        UsersEmail: $('#UsersEmail').val(),
                        Content: $('#Content').val(),
                        UserEnvironment: $('#UserEnvironment').val()
                    },

                    dataType: 'json',

                    success: function (response) {
                        if (response.OperationSuccessful === true) {
                            feedbackResponsePlace.html('<span>' + response.ResultMessage + '</span>');
                        }
                        else {
                            capthaResponsePlace.html('<span>' + response.ResultMessage + '</span>');
                        }
                    },

                    error: function () {
                        alert('Error');
                    },

                    beforeSend: function () {
                        waitPlace.html('<img src="/Content/ajax-loader.gif" alt="Wait" />');
                    },

                    complete: function () {
                        waitPlace.html('');
                    }
                });
            }
        });
    })();
});