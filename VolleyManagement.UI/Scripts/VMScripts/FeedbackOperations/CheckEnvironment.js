;$(document).ready(function () {

    var environment = navigator.userAgent;
    $('#isUserAgree').prop('checked', true);
    $('#UserEnvironment').val(environment);

    $("#isUserAgree").change(function () {
        if (this.checked) {
            $('#UserEnvironment').val(environment);
        }
        else {
            $('#UserEnvironment').val('');
        }
    });

    $('#feedbackForm').submit(function() {
        if (!$('#isUserAgree').prop('checked')) {
            $('#UserEnvironment').val('');
        }
    });
});