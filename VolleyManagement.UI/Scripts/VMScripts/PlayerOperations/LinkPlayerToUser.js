;$(document).ready(function () {
    ;(function () {

        var messagePlace = $('#ajaxResultMessage');
        var linkButton = $("#linkUser");

        linkButton.click(function () {

            var playerId = $('#Model_Id').val();
            var linkMessage = $('#linkMessage').html();

            $.ajax({
                url: "/Players/LinkWithUser",
                datatype: 'json',
                data: { id: playerId },
                success: function (message) {
                    messagePlace.html(message + linkMessage);
                    linkButton.prop('disabled', true);
                },
                error: function () {
                    messagePlace.html('ERROR');
                }
            });
        });

    })();
});



