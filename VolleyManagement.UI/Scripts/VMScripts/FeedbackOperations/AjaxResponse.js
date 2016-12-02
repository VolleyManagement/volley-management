'use strict';
$(document).ready(function () {
    (function () {
        $('#send-feedback').click(function () {

            clearFields();

            if (fieldValidation() === true) {

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
                            $('#responsePlace')
                                .html('<span>' + response.ResultMessage + '</span>');
                        }
                        else {
                            $('#capthaResponsePlace')
                                .html('<span>' + response.ResultMessage + '</span>')
                                .addClass('field-validation-color');
                        }
                    },
                    error: function () {
                        alert('Error');
                    }
                });
            };
        });
    })();
});

var fieldValidation = function () {

    if ($('#UsersEmail').val() == "") {
        $('#emailValidPlace')
            .html('Field "Email" is required')
            .addClass('field-validation-color');

        //regex
        return false;
    }

    if ($('#Content').val() == "") {
        $('#contentValidPlace')
            .html('Field "Content" is required')
            .addClass('field-validation-color');
        return false;
    }

    return true;
};

var clearFields = function () {

    $('#emailValidPlace').html('');
    $('#contentValidPlace').html('');
    $('#capthaResponsePlace').html('');
    $('#responsePlace').html('');
};